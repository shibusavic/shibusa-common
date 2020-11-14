# Shibusa Common (C#) Libraries

This is a collection of libraries containing common, non-proprietary logic and utilities for use in many kinds of projects.

## Libraries

| Project                                                      |                        NuGet Package                         |
| :----------------------------------------------------------- | :----------------------------------------------------------: |
| **Shibusa.Calendar** contains calendar calculations for **U.S. holidays** and common manipulations, such as dealing with work days. |  ![Nuget](https://img.shields.io/nuget/v/Shibusa.Calendar)   |
| **Shibusa.Data** contains code for fluent **WHERE clause construction** (very useful with inline SQL construction) and logic for paged query results, including temporal tables. |    ![Nuget](https://img.shields.io/nuget/v/Shibusa.Data)     |
| **Shibusa.Maths** contains some classic math calculations, including a few simple probability constructs. |    ![Nuget](https://img.shields.io/nuget/v/Shibusa.Maths)    |
| **Shibusa.PersonBuilder** is a fluent person factory that **creates random or semi-random "people;"** it comes in handy in testing sometimes. See the unit tests for some good examples. | ![Nuget](https://img.shields.io/nuget/v/Shibusa.PersonBuilder) |
| **Shibusa.Reports** contains functionality for quickly generating reports for the console, CSV, or (in a future version) Excel. Kudos to the team over at [ClosedXML](https://github.com/ClosedXML/ClosedXML) for doing an excellent job. I've only made use of a minority of their libraries, but everything I've asked of it has been intuitive. |   ![Nuget](https://img.shields.io/nuget/v/Shibusa.Reports)   |
| **Shibusa.Transformations** contains a number of useful manipulations, including a **cryptography** class for creating and verifying hashes for strings and files, formatters for U.S. **Social Security Numbers** and **phone numbers,** some common **DateTime** manipulations, and a fairly robust **raw text transformation** tool. There's even a solid **number-to-words** converter. | ![Nuget](https://img.shields.io/nuget/v/Shibusa.Transformations) |

To install from **Package Manager Console**:

```
PM > Install-Package Shibusa.Calendar
```

```
PM > Install-Package Shibusa.Data
```

```
PM > Install-Package Shibusa.Maths
```

```
PM > Install-Package Shibusa.PersonBuilder
```

```
PM > Install-Package Shibusa.Reports
```

```
PM > Install-Package Shibusa.Transformations
```

## Utilities

**Shibusa.FindTextCli** is a console application that can search across specific directories and file extensions for one or more regular expressions. Expressions can be applied as either individual or cumulative.

**Shibusa.NameExtractor** was the utility I used to create the male and female names for *Shibusa.PersonBuilder*.

