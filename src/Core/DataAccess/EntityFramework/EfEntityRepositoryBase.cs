using Core.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections;
using System.Reflection;

namespace Core.DataAccess.EntityFramework;
public class EfRepositoryBase<TEntity, TEntityId, TContext>
    : IEntityRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TContext : DbContext,new()
{


    public TEntity? Get(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true)
    {
        using (TContext Context = new TContext())
        {
            IQueryable<TEntity> queryable = Context.Set<TEntity>();

            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            return queryable.FirstOrDefault(predicate);
        }
    }

    public ICollection<TEntity> GetList(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true)
    {
        using (TContext Context = new TContext())
        {
            IQueryable<TEntity> queryable = Context.Set<TEntity>();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            if (predicate != null)
                queryable = queryable.Where(predicate);
            if (orderBy != null)
                return orderBy(queryable).ToList();
            return queryable.ToList();
        }
    }

    public bool Any(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true)
    {
        using (TContext Context = new TContext())
        {
            IQueryable<TEntity> queryable = Context.Set<TEntity>();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            if (predicate != null)
                queryable = queryable.Where(predicate);
            return queryable.Any();
        }
    }

    public TEntity Add(TEntity entity)
    {
        using (TContext Context = new TContext())
        {
            entity.CreatedDate = DateTime.UtcNow;
            Context.Add(entity);
            Context.SaveChanges();
            return entity;
        }
    }

    public ICollection<TEntity> AddRange(ICollection<TEntity> entities)
    {
        using (TContext Context = new TContext())
        {
            foreach (TEntity entity in entities)
                entity.CreatedDate = DateTime.UtcNow;
            Context.AddRange(entities);
            Context.SaveChanges();
            return entities;
        }
    }

    public TEntity Update(TEntity entity)
    {
        using (TContext Context = new TContext())
        {
            entity.UpdatedDate = DateTime.UtcNow;
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
            return entity;
        }
    }

    public ICollection<TEntity> UpdateRange(ICollection<TEntity> entities)
    {
        using (TContext Context = new TContext())
        {
            foreach (TEntity entity in entities)
                entity.UpdatedDate = DateTime.UtcNow;
            Context.UpdateRange(entities);
            Context.SaveChanges();
            return entities;
        }
    }

    public TEntity Delete(TEntity entity, bool permanent = false)
    {
        using (TContext Context = new TContext())
        {
            SetEntityAsDeletedAsync(entity, permanent,Context).Wait();
            Context.SaveChangesAsync().Wait();
            return entity;
        }
    }

    public ICollection<TEntity> DeleteRange(ICollection<TEntity> entities, bool permanent = false)
    {
        using (TContext Context = new TContext())
        {
            SetEntityAsDeletedAsync(entities, permanent, Context).Wait();
            Context.SaveChangesAsync().Wait();
            return entities;
        }
    }



    protected async Task SetEntityAsDeletedAsync(TEntity entity, bool permanent,TContext context)
    {
        if (!permanent)
        {
            CheckHasEntityHaveOneToOneRelation(entity,context);
            await setEntityAsSoftDeletedAsync(entity,context);
        }
        else
        {
            context.Remove(entity);
        }
    }

    protected void CheckHasEntityHaveOneToOneRelation(TEntity entity,TContext context)
    {
        bool hasEntityHaveOneToOneRelation =
            context
                .Entry(entity)
                .Metadata.GetForeignKeys()
                .All(
                    x =>
                        x.DependentToPrincipal?.IsCollection == true
                        || x.PrincipalToDependent?.IsCollection == true
                        || x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity.GetType()
                ) == false;
        if (hasEntityHaveOneToOneRelation)
            throw new InvalidOperationException(
                "Entity has one-to-one relationship. Soft Delete causes problems if you try to create entry again by same foreign key."
            );
    }

    private async Task setEntityAsSoftDeletedAsync(IEntityTimestamps entity,TContext context)
    {
        if (entity.DeletedDate.HasValue)
            return;
        entity.DeletedDate = DateTime.UtcNow;

        var navigations = context
            .Entry(entity)
            .Metadata.GetNavigations()
            .Where(x => x is { IsOnDependent: false, ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade })
            .ToList();
        foreach (INavigation? navigation in navigations)
        {
            if (navigation.TargetEntityType.IsOwned())
                continue;
            if (navigation.PropertyInfo == null)
                continue;

            object? navValue = navigation.PropertyInfo.GetValue(entity);
            if (navigation.IsCollection)
            {
                if (navValue == null)
                {
                    IQueryable query = context.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType()).ToListAsync();
                    if (navValue == null)
                        continue;
                }

                foreach (IEntityTimestamps navValueItem in (IEnumerable)navValue)
                    await setEntityAsSoftDeletedAsync(navValueItem, context);
            }
            else
            {
                if (navValue == null)
                {
                    IQueryable query = context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType())
                        .FirstOrDefaultAsync();
                    if (navValue == null)
                        continue;
                }

                await setEntityAsSoftDeletedAsync((IEntityTimestamps)navValue,context);
            }
        }

        context.Update(entity);
    }

    protected IQueryable<object> GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
    {
        Type queryProviderType = query.Provider.GetType();
        MethodInfo createQueryMethod =
            queryProviderType
                .GetMethods()
                .First(m => m is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })
                ?.MakeGenericMethod(navigationPropertyType)
            ?? throw new InvalidOperationException("CreateQuery<TElement> method is not found in IQueryProvider.");
        var queryProviderQuery =
            (IQueryable<object>)createQueryMethod.Invoke(query.Provider, parameters: new object[] { query.Expression })!;
        return queryProviderQuery.Where(x => !((IEntityTimestamps)x).DeletedDate.HasValue);
    }

    protected async Task SetEntityAsDeletedAsync(IEnumerable<TEntity> entities, bool permanent,TContext context)
    {
        foreach (TEntity entity in entities)
            await SetEntityAsDeletedAsync(entity, permanent,context);
    }

    public ICollection<TEntity> Search(string searchTerm, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true)
    {
        var parameter = Expression.Parameter(typeof(TEntity), nameof(TEntity));
        Expression predicateBody = null;

        foreach (var property in typeof(TEntity).GetProperties())
        {
            if (property.PropertyType == typeof(string))
            {
                var propertyAccess = Expression.Property(parameter, property);
                var searchTermExpression = Expression.Constant(searchTerm);

                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var containsExpression = Expression.Call(propertyAccess, containsMethod, searchTermExpression);

                predicateBody = predicateBody == null ? containsExpression : Expression.Or(predicateBody, containsExpression);
            }
        }

        var predicate = Expression.Lambda<Func<TEntity, bool>>(predicateBody, parameter);

        var result = GetList(
            predicate: predicate,
            include: null, 
            withDeleted: false,
            enableTracking: true
        );

        return result;
    }
}
