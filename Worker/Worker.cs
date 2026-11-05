using System.Globalization;
using System.IO.Ports;
using Application.Features.Samples.RegisterSample;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Worker;

public class Worker(ILogger<Worker> logger, IMediator mediator) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var attempt = 1;
        while (true)
        {
            try
            {
                try
                {
                    var serialPortName = await ResolveAvailableSerialPort(stoppingToken);
                    using var serial = InitializeSerialPort(serialPortName);
                    serial.Open();

                    await PollSerial(
                        serialConnection: serial,
                        onDataLineReceived: HandleDataReceived,
                        stoppingToken
                    );
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Unexpected exception.");
                }

                await Task.Delay(GetRetryDelay(attempt++), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Worker stopped.");
                return;
            }
        }
    }

    private async Task PollSerial(SerialPort serialConnection, Func<string, Task> onDataLineReceived, CancellationToken stoppingToken)
    {
        var attempt = 1;
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var line = serialConnection.ReadLine();
                
                await onDataLineReceived(line);
                
                attempt = 1;
            }
            catch (InvalidDataLineException ex)
            {
                logger.LogWarning(
                    "Invalid data received ({InvalidDataExceptionMessage}): {InvalidDataLine}",
                    ex.Message,
                    ex.Line
                );
                serialConnection.DiscardInBuffer();
            }
            catch (TimeoutException)
            {
                logger.LogWarning("Read operation timed out.");
            }

            var delay = GetRetryDelay(attempt++);
            
            logger.LogInformation(
                "Retrying in {Delay} seconds...",
                delay.TotalSeconds    
            );

            await Task.Delay(delay, stoppingToken);
        }
    }
    
    private async Task<string> ResolveAvailableSerialPort(CancellationToken stoppingToken)
    {
        var attempt = 1;
        while (true)
        {
            logger.LogInformation("Resolving available serial port...");
            stoppingToken.ThrowIfCancellationRequested();
            
            var availableSerialPorts = SerialPort.GetPortNames().ToList();

            if (availableSerialPorts.Count > 0)
            {
                return availableSerialPorts[0];
            }
            
            await Task.Delay(GetRetryDelay(attempt++, factor: 1.2), stoppingToken);
        }
    }

    private static SerialPort InitializeSerialPort(string serialPortName)
    {
        SerialPort? serial = null;
        try
        {
            serial = new SerialPort(serialPortName);
            serial.BaudRate = 115200;
            serial.Parity = Parity.None;
            serial.StopBits = StopBits.One;
            serial.DataBits = 8;
            serial.Handshake = Handshake.None;
            serial.ReadTimeout = 5000;
            return serial;
        }
        catch
        {
            serial?.Dispose();
            throw;
        }
    }

    private async Task HandleDataReceived(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return;
        }
        
        var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (data.Length is not 4)
        {
            throw new InvalidDataLineException("Invalid data shape", line);
        }
        if (!double.TryParse(data[0], CultureInfo.InvariantCulture, out var ph))
        {
            throw new InvalidDataLineException("Invalid PH", line);
        }
        if (!double.TryParse(data[1], CultureInfo.InvariantCulture, out var turbidity))
        {
            throw new InvalidDataLineException("Invalid turbidity", line);
        }
        if (!double.TryParse(data[2], CultureInfo.InvariantCulture, out var temperature))
        {
            throw new InvalidDataLineException("Invalid temperature", line);
        }
        if (!int.TryParse(data[3], CultureInfo.InvariantCulture, out var tds))
        {
            throw new InvalidDataLineException("Invalid tds", line);
        }

        await mediator.Send(
            new RegisterSampleCommand(
                new Sample(
                    DateTime.UtcNow,
                    SampleSourcingPoint.AfterFiltration,
                    ph,
                    turbidity,
                    temperature,
                    tds
                )
            )
        );
    }
    
    private static TimeSpan GetRetryDelay(int attempt, double factor = 2, double maxBackoff = 5)
    {
        var exponent = Math.Min(attempt - 1, maxBackoff);
        return TimeSpan.FromSeconds(Math.Pow(factor, exponent));
    }

    private class InvalidDataLineException(string message, string line) : ApplicationException(message)
    {
        public string Line { get; } = line;
    }
}