using CsvHelper.Configuration.Attributes;

namespace RecSys.Api.CommonDtos;

public class CustomsElementRaw
{
    [Ignore]
    public long? Id { get; init; }

    [Index(0)]
    public string? Napr { get; init; }

    [Index(1)]
    [TypeConverter(typeof(DateTimeConverter))]
    public DateTime? Period { get; init; }

    [Index(2)]
    public string? Nastranapr { get; init; }

    [Index(3)]
    [TypeConverter(typeof(ItemTypeTrimmerConverter))]
    public string? Tnved { get; init; }

    [Index(4)]
    public int? Edizm { get; init; }

    [Index(5)]
    public decimal? Stoim { get; init; }

    [Index(6)]
    public decimal? Netto { get; init; }

    [Index(7)]
    public decimal? Kol { get; init; }

    [Index(8)]
    public string? Region { get; init; }

    [Index(9)]
    public string? RegionsS { get; init; }

    [Ignore]
    public bool? IsProcessed { get; init; }
}
