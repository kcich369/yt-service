using System.Linq.Expressions;
using Domain.Abstractions;

namespace Persistence.Specifications.Base;

public class WhereIfExpressionElement<TEntity> where TEntity : class, IEntity
{
    public bool Term { get; }
    public Expression<Func<TEntity, bool>> Expression { get; }

    public WhereIfExpressionElement(bool term, Expression<Func<TEntity, bool>> expression)
    {
        Term = term;
        Expression = expression;
    }
}