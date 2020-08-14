#!/bin/bash
dotnet new sln -n Shibusa.Common
dotnet sln add Shibusa.Calendar/
dotnet sln add Shibusa.Data/
dotnet sln add Shibusa.Maths/
dotnet sln add Shibusa.NameExtractor/
dotnet sln add Shibusa.PersonBuilder/
dotnet sln add Shibusa.Transformations/
dotnet sln add Shibusa.Validators/
dotnet sln add Shibusa.Calendar.UnitTests/
dotnet sln add Shibusa.Data.UnitTests/
dotnet sln add Shibusa.Maths.UnitTests/
dotnet sln add Shibusa.PersonBuilder.UnitTests/
dotnet sln add Shibusa.Transformations.UnitTests/
dotnet sln add Shibusa.Validators.UnitTests/
