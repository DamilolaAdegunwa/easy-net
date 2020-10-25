using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EasyNet.Domain.Entities;

namespace EasyNet.EntityFrameworkCore.Tests.Entities
{
    [Table("Users")]
    public class User : Entity<long>
    {
        public string Name { get; set; }

        public Status Status { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }

    public enum Status
    {
        Active = 0,
        Inactive = -1
    }
}
