using Application.Features.SampleEvaluator.EvaluateSample;
using Application.Features.SampleEvaluator.GetSampleEvaluator;
using Application.Features.Samples.QuerySamples;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ScreensController(IMediator mediator) : ControllerBase
{
    [HttpGet("Dashboard", Name = "GetDashboardScreenData")]
    public async Task<ActionResult<DashboardScreenData>> GetDashboardScreenData()
    {
        var samplesQuery = new SamplesPagedQuery(
            From: DateTime.UtcNow - TimeSpan.FromHours(24),
            SampleSourcingPoint: SampleSourcingPoint.AfterFiltration,
            PageSize: int.MaxValue
        );

        var recentSamplesPaged = await mediator.Send(samplesQuery);
        var recentSamples = recentSamplesPaged.Items;

        if (recentSamples.Count == 0)
        {
            return Ok(new DashboardScreenData(null, recentSamples));
        }

        var latestSample = recentSamples
            .Where(x => x.SourcingPoint is SampleSourcingPoint.AfterFiltration)
            .MaxBy(x => x.Timestamp);

        var sampleEvaluator = await mediator.Send(new SampleEvaluatorQuery());

        var latestSampleEvaluation = sampleEvaluator.Evaluate(latestSample!);

        return Ok(new DashboardScreenData(latestSampleEvaluation, recentSamples));
    }

    [HttpGet("Comparison", Name = "GetComparisonScreenData")]
    public async Task<ActionResult<object>> GetComparisonScreenData()
    {
        var samplesBeforeFiltration = await mediator.Send(new SamplesPagedQuery(PageSize: 1, SampleSourcingPoint: SampleSourcingPoint.BeforeFiltration));
        var sampleBeforeFiltration = samplesBeforeFiltration.Items.SingleOrDefault();
        var evaluatedSampleBeforeFiltration = sampleBeforeFiltration is not null
            ? await mediator.Send(new EvaluateSampleCommand(sampleBeforeFiltration))
            : null;

        var samplesAfterFiltration = await mediator.Send(new SamplesPagedQuery(PageSize: 1, SampleSourcingPoint: SampleSourcingPoint.AfterFiltration));
        var sampleAfterFiltration = samplesAfterFiltration.Items.SingleOrDefault();
        var evaluatedSampleAfterFiltration = sampleAfterFiltration is not null
            ? await mediator.Send(new EvaluateSampleCommand(sampleAfterFiltration))
            : null;

        return Ok(new ComparisonScreenData(evaluatedSampleBeforeFiltration, evaluatedSampleAfterFiltration));
    }

    [HttpGet("History", Name = "GetHistoryScreenData")]
    public async Task<ActionResult<HistoryScreenData>> GetHistoryScreenData([FromQuery] TimeSpan? fromLast)
    {
        var samplesQuery = new SamplesPagedQuery(
            From: DateTime.UtcNow - TimeSpan.FromHours(24),
            SampleSourcingPoint: SampleSourcingPoint.AfterFiltration,
            PageSize: int.MaxValue
        );

        if (fromLast is not null)
        {
            samplesQuery = samplesQuery with { From = DateTime.UtcNow - fromLast };
        }

        var samplesHistory = await mediator.Send(samplesQuery);

        var samplesHistoryPaged = await mediator.Send(
            samplesQuery with
            {
                PageSize = 5
            }
        );

        return Ok(new HistoryScreenData(samplesHistory.Items, samplesHistoryPaged));
    }

    [HttpGet("Alerts", Name = "GetAlertsAndConfigScreenData")]
    public async Task<ActionResult<AlertsAndConfigurationScreenData>> GetAlertsAndConfigScreenData()
    {
        var sampleEvaluator = await mediator.Send(new SampleEvaluatorQuery());

        var samplesAfterFiltration = await mediator.Send(new SamplesPagedQuery(PageSize: 1, SampleSourcingPoint: SampleSourcingPoint.AfterFiltration));
        var sampleAfterFiltration = samplesAfterFiltration.Items.SingleOrDefault();
        var evaluatedSampleAfterFiltration = sampleAfterFiltration is not null
            ? await mediator.Send(new EvaluateSampleCommand(sampleAfterFiltration))
            : null;

        var alerts = new List<Alert>();

        switch (evaluatedSampleAfterFiltration?.PHEvaluationResult)
        {
            case OverUpperBoundEvaluatedSampleMetric:
                alerts.Add(new Alert($"PH acima do limite definido ({evaluatedSampleAfterFiltration.PHEvaluationResult.Value})", evaluatedSampleAfterFiltration.Timestamp));
                break;

            case UnderLowerBoundEvaluatedSampleMetric:
                alerts.Add(new Alert($"PH abaixo do limite definido ({evaluatedSampleAfterFiltration.PHEvaluationResult.Value})", evaluatedSampleAfterFiltration.Timestamp));
                break;
        }

        switch (evaluatedSampleAfterFiltration?.TurbidityEvaluationResult)
        {
            case OverUpperBoundEvaluatedSampleMetric:
                alerts.Add(new Alert($"Turbidez acima do limite definido ({evaluatedSampleAfterFiltration.TurbidityEvaluationResult.Value})", evaluatedSampleAfterFiltration.Timestamp));
                break;

            case UnderLowerBoundEvaluatedSampleMetric:
                alerts.Add(new Alert($"Turbidez abaixo do limite definido ({evaluatedSampleAfterFiltration.TurbidityEvaluationResult.Value})", evaluatedSampleAfterFiltration.Timestamp));
                break;
        }

        switch (evaluatedSampleAfterFiltration?.TemperatureEvaluationResult)
        {
            case OverUpperBoundEvaluatedSampleMetric:
                alerts.Add(new Alert($"Temperatura acima do limite definido ({evaluatedSampleAfterFiltration.TemperatureEvaluationResult.Value})", evaluatedSampleAfterFiltration.Timestamp));
                break;

            case UnderLowerBoundEvaluatedSampleMetric:
                alerts.Add(new Alert($"Temperatura abaixo do limite definido ({evaluatedSampleAfterFiltration.TemperatureEvaluationResult.Value})", evaluatedSampleAfterFiltration.Timestamp));
                break;
        }

        switch (evaluatedSampleAfterFiltration?.TDSEvaluationResult)
        {
            case OverUpperBoundEvaluatedSampleMetric:
                alerts.Add(new Alert($"Condutividade acima do limite definido ({evaluatedSampleAfterFiltration.TDSEvaluationResult.Value})", evaluatedSampleAfterFiltration.Timestamp));
                break;

            case UnderLowerBoundEvaluatedSampleMetric:
                alerts.Add(new Alert($"Condutividade abaixo do limite definido ({evaluatedSampleAfterFiltration.TDSEvaluationResult.Value})", evaluatedSampleAfterFiltration.Timestamp));
                break;
        }

        return Ok(new AlertsAndConfigurationScreenData(sampleEvaluator, alerts));
    }
}

public record DashboardScreenData(EvaluatedSample? LatestSampleEvaluation, IEnumerable<Sample> Samples);

public record ComparisonScreenData(EvaluatedSample? SampleBeforeFiltration, EvaluatedSample? SampleAfterFiltration);

public record AlertsAndConfigurationScreenData(SampleEvaluator CurrentSettings, IEnumerable<Alert> Alerts);
public record Alert(string Message, DateTime Timestamp);

public record HistoryScreenData(IEnumerable<Sample> SamplesHistory, SamplesPagedQueryResult SamplesHistoryPaged);
