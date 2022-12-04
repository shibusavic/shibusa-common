# Extensions

The `StreamExtensions` class provides extensions to faciliate writing strings (and *lines*) to streams.

The `DateTimeExtensions` class provides a some `DateTime` manipulations:

- `StartOfDay()` - moves a date to the zero hour.
- `EndOfDay()` - moves a date to the last millisecond of the day.
- `AddWeekdays()' - moves a date forward the specified number of weekdays.

The `EnumExtensions` class provides utility for adding the `[Description]` attribute to enums.
This is useful when you want to map an enum to a string value with spaces or special characters.

```
ThingILike me = ThingILike.All;

Console.WriteLine(me.GetDescription()); // 'All of it'

me = ThingILike.Nothing;

Console.WriteLine(me.GetDescription()); // 'Nothing' (when no `Description` is present, defaults to string value)

private enum ThingILike
{
    Nothing = 0,
    [Description("Long walks")]
    LongWalks,
    [Description("All of it")]
    All
}

```