using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNet.Dto
{
    public interface IEntityDto : IEntityDto<int>
    {
    }

    public interface IEntityDto<out TPrimaryKey>
    {
        TPrimaryKey Id { get; }
    }
}
