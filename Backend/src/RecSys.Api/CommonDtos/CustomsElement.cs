#pragma warning disable IDE0005
using RecSys.Customs.Client;
#pragma warning restore IDE0005

namespace RecSys.Api.CommonDtos;

public class CustomsElement
{
    public Region Region { get; init; } = null!;

    public ItemType ItemType { get; init; } = null!;

    public Country Country { get; init; } = null!;

    public TransferAmount? Export { get; init; }

    public TransferAmount? Import { get; init; }
}

public class CustomsElementForReport
{
    public string ItemType { get; init; } = null!;

    public string ItemTypeName { get; init; } = null!;

    public decimal Coef { get; init; }

    public long Region { get; init; }
}
