using System.ComponentModel.DataAnnotations.Schema;
using Xunit;
using Xunit.Abstractions;

namespace Shibusa.Data.UnitTests;

public class PostgeSQLSqlBuilderTests
{
    private readonly ITestOutputHelper output;

    public PostgeSQLSqlBuilderTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact] 
    public void CreateSelect_Invalid_ReturnsNull()
    {
        Assert.Null(PostgeSQLSqlBuilder.CreateSelect(typeof(InvalidTable)));
    }

    [Fact]
    public void CreateInsert_Invalid_ReturnsNull()
    {
        Assert.Null(PostgeSQLSqlBuilder.CreateInsert(typeof(InvalidTable)));
    }

    [Fact]
    public void CreateUpsert_Invalid_ReturnsNull()
    {
        Assert.Null(PostgeSQLSqlBuilder.CreateUpsert(typeof(InvalidTable)));
    }

    [Fact]
    public void CreateDelete_Invalid_ReturnsNull()
    {
        Assert.Null(PostgeSQLSqlBuilder.CreateDelete(typeof(InvalidTable)));
    }

    [Fact]
    public void GetColumnList_InvalidTable_Empty()
    {
        Assert.Null(PostgeSQLSqlBuilder.GetColumnList(typeof(InvalidTable)));
    }

    [Fact]
    public void GetColumnList_ValidTable_GetList()
    {
        var colList = PostgeSQLSqlBuilder.GetColumnList(typeof(ValidTable));

        Assert.NotNull(colList);
        Assert.Equal("id, name, age", colList);

        output.WriteLine(colList);
    }

    [Fact]
    public void GetColumnList_Prefix_PrefixIncluded()
    {
        var colList = PostgeSQLSqlBuilder.GetColumnList(typeof(ValidTable), "C");

        Assert.NotNull(colList);
        Assert.Equal("C.id, C.name, C.age", colList);

        output.WriteLine(colList);
    }

    [Fact]
    public void GetColumnList_Excluded_ExcludesColumns()
    {
        var colList = PostgeSQLSqlBuilder.GetColumnList(typeof(ValidTable), null, "name", "age");

        Assert.NotNull(colList);
        Assert.Equal("id", colList);

        output.WriteLine(colList);
    }

    [Fact]
    public void CreateSelect_Valid_CreatesSelect()
    {
        string expected = @"SELECT id, name, age FROM public.test_table";

        var selectStatement = PostgeSQLSqlBuilder.CreateSelect(typeof(ValidTable));

        output.WriteLine(selectStatement);

        Assert.Equal(expected, selectStatement);
    }

    [Fact]
    public void CreateInsert_Valid_CreatesInsert()
    {
        string expected = @"INSERT INTO public.test_table
(id, name, age)
VALUES
(@Id, @Name, @Age)";

        var insertStatement = PostgeSQLSqlBuilder.CreateInsert(typeof(ValidTable));

        output.WriteLine(insertStatement);

        Assert.Equal(expected, insertStatement);
    }

    [Fact]
    public void CreateUpsert_Valid_CreatesUpsert()
    {
        string expected = @"INSERT INTO public.test_table
(id, name, age)
VALUES
(@Id, @Name, @Age)
ON CONFLICT (id)
DO UPDATE
SET
name = @Name,
age = @Age";

        var upsertStatement = PostgeSQLSqlBuilder.CreateUpsert(typeof(ValidTable));

        output.WriteLine(upsertStatement);

        Assert.Equal(expected, upsertStatement);
    }
}

[Table("test_table", Schema = "public")]
internal class ValidTable
{
    public ValidTable(Guid id, string name, int age)
    {
        Id = id;
        Name = name;
        Age = age;
    }

    [ColumnWithKey("id", IsPartOfKey = true, Order = 1, TypeName = "uuid")]
    public Guid Id { get; }

    [ColumnWithKey("name", IsPartOfKey = false, Order = 2, TypeName = "text")]
    public string Name { get; }

    [ColumnWithKey("age", IsPartOfKey = false, Order = 3, TypeName = "integer")]
    public int Age { get; }
}

internal class InvalidTable
{
    public InvalidTable(Guid id, string name, int age)
    {
        Id = id;
        Name = name;
        Age = age;
    }

    public Guid Id { get; }

    public string Name { get; }

    public int Age { get; }
}
