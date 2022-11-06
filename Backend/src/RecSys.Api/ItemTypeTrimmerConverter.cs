using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace RecSys.Api;

public class ItemTypeTrimmerConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        => text?[..6];
}
