using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shibusa.Data;

public static class PostgeSQLSqlBuilder
{
    private readonly static string NL = Environment.NewLine;

    public static string? GetColumnList(Type type, string? prefix = null, params string[] columnNamesToExclude)
    {
        if (type == null) { throw new ArgumentNullException(nameof(type)); }

        if (!string.IsNullOrWhiteSpace(prefix) && !prefix.EndsWith('.')) { prefix = $"{prefix}."; }

        List<string> selectProperties = new();

        object[] attributes = type.GetCustomAttributes(typeof(TableAttribute), true);

        if (attributes?.Any() ?? false)
        {
            System.Reflection.PropertyInfo[] properties = type.GetProperties();

            foreach (System.Reflection.PropertyInfo property in properties)
            {
                object[] ColumnWithKeyAttribute = property.GetCustomAttributes(typeof(ColumnWithKeyAttribute), true);

                if (ColumnWithKeyAttribute?.Any() ?? false)
                {
                    string? columnName = ((ColumnWithKeyAttribute)ColumnWithKeyAttribute.Last()).Name;

                    if (columnName == null) { throw new Exception($"Unable to get column name for {property.Name}"); }

                    if (!columnNamesToExclude.Contains(columnName, StringComparer.OrdinalIgnoreCase))
                    {
                        if (columnName.Equals(property.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            selectProperties.Add($"{prefix}{columnName}");
                        }
                        else
                        {
                            selectProperties.Add($"{prefix}{columnName} AS {property.Name}");
                        }
                    }
                }
            }
        }
        return selectProperties.Any() ? string.Join(", ", selectProperties) : null;
    }

    public static string? CreateSelect(Type type, string? prefix = null, params string[] columnNamesToExclude)
    {
        if (type == null) { throw new ArgumentNullException(nameof(type)); }
        TableAttribute[] attributes = type.GetCustomAttributes(typeof(TableAttribute), true).Cast<TableAttribute>().ToArray();

        var columns = GetColumnList(type, prefix, columnNamesToExclude);

        return !string.IsNullOrWhiteSpace(columns) ? $"SELECT {columns} FROM {GetFullTableName(attributes.Last())} {prefix}".Trim() : null;
    }

    public static string? CreateInsert(Type type)
    {
        if (type == null) { throw new ArgumentNullException(nameof(type)); }

        StringBuilder result = new();

        TableAttribute[] attributes = type.GetCustomAttributes(typeof(TableAttribute), true).Cast<TableAttribute>().ToArray();

        if (attributes?.Any() ?? false)
        {
            List<string> columnNames = new();
            List<string> propertyNames = new();

            System.Reflection.PropertyInfo[] properties = type.GetProperties();

            foreach (System.Reflection.PropertyInfo property in properties)
            {
                object[] ColumnWithKeyAttribute = property.GetCustomAttributes(typeof(ColumnWithKeyAttribute), true);

                if (ColumnWithKeyAttribute?.Any() ?? false)
                {
                    ColumnWithKeyAttribute dbColumn = ((ColumnWithKeyAttribute)ColumnWithKeyAttribute.Last());
                    if (dbColumn.Name != null)
                    {
                        columnNames.Add(dbColumn.Name);
                        propertyNames.Add($"@{property.Name}");
                    }
                }
            }

            result.AppendLine($"INSERT INTO {GetFullTableName(attributes.Last())}{NL}({string.Join(", ", columnNames)}){NL}VALUES{NL}({string.Join(", ", propertyNames)})");
        }

        string final = result.ToString().Trim();
        return final.Length == 0 ? null : final;
    }

    public static string? CreateUpsert(Type type)
    {
        if (type == null) { throw new ArgumentNullException(nameof(type)); }

        StringBuilder result = new();

        TableAttribute[] attributes = type.GetCustomAttributes(typeof(TableAttribute), true).Cast<TableAttribute>().ToArray();

        if (attributes?.Any() ?? false)
        {
            List<string> columnNames = new();
            List<string> propertyNames = new();
            List<string> partsOfKey = new();
            IDictionary<string, string> columnsToUpdate = new Dictionary<string, string>();

            System.Reflection.PropertyInfo[] properties = type.GetProperties();

            foreach (System.Reflection.PropertyInfo property in properties)
            {
                object[] ColumnWithKeyAttribute = property.GetCustomAttributes(typeof(ColumnWithKeyAttribute), true);

                if (ColumnWithKeyAttribute?.Any() ?? false)
                {
                    ColumnWithKeyAttribute dbColumn = (ColumnWithKeyAttribute)ColumnWithKeyAttribute.Last();
                    if (dbColumn.Name != null)
                    {
                        columnNames.Add(dbColumn.Name);
                        propertyNames.Add($"@{property.Name}");
                        if (dbColumn.IsPartOfKey)
                        {
                            partsOfKey.Add(dbColumn.Name);
                        }
                        else
                        {
                            columnsToUpdate.Add(dbColumn.Name, property.Name);
                        }
                    }
                }
            }

            result.AppendLine(
                $"INSERT INTO {GetFullTableName(attributes.Last())}{NL}({string.Join(", ", columnNames)}){NL}VALUES{NL}({string.Join(", ", propertyNames)})");

            if (columnsToUpdate.Any())
            {
                List<string> updateStatements = new();

                foreach (var column in columnsToUpdate)
                {
                    updateStatements.Add($"{column.Key} = @{column.Value}");
                }

                if (updateStatements.Any())
                {
                    result.Append($"ON CONFLICT ({string.Join(", ", partsOfKey)}){NL}DO UPDATE{NL}SET{NL}{string.Join($",{NL}", updateStatements)}");
                }

                result.AppendLine();
            }
            else
            {
                result.Append($"ON CONFLICT ({string.Join(", ", partsOfKey)}){NL}DO NOTHING;");
            }
        }

        string final = result.ToString().Trim();
        return final.Length == 0 ? null : final;
    }

    public static string? CreateDelete(Type type)
    {
        if (type == null) { throw new ArgumentNullException(nameof(type)); }

        object[] attributes = type.GetCustomAttributes(typeof(TableAttribute), true);

        if (attributes?.Any() ?? false)
        {
            return $"DELETE FROM {((TableAttribute)attributes.Last()).Name}";
        }

        return null;
    }

    private static string GetFullTableName(TableAttribute attribute)
    {
        return string.IsNullOrWhiteSpace(attribute.Schema)
           ? attribute.Name
           : $"{attribute.Schema}.{attribute.Name}";
    }
}
