using System.Data;
using System.Data.Common;

namespace RecSys.Platform.Data.Providers;

public interface IDbTransactionsProvider
{
    DbTransaction? Current { get; }

    Task<DbTransaction> BeginTransactionAsync(CancellationToken token);

    Task<DbTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel,
        CancellationToken token);
}
