using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EasyNet.Domain.Entities;

namespace EasyNet.EntityFrameworkCore.Tests.Entities
{
    [Table("Roles")]
    public class Role : Entity
    {
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
