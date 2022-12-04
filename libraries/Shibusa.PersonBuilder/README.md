# Person Builder

This library is for creating simple, random abstractions of humans.

`PersonBuilder` is a fluent-style person generator.

```
    Person person = new PersonBuilder()
        .WithAge(24)
        .WithGender(Gender.Female)
        .WithName(includeMiddleName: true)
        .WithRaces(minimum: 1, maximum: 2)
        .WithEthnicity(PersonBuilder.Constants.Ethnicity.NOT_HISPANIC)
        .Build();
```