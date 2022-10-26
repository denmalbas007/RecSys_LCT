using System.Data.Common;

namespace RecSys.Platform.Data.Factories;

public interface IPostgresConnectionFactory
{
    DbConnection GetConnection();
}
