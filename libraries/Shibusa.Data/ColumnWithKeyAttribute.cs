namespace Shibusa.Data;

/// <summary>
/// This expands the .NET <see cref="ColumnAttribute"/> class by
/// adding a boolean indicator for whether the column is part of the primary key.
/// <remarks>This is useful for the SQL builder utilities.</remarks>
/// </summary>
public class ColumnWithKeyAttribute : System.ComponentModel.DataAnnotations.Schema.ColumnAttribute
{
    /// <summary>
    /// Create a new instance of the <see cref="ColumnWithKeyAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of the column.</param>
    public ColumnWithKeyAttribute(string name) : base(name)
    {
    }

    /// <summary>
    /// Gets an indicator of whether this column is part of the primary key.
    /// </summary>
    public bool IsPartOfKey { get; set; }
}
