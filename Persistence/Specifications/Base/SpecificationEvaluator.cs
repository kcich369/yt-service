using Domain.Entities.Base;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Persistence.QueryableExtensions;

namespace Persistence.Specifications.Base;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> ApplySpecification<TEntity>(this IQueryable<TEntity> inputQuery,
        Specification<TEntity> specification) where TEntity : class, IEntity
    {
        var query = inputQuery;
        if (specification.Criteria is not null)
            query = query.Where(specification.Criteria);

        if (specification.IncludeExpressions.Any())
            query = specification.IncludeExpressions.Aggregate(query,
                (current, includeExpression) => current.Include(includeExpression));

        if (specification.WhereIfExpressions.Any())
            query = specification.WhereIfExpressions.Aggregate(query,
                (current, where) => current.WhereIf(where.Term, where.Expression));

        if (specification.AsNoTracking)
            query = query.AsNoTracking();
        if (specification.AsSplitQuery)
            query = query.AsSplitQuery();
        if (specification.Take.HasValue)
            query = query.Take(specification.Take.Value);

        return query;
    }

    public static IQueryable<TResult> ApplySelectedSpecification<TEntity, TResult>(this IQueryable<TEntity> inputQuery,
        SelectedSpecification<TEntity, TResult> specification) where TEntity : class, IEntity
    {
        var query = inputQuery.ApplySpecification(specification);
        if (specification.Select is null)
            throw new SelectedSpecificationException("Select expression was not implemented");

        return query.Select(specification.Select);
    }
}