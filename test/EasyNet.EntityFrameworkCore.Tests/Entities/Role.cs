using System.ComponentModel.DataAnnotations.Schema;
using EasyNet.Domain.Entities;

namespace EasyNet.EntityFrameworkCore.Tests.Entities
{
    [Table("Roles")]
    public class Role : Entity
    {
        public long UserId { get; set; }

        public string Name { get; set; }

        public virtual User User { get; set; }
    }
}
