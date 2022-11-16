namespace RecSys.Api.CommonDtos;

public class CustomElementDb2
{
    public long ItemType { get; init; }

    public string Country { get; init; } = null!;

    public long? UnitType { get; init; }

    public decimal ImportWorthTotal { get; init; }

    public decimal ImportNettoTotal { get; init; }

    public decimal ImportAmountTotal { get; init; }

    public decimal ExportWorthTotal { get; init; }

    public decimal ExportNettoTotal { get; init; }

    public decimal ExportAmountTotal { get; init; }

    public long Region { get; init; }
}

// values (:ItemType, :Period, :Country, :Region, :UnitType, :ImportWorthTotal, :ImportNettoTotal, :ImportAmountTotal, :ExportWorthTotal, :ExportNettoTotal, :ExportAmountTotal)
