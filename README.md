# Shibusa

This is a collection of libraries containing common, non-proprietary logic and utilities for use in many kinds of projects.

## Shibusa.Calendar

### Calendar

This class contains basic calendar manipulation functions, like counting of days and adjusting weekend holidays to either Friday or Monday.

### UnitedStatesCalendar

This class inherits from `Calendar` and contains calculations for U.S. holidays (e.g., Easter, Thanksgiving, etc.).

---

## Shibusa.Data

### SqlFor

This class is a factory for constructing T-SQL `FOR` clauses for temporal table queries.

### SqlOrderBy

This class is a factory for creating T-SQL `ORDER BY` clauses.

### SqlWhere

This class is a fluent-like factory for creating T-SQL `WHERE` clauses.

---

## Shibusa.Maths

### Calculate

This class contains common calculations, such as Age, IsPrime, etc.

### Probability

This class represents the liklihood of an event and contains static functions for independent, mutually exclusive, non-mutually exclusive, and conditional probabilities.

---

## Shibusa.PersonBuilder

This is a collection of fluent-like classes that generate random people for testing purposes.

### Examples

```
[Fact]
public void BuildRandomFemale()
{
    Person person = new PersonBuilder()
        .WithAge(24)
        .WithGender(Gender.Female)
        .WithName(includeMiddleName: true)
        .WithRaces(minimum: 1, maximum: 2)
        .WithEthnicity(PersonBuilder.Constants.Ethnicity.NOT_HISPANIC)
        .Build();

    Assert.Equal<int>(24, person.Age);
    Assert.Equal(Gender.Female, person.Gender);
    Assert.NotNull(person.Name.FirstName);
    Assert.NotNull(person.Name.LastName);
    Assert.NotNull(person.Name.MiddleInitial);
    Assert.NotNull(person.Name.MiddleName);
    Assert.Equal(PersonBuilder.Constants.Ethnicity.NOT_HISPANIC, person.Ethnicity);
    Assert.True(person.Races.Count() >= 1 && person.Races.Count() <= 2);
}
```

---

## Shibusa.Transformations

### Cryptography

This class conttains functions for creating and verifying hashes for strings and files.

### SocialSecurityNumberFormatter

An IFormatProvider implementation for Social Security Numbers.

### TransformDateTime

This class provides static functions for moving a DateTime object from its current value to either the start or end of that same day, preserving the "kind" of date in the process.

These are quite useful for CRUD operations and also ensuring that you're capturing all instances within a range measured by dates (set the start of the range to StartOfDay and the end of the range to EndOfDay to ensure inclusive capture).

See the unit tests for examples of usage.

### TransformRawText

This class provides static functions for raw text manipulation (e.g., mail merge). The `Transform` functions are useful for merging templates, dictionaries, and regular expressions into desired outputs.

Regular expressions are provided for data tags, curly braces, square brackets, and pound signs, but any regular expression can be provided. The `indexOfGroupWithKey` is useful when providing custom regular expressions.

A couple of additional functions are provided for common string manipulation, including `ConvertNewLinesAndTabsToSpaces` and `CondenseSpacingAndTrim`.

The `Transform` function allows for recursive processing, so it's possible to replace tags with text containing other tags; it will process the text until no tags remain or until the `maxRecursiveIterations` has been met. This argument is intended to prevent infinite loops.

This class also contains a couple of common string manipulation functions, such as a function to remove newline characters and one to condense spacing within a string.

#### Example
```
string firstName = "Kitty";
string text = "<html><body><p>Hello #FirstName#!</p></body></html>";
string expected = $"<html><body><p>Hello {firstName}!</p></body></html>";

// The dictionary keys are case insensitive.
var dictionary = new Dictionary<string, string> { { "firstname", firstName } };

var output = TransformRawText.Transform(template: text,
	keyValuePairs: dictionary,
	regularExpression: POUND_EXPRESSION,
	indexOfGroupWithKey: 1);

Assert.Equal(expected, output);

// This line and the Tranform() function above are equivalent.
output = TransformRawText.TransformPounds(text, dictionary);

Assert.Equal(expected, output);
```

See the unit test project for additional examples of usage.

### TransformDollarsByFrequency

This class converts dollar amounts to other dollar amounts based on frequency (e.g., weekly to monthly, annually to hourly, etc.)

See the unit tests for examples of usage; every possible conversion is tested.

### TransformNumbersToWords

This class converts number values to English text.

### UnitedStatesPhoneNumberFormatter

This class is a custom formatter for U.S. phone numbers.

For traditional formatting, use the "F" format (this is the default). For digits only, use "N." To include the U.S. country code, use "I." To use dots instead of dashses, use "dots," and use "IDots" for country code plus dots.

#### Examples
```
string phone;
phone = string.Format(new UnitedStatesPhoneFormatter(), "{0}", "1234567890"); // (123) 456-7890
phone = string.Format(new UnitedStatesPhoneFormatter(), "{0:F}", "1234567890"); // (123) 456-7890
phone = string.Format(new UnitedStatesPhoneFormatter(), "{0:N}", "1234567890"); // 1234567890
phone = string.Format(new UnitedStatesPhoneFormatter(), "{0:dots}", "1234567890"); // 123.456.7890
phone = string.Format(new UnitedStatesPhoneFormatter(), "{0:I}", "1234567890"); // +1 (123) 456-7890
phone = string.Format(new UnitedStatesPhoneFormatter(), "{0:Idots}", "1234567890"); // +1.123.456.7890
```

### SocialSecurityNumberFormatter

This class is a custom formatter for Social Security Numbers.

For traditional formatting, use the "F" format (this is the default). For digits only, use "N," and for dots instead of dashes, use "dots."

#### Examples
```
string ssn;
ssn = string.Format(new SocialSecurityNumberFormatter(), "{0}", "123456789"); // 123-45-6789
ssn = string.Format(new SocialSecurityNumberFormatter(), "{0:F}", "123456789"); // 123-45-6789
ssn = string.Format(new SocialSecurityNumberFormatter(), "{0:N}", "123456789"); // 123456789
ssn = string.Format(new SocialSecurityNumberFormatter(), "{0:dots}", "123456789"); // 123.45.6789
```

---

## Shibusa.Validators

### EmailAddress

This class validates an email address. Email validation is much harder than it seems; no single regular expression could ever accomplish this task.

Some relevant references:

[List of valid and invalid email addresses](http://codefool.tumblr.com/post/15288874550/list-of-valid-and-invalid-email-addresses)

[Wikipedia](https://en.wikipedia.org/wiki/Email_address)

[w3.org](https://www.w3.org/Protocols/rfc822/3_Lexical.html)

[Microsoft](https://blogs.msdn.microsoft.com/testing123/2009/02/06/email-address-test-cases/)

### SocialSecurityNumber

This class validates a Social Security Number. The `IsValidStructure` function validates the structure of the SSN and the `IsValid` function validates that the SSN is valid.

[ssa.gov](https://www.ssa.gov/employer/stateweb.htm)

### UnitedStatesPhoneNumber

The `IsValidStructure` in this class validates a 10 or 11 digit U.S. phone number. 

---

## FindTextCli

This is a console application that searches for regular expression matches within the content of files in a directory; See the `/FindTextCli/README.md.`