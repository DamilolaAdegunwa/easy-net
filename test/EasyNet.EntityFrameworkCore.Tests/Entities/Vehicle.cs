using System.ComponentModel.DataAnnotations.Schema;
using EasyNet.Domain.Entities;

namespace EasyNet.EntityFrameworkCore.Tests.Entities
{
    [Table("Vehicles")]
    public class Vehicle : Entity
    {
        public string Name { get; set; }
    }
}
