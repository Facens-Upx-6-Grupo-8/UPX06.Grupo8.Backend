using Application.Common;
using Application.Persistance;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.QuerySamples;

internal class QuerySamplesUseCase(AppDbContext appDbContext) : IRequestHandler<SamplesQuery, SamplesQueryResult>
{
    public async Task<SamplesQueryResult> Handle(SamplesQuery request, CancellationToken cancellationToken)
    {
        var query = appDbContext.Samples.AsQueryable();

        if (request.From.HasValue)
        {
            query = query.Where(s => DateOnly.FromDateTime(s.Timestamp) >= request.From.Value);
        }
        if (request.To.HasValue)
        {
            query = query.Where(s => DateOnly.FromDateTime(s.Timestamp) <= request.To.Value);
        }
        if (request.SampleSourcingPoint.HasValue)
        {
            query = query.Where(s => s.SourcingPoint == request.SampleSourcingPoint.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var maxPage = (int)Math.Ceiling((double)totalCount / request.PageSize);
        var page = Math.Min(request.Page, maxPage);

        query = query.OrderByDescending(s => s.Timestamp)
                     .Skip((page - 1) * request.PageSize)
                     .Take(request.PageSize);

        return new SamplesQueryResult(
            Items: await query.ToListAsync(cancellationToken),
            Page: page,
            PageSize: request.PageSize,
            LastPage: maxPage,
            TotalCount: totalCount
        );
    }
}

public record SamplesQuery(DateOnly? From = null, DateOnly? To = null, SampleSourcingPoint? SampleSourcingPoint = null, int Page = 1, int PageSize = 10) : PagedQuery(Page, PageSize), IRequest<SamplesQueryResult>;
public record SamplesQueryResult(IReadOnlyList<Sample> Items, int Page, int PageSize, int LastPage, int TotalCount) : PagedResult<Sample>(Items, Page, PageSize, LastPage, TotalCount);
