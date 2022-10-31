namespace RecSys.Api.CommonDtos;

public class TransferAmount
{
    public decimal TradeSum { get; init; }

    public decimal Gross { get; init; }

    public decimal Amount { get; init; }

    public Unit? Unit { get; init; }
}
