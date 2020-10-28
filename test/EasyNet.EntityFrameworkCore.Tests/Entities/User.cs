using System.ComponentModel.DataAnnotations.Schema;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Entities.Auditing;

namespace EasyNet.EntityFrameworkCore.Tests.Entities
{
    [Table("Users")]
    public class User : Entity<long>
    {
        public string Name { get; set; }

        public Status Status { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    }

    public enum Status
    {
        Active = 0,
        Inactive = -1
    }
}
