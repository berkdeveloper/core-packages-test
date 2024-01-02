using Core.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections;
using System.Reflection;

namespace Core.Persistence.Repositories.Helpers;

internal static class EntityHelper<TAggregate, TKey, TContext> where TAggregate : AggregateRoot<TKey> where TContext : DbContext
{

    #region Async
    public static async Task SetEntityAsDeletedAsync(TAggregate entity, bool permanent, TContext context)
    {
        if (!permanent)
        {
            CheckHasEntityHaveOneToOneRelation(entity, context);
            await SetEntityAsSoftDeletedAsync(entity, context);
        }
        else
        {
            context.Remove(entity);
        }
    }

    private static async Task SetEntityAsSoftDeletedAsync(IEntityTimestamps entity, TContext context)
    {
        if (entity.DeletedDate.HasValue)
            return;

        entity.SetDeletedDate(DateTime.UtcNow);

        var navigations = context
            .Entry(entity)
            .Metadata.GetNavigations()
            .Where(x => x is { IsOnDependent: false, ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade })
            .ToList();

        foreach (INavigation? navigation in navigations)
        {
            if (navigation.TargetEntityType.IsOwned())
                continue;
            if (navigation.PropertyInfo is null)
                continue;

            object? navValue = navigation.PropertyInfo.GetValue(entity);

            if (navigation.IsCollection)
            {
                if (navValue is null)
                {
                    IQueryable query = context.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();

                    navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType()).ToListAsync();

                    if (navValue is null)
                        continue;
                }

                foreach (IEntityTimestamps navValueItem in (IEnumerable)navValue)
                    await SetEntityAsSoftDeletedAsync(navValueItem, context);
            }
            else
            {
                if (navValue is null)
                {
                    IQueryable query = context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();

                    navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType())
                        .FirstOrDefaultAsync();

                    if (navValue is null)
                        continue;
                }

                await SetEntityAsSoftDeletedAsync((IEntityTimestamps)navValue, context);
            }
        }

        context.Update(entity);
    }

    public static async Task SetEntityAsDeletedAsync(IEnumerable<TAggregate> entities, bool permanent, TContext context)
    {
        foreach (TAggregate entity in entities)
            await SetEntityAsDeletedAsync(entity, permanent, context);
    }

    #endregion

    #region Sync Operation

    public static void SetEntityAsDeleted(TAggregate entity, bool permanent, TContext context)
    {
        if (!permanent)
        {
            CheckHasEntityHaveOneToOneRelation(entity, context);
            SetEntityAsSoftDeleted(entity, context);
        }
        else
        {
            context.Remove(entity);
        }
    }

    private static void SetEntityAsSoftDeleted(IEntityTimestamps entity, TContext context)
    {
        if (entity.DeletedDate.HasValue)
            return;
        entity.SetDeletedDate(DateTime.UtcNow);

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
                    navValue = GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType()).ToList();
                    if (navValue == null)
                        continue;
                }

                foreach (IEntityTimestamps navValueItem in (IEnumerable)navValue)
                    SetEntityAsSoftDeleted(navValueItem, context);
            }
            else
            {
                if (navValue == null)
                {
                    IQueryable query = context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                    navValue = GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType())
                        .FirstOrDefault();
                    if (navValue == null)
                        continue;
                }

                SetEntityAsSoftDeleted((IEntityTimestamps)navValue, context);
            }
        }

        context.Update(entity);
    }

    public static void SetEntityAsDeleted(IEnumerable<TAggregate> entities, bool permanent, TContext context)
    {
        foreach (TAggregate entity in entities)
            SetEntityAsDeleted(entity, permanent, context);
    }

    #endregion

    private static void CheckHasEntityHaveOneToOneRelation(TAggregate entity, TContext context)
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

    private static IQueryable<object> GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
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

}
