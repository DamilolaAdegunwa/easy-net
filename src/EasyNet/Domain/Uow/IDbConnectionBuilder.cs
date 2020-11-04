using System.Data;
using System.Data.Common;

namespace EasyNet.Domain.Uow
{
    public interface IDbConnectionBuilder
    {
        DbConnection CreateConnection();
    }
}
