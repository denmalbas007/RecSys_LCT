namespace RecSys.Api.CommonDtos;

public class CustomElementDb
{
    public long Id { get; init; }

    public bool Direction { get; init; }

    public DateTime Period { get; init; }

    public string Country { get; init; } = null!;

    public long ItemType { get; init; }

    public long? UnitType { get; init; }

    public decimal WorthTotal { get; init; }

    public decimal GrossTotal { get; init; }

    public decimal AmountTotal { get; init; }

    public long Region { get; init; }
}

// values (:ItemType, :Period, :Country, :Region, :UnitType, :ImportWorthTotal, :ImportNettoTotal, :ImportAmountTotal, :ExportWorthTotal, :ExportNettoTotal, :ExportAmountTotal)
