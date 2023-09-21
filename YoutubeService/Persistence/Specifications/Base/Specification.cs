using System.Linq.Expressions;
using Domain.Abstractions;

namespace Persistence.Specifications.Base;

public abstract class Specification<TEntity> where TEntity : class, IEntity
{
    public Expression<Func<TEntity, bool>> Criteria { get; }
    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();
    public List<WhereIfExpressionElement<TEntity>> WhereIfExpressions { get; } = new();
    public bool AsNoTracking { get; protected init; }
    public bool AsSplitQuery { get; protected init; }
    
    public int? Take { get; protected init; }

    protected Specification(Expression<Func<TEntity, bool>> criteria) => Criteria = criteria;
    protected void AddInclude(Expression<Func<TEntity, object>> include) => IncludeExpressions.Add(include);

    protected void AddWhereIf(bool term, Expression<Func<TEntity, bool>> whereIfExpression) =>
        WhereIfExpressions.Add(new WhereIfExpressionElement<TEntity>(term, whereIfExpression));
}