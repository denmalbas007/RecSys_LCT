using System.Text;

namespace RecSys.Api.CommonDtos;

public class Filter
{
    public long[] Regions { get; init; } = Array.Empty<long>();

    public string[] ItemTypes { get; init; } = Array.Empty<string>();

    public string[] Countries { get; init; } = Array.Empty<string>();

    public string GetWhereClause()
    {
        var clauses = new List<string>();
        var sb = new StringBuilder();
        if (Regions.Length == 0 && ItemTypes.Length == 0 && Countries.Length == 0)
            return string.Empty;
        if (Regions.Length > 0)
            clauses.Add("region = ANY(:Regions)");
        if (ItemTypes.Length > 0)
            clauses.Add("item_type = ANY(:ItemTypes)");
        if (Countries.Length > 0)
            clauses.Add("country = ANY(:Countries)");
        var query = string.Join(" AND ", clauses);
        sb.Append("WHERE ").Append(query);
        return sb.ToString();
    }
}
