# Calendar Utilities

This library contains a static `Calendar` class that performs some simple functions:

- Collecting and/or counting *days* inclusively between two `DateOnly` (`DateTime` in *netstandard2.0*) instances.
- Collecting and/or counting *weekdays* inclusively between two `DateOnly` (`DateTime` in *netstandard2.0*) instances.
- Adjusting a holiday's celebrated date (Saturday falls to Friday and Sunday jumps to Monday).

The `UnitedStatesCalendar` is a U.S.-specific holiday generation utility.
Given a year or two dates, a collection of holidays will be delivered.