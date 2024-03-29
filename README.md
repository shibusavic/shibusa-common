# Shibusa Common (C#) Libraries

This is a collection of libraries containing common, non-proprietary logic and generic utilities for use in many kinds of projects.

## Libraries

### Target Frameworks

The libraries currently target `netstandard2.0`, `net6.0`, and `net7.0`, though not all features are available for `netstandard2.0`.

### Packages

| Project                                                      |                        NuGet Package                         |
| :----------------------------------------------------------- | :----------------------------------------------------------: |
| **Shibusa.Calendar** contains calendar calculations for **U.S. holidays** and common manipulations, such as dealing with work days. |  ![Nuget](https://img.shields.io/nuget/v/Shibusa.Calendar)   |
| **Shibusa.Data** contains code for fluent **WHERE clause construction** (very useful with inline SQL construction) and logic for paged query results, including temporal tables. |    ![Nuget](https://img.shields.io/nuget/v/Shibusa.Data)     |
| **Shibusa.Extensions** contains common extensions for DateTime, Enum, and Streams. |  ![Nuget](https://img.shields.io/nuget/v/Shibusa.Extensions)   |
| **Shibusa.Maths** contains some classic math calculations, including a few simple probability constructs. |    ![Nuget](https://img.shields.io/nuget/v/Shibusa.Maths)    |
| **Shibusa.PersonBuilder** is a fluent person factory that **creates random or semi-random "people;"** it comes in handy in testing sometimes. See the unit tests for examples. | ![Nuget](https://img.shields.io/nuget/v/Shibusa.PersonBuilder) |
| **Shibusa.Reports** contains functionality for quickly generating reports for the console or CSV. |   ![Nuget](https://img.shields.io/nuget/v/Shibusa.Reports)   |
| **Shibusa.Transformations** contains a number of useful manipulations, including a **cryptography** class for creating and verifying hashes for strings and files, formatters for U.S. **Social Security Numbers** and **phone numbers,** some common **DateTime** manipulations, and a fairly robust **raw text transformation** tool. There's even a **number-to-words** converter. | ![Nuget](https://img.shields.io/nuget/v/Shibusa.Transformations) |
| **Shibusa.Validators** contains validators for email addresses, phone numbers, and Social Security Numbers.|   ![Nuget](https://img.shields.io/nuget/v/Shibusa.Validators)   |


To install from **Package Manager Console**:

| Project    |  Install Command   |
| ---------- | ------------------ |
| **Shibusa.Calendar** | PM > Install-Package Shibusa.Calendar |
| **Shibusa.Data** | PM > Install-Package Shibusa.Data |
| **Shibusa.Extensions** | PM > Install-Package Shibusa.Extensions |
| **Shibusa.Maths** | PM > Install-Package Shibusa.Maths |
| **Shibusa.PersonBuilder** | PM > Install-Package Shibusa.PersonBuilder |
| **Shibusa.Reports** | PM > Install-Package Shibusa.Reports |
| **Shibusa.Transformations** | PM > Install-Package Shibusa.Transformations |
| **Shibusa.Validators** | PM > Install-Package Shibusa.Validators |
