# Reports

This library has tools for generating CSV (or other delimited) reports dynamically.

```
    string[] expectedHeaders = new string[] { "A", "B", "C" };

    ConsoleReport report = new(new ReportConfiguration()
    {
        AddDiscoveredHeaders = true,
        Delimiter = PipeDelimiter,
        HeadersAreCaseSensitive = false
    });

    report.AddLine(new Dictionary<string, string>()
    {
        { expectedHeaders[0], "a"},
        { expectedHeaders[1], "b"},
        { expectedHeaders[2], "c"},
    });

    string headers = report.GetHeaderAsString();
```