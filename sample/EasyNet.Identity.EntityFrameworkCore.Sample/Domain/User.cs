using System;
using EasyNet.Domain.Entities.Auditing;
using EasyNet.Identity.EntityFrameworkCore.Domain.Entities;

namespace EasyNet.Identity.EntityFrameworkCore.Sample.Domain
{
    public class User : EasyNetUser, IFullAudited<int>
    {
        public DateTime CreationTime { get; set; }

        public int? CreatorUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public int? LastModifierUserId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletionTime { get; set; }

        public int? DeleterUserId { get; set; }
    }
}
