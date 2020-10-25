using System.ComponentModel.DataAnnotations.Schema;
using EasyNet.Domain.Entities;

namespace EasyNet.EntityFrameworkCore.Tests.Entities
{
    [Table("Books")]
    public class Book : Entity<long>
    {
        public string Name { get; set; }
    }
}
