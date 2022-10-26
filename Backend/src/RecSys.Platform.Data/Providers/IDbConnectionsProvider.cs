using System.Data.Common;

namespace RecSys.Platform.Data.Providers;

public interface IDbConnectionsProvider
{
    DbConnection GetConnection();
}
