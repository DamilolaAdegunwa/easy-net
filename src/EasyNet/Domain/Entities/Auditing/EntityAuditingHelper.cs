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

            if (entity is ICreationAudited)
            {
                var entityType = entity.GetType();
                var interfaces = entityType.GetInterfaces();

                var creationAuditedGenericInterface = interfaces.SingleOrDefault(p => p.Name == typeof(ICreationAudited<>).Name);
                if (creationAuditedGenericInterface != null)
                {
                    var creatorUserIdProperty = entityType.GetProperty("CreatorUserId");
                    if (creatorUserIdProperty == null)
                    {
                        throw new EasyNetException($"Cannot found property CreatorUserId in the entity {entityType.AssemblyQualifiedName}.");
                    }

                    var userIdType = creationAuditedGenericInterface.GetGenericArguments().FirstOrDefault();
                    if (userIdType != null)
                    {
                        creatorUserIdProperty.SetValue(entity, userId, userIdType);
                    }

                }
            }
        }
    }
}
