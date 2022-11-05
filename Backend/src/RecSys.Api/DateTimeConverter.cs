using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace RecSys.Api;

public class DateTimeConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text is null)
            return null;
        var split = text.Split("/");
        var year = int.Parse(split[1]);
        var month = int.Parse(split[0]);
        return new DateTime(year, month, 1);
    }
}
