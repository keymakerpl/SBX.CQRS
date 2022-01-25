using System.Data;

namespace Application.Utils.Connection
{
    public interface ISqlQueriesConnectionFactory
    {
        IDbConnection CreateOpenConnection();
    }
}