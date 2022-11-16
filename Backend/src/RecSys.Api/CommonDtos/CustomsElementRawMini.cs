using CsvHelper.Configuration.Attributes;

namespace RecSys.Api.CommonDtos;

public class CustomsElementRawMini
{
    [Name("napr")]
    public string? Napr { get; init; }

    [Name("period")]
    [TypeConverter(typeof(DateTimeConverter))]
    public DateTime Period { get; init; }

    [Name("nastranapr")]
    public string? Nastranapr { get; init; }

    [Name("tnved_6")]
    public string? Tnved { get; init; }

    [Name("Stoim")]
    public decimal Stoim { get; init; }

    [Name("netto")]
    public decimal Netto { get; init; }

    [Name("kol")]
    public decimal Kol { get; init; }
}
