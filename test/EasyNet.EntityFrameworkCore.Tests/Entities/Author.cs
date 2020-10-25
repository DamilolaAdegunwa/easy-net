using System.ComponentModel.DataAnnotations.Schema;
using EasyNet.Domain.Entities;

namespace EasyNet.EntityFrameworkCore.Tests.Entities
{
    [Table("Authors")]
    public class Author : Entity<long>
    {
        public string Name { get; set; }
    }
}
