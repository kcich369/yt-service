using System.Linq.Expressions;
using Domain.Abstractions;

namespace Persistence.Specifications.Base;

public abstract class SelectedSpecification<TEntity, TResult> : Specification<TEntity> where TEntity : class, IEntity
{
    public Expression<Func<TEntity, TResult>> Select { get; private set; }

    protected SelectedSpecification(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TResult>> select)
        : base(criteria) => Select = select;
}