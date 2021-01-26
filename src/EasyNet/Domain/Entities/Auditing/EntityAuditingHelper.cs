using System;
using System.Linq;
using EasyNet.Timing;
using EasyNet.Extensions;

namespace EasyNet.Domain.Entities.Auditing
{
    public static class EntityAuditingHelper
    {
        public static void SetCreationAuditProperties(object entity, object userId)
        {
            if (entity == null)
            {
                // Return if the entity is null.
                return;
            }

            if (entity is IHasCreationTime hasCreationTimeEntity)
            {
                if (hasCreationTimeEntity.CreationTime == default)
                {
                    hasCreationTimeEntity.CreationTime = Clock.Now;
                }
            }

            if (entity is ICreationAudited || entity.GetType().HasImplementedRawGeneric(typeof(ICreationAudited<>)))
            {
                SetUserIdType(entity, userId, typeof(ICreationAudited<>), "CreatorUserId");
            }
        }

        public static void SetModificationAuditProperties(object entity, object userId)
        {
            if (entity == null)
            {
                // Return if the entity is null.
                return;
            }

            if (entity is IHasModificationTime hasCreationTimeEntity)
            {
                hasCreationTimeEntity.LastModificationTime = Clock.Now;
            }

            if (entity is IModificationAudited || entity.GetType().HasImplementedRawGeneric(typeof(IModificationAudited<>)))
            {
                SetUserIdType(entity, userId, typeof(IModificationAudited<>), "LastModifierUserId");
            }
        }

        public static void SetDeletionAuditProperties(object entity, object userId)
        {
            if (entity == null)
            {
                // Return if the entity is null.
                return;
            }

            if (entity is ISoftDelete softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
            }

            if (entity is IHasDeletionTime hasCreationTimeEntity)
            {
                hasCreationTimeEntity.DeletionTime = Clock.Now;
            }

            if (entity is IDeletionAudited || entity.GetType().HasImplementedRawGeneric(typeof(IModificationAudited<>)))
            {
                SetUserIdType(entity, userId, typeof(IDeletionAudited<>), "DeleterUserId");
            }
        }

        private static void SetUserIdType(object entity, object userId, Type auditedGenericType, string userIdPropertyName)
        {
            var entityType = entity.GetType();
            var interfaces = entityType.GetInterfaces();

            // Try to get interface IAudited<TUserPrimaryKey>.
            var auditedOfUserPrimaryKeyInterface = interfaces.SingleOrDefault(p => p.Name == auditedGenericType.Name);
            if (auditedOfUserPrimaryKeyInterface != null)
            {
                var arguments = auditedOfUserPrimaryKeyInterface.GetGenericArguments();

                if (arguments.Length == 1)
                {
                    // Try to get property info of the lastModifierUserIdProperty.
                    var userIdProperty = entityType.GetProperty(userIdPropertyName);
                    if (userIdProperty == null)
                    {
                        throw new EasyNetException($"Cannot found property {userIdPropertyName} in the entity {entityType.AssemblyQualifiedName}.");
                    }

                    userIdProperty.SetValue(entity, userId, arguments.First());
                }
                else
                {
                    throw new InvalidOperationException($"Cannot found correct user id type in the {entityType.AssemblyQualifiedName} for type {auditedGenericType.AssemblyQualifiedName}.");
                }
            }
        }
    }
}
