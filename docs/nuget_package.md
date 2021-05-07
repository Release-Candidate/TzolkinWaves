# LunaZodiaco NuGet Package

LunaZodiaco is a F# NuGet package to calculate the moon phase and the moon's zodiac (ecliptic longitude) at a given day.
It uses [Conway's method](http://www.faqs.org/faqs/astronomy/faq/part3/section-15.html) to calculate the moon age, which works in the 20th and 21th century and has an accuracy of about 1 day. The zodiac / ecliptic longitude ist calculated using an algorithm of [Paul Schlyter](http://stjarnhimlen.se/comp/tutorial.html#7) - *Computing planetary positions - a tutorial with worked examples*.

So both values aren't accurate enough for use in a serious astronomy program, but suitable for a calender like application - the position changes during one day anyway.

You can try LunaZodiaco interactively online using this Jupyter notebook at [![Binder](https://mybinder.org/badge_logo.svg)](https://mybinder.org/v2/gh/Release-Candidate/LunaZodiaco/main?filepath=LunaZodiaco.ipynb).

## Installation of the NuGet Package

Download an install the LunaZodiaco Nuget package using your IDE (Visual Studio or JetBrains Rider) or one of the command-line possibilities: [LunaZodiaco Package at NuGet](https://www.nuget.org/packages/LunaZodiaco/)

In an interactive F# FSI session, you can use the command #r "nuget: LunaZodiaco" to download and use the NuGet package LunaZodiaco.

```F#
#r "nuget: LunaZodiaco"
```

    Installed package LunaZodiaco version 0.9.2

Everything is located in the namespace `RC.Moon`, so lets open that.

To check if everything is working, we try to display the Unicode codepoint of a full moon:

```F#
open RC.Moon

MoonPhase.FullMoon.ToUnicode ()
```

```ps1con
    🌕️
```

## Module MoonPhase

Types and functions regarding the lunar phase are located in the module `MoonPhase`.

There is a discriminated union for all 8 lunar phases:

```F#
MoonPhase.NewMoon
```

```text
    Luna nueva
```

```F#
MoonPhase.WaxingCrescent
```

```text
    Luna creciente
```

```F#
MoonPhase.FirstQuarter
```

```text
    Cuarto creciente
```

```F#
MoonPhase.WaxingGibbous
```

```text
    Creciente gibosa
```

```F#
MoonPhase.FullMoon
```

```text
    Luna Llena
```

```F#
MoonPhase.WaningGibbous
```

```text
    Menguante gibosa
```

```F#
MoonPhase.LastQuarter
```

```text
    Cuarto menguante
```

```F#
MoonPhase.WaningCrescent
```

```text
    Luna menguante
```

The member `ToString` and the function `MoonPhase.toString` both return the Spanish name of the lunar phase.

```F#
MoonPhase.NewMoon.ToString ()
```

```text
    Luna nueva
```

```F#
MoonPhase.toString MoonPhase.NewMoon
```

```text
    Luna nueva
```

The member `ToUnicode` returns the Unicode codepoint of the lunar phase, a symbol. Same as the function `MoonPhase.toUnicode`.

```F#
MoonPhase.FirstQuarter.ToUnicode ()
```

```ps1con
    🌓️
```

```F#
MoonPhase.toUnicode MoonPhase.FirstQuarter
```

```ps1con
    🌓️
```

The member `ToInt` return an integer index of the lunar phase, where `0` is the new moon, `4` is the full moon and `7` waning crescent.

```F#
MoonPhase.WaxingGibbous.ToInt ()
```

```ps1con
    3
```

The member `FromInt` and function `MoonPhase.fromInt` convert an int to a moon phase option, it is the inverse of the function `ToInt` above.

```F#
MoonPhase.fromInt 3
```

```ps1con
               Value
    Creciente gibosa
```

```F#
MoonPhase.T.FromInt 3
```

```ps1con
               Value
    Creciente gibosa
```

Too big or small integers yield `None`.

```F#
MoonPhase.fromInt -6
```

```ps1con
    None
```

The function `MoonPhase.getInterval` and member `Interval` return the elongation interval of the phase.

The last quarter occures between an elongation angle (between the moon and the sun) of 270 degrees up to, but not including, 315 degrees.

```F#
MoonPhase.LastQuarter.Interval ()
```

```ps1con
    Item1  Item2
      270    315
```

```F#
MoonPhase.getInterval MoonPhase.LastQuarter
```

```ps1con
    Item1  Item2
      270    315
```

The inverse function of `getInterval` is `fromAngle`, the member function is named `FromAngle`. This returns the lunar phase to a given elongation angle. Angles in degrees need the measurement ``.

```F#
MoonPhase.fromAngle 346.
```

```ps1con
             Value
    Luna menguante
```

```F#
MoonPhase.T.FromAngle 346.
```

```ps1con
             Value
    Luna menguante
```

## Module Zodiac

All zodiac related functions the the zodiac type are contained in the module `Zodiac`.

There is a discriminated union for all 12 zodiacs:

```F#
Zodiac.Aries
```

```ps1con
    Aries
```

```F#
Zodiac.Taurus
```

```ps1con
    Tauro
```

```F#
Zodiac.Gemini
```

```ps1con
    Géminis
```

And so on, until

```F#
Zodiac.Aquarius
```

```ps1con
    Acuario
```

```F#
Zodiac.Pisces
```

```ps1con
    Piscis
```

The member `ToString` and the function `Zodiac.toString` both return the Spanish name of the lunar phase.

```F#
Zodiac.Aries.ToString ()
```

```ps1con
    Aries
```

```F#
Zodiac.toString Zodiac.Aries
```

```ps1con
    Aries
```

The member `ToUnicode` returns the Unicode codepoint of the zodiac, a symbol. Same as the function `Zodiac.toUnicode`.

```F#
Zodiac.Gemini.ToUnicode ()
```

```ps1con
    ♊
```

```F#
Zodiac.toUnicode Zodiac.Gemini
```

```ps1con
    ♊
```

The member `ToInt` return an integer index of the zodiac, where `0` is the Aries, ... and `11` Pisces.

```F#
Zodiac.Capricorn.ToInt ()
```

```ps1con
    9
```

The member `FromInt` and function `Zodiac.fromInt` convert an int to a moon phase option, it is the inverse of the function `ToInt` above.

```F#
Zodiac.fromInt 5
```

```ps1con
    Value
    Virgo
```

```F#
Zodiac.T.FromInt 5
```

```ps1con
    Value
    Virgo
```

Too big or small integers yield `None`.

```F#
Zodiac.fromInt -6
```

```ps1con
    None
```

The function `Zodiac.getInterval` and member `Interval` return the ecliptic longitude interval of the phase.

Libra occurs in the ecliptic longitude angle of 180 to 210 degrees, including 180 but excluding 210.

```F#
Zodiac.Libra.Interval ()
```

```ps1con
    Item1  Item2
      180    210
```

```F#
Zodiac.getInterval Zodiac.Libra
```

```ps1con
    Item1  Item2
      180    210
```

The inverse function of `getInterval` is `fromAngle`, the member function is named `FromAngle`. This returns the zodiac to a given ecliptic longitude angle. Angles in degrees need the measurement ``.

```F#
Zodiac.fromAngle 346.
```

```ps1con
     Value
    Piscis
```

```F#
Zodiac.T.FromAngle 346.
```

```ps1con
     Value
    Piscis
```

## Module LunaZodiaco

This module contains the moon phase and zodiac calculations and the type `MoonDay`, that holds the zodiac and lunar phase for a given date.

```F#
let moonDay = { LunaZodiaco.MoonDay.Phase = MoonPhase.WaningGibbous; LunaZodiaco.Zodiac = Zodiac.Capricorn }
moonDay
```

```ps1con
        Phase                Zodiac
    Menguante gibosa    Capricornio
```

`MoonDay` does have the member `ToStrings` and function `LunaZodiaco.toStrings`, that return a tuple of strings

```F#
LunaZodiaco.toStrings moonDay
```

```ps1con
            Item1             Item2
    Menguante gibosa    Capricornio
```

```F#
moonDay.ToStrings ()
```

```ps1con
            Item1             Item2
    Menguante gibosa    Capricornio
```

And the member `ToString`, that returns a single string.

```F#
moonDay.ToString ()
```

```ps1con
    { Phase = WaningGibbous
      Zodiac = Capricorn }
```

The function `LunaZodiaco.toUnicode` returns the moon phase and zodiac as a tuple of Unicode code points / symbols:

```F#
LunaZodiaco.toUnicode moonDay
```

```ps1con
       Item1  Item2
    🌖    ♑
```

```F#
moonDay.ToUnicode ()
```

```ps1con
    Item1  Item2
       🌖    ♑
```

The member `ToIntervals` and the function `LunaZodiaco.toIntervals` return the elongation angle and the ecliptic longitude angle interval of the moon phase and zodiac as a tuple.

```F#
LunaZodiaco.toIntervals moonDay
```

```ps1con
        Item1           Item2
    ( 225, 270 )    ( 270, 300 )
```

```F#
moonDay.ToIntervals ()
```

```ps1con
        Item1           Item2
    ( 225, 270 )    ( 270, 300 )
```

## Calculation of Moon Age and Ecliptic Longitude

The function `LunaZodiaco.computeMoonPhase` returns the moon phase interval of the moon at a given date, with an accuracy of +/- one day. Attention: it only works in the 20th and 21st century, as it uses Conways algorithm to calculate the moon age (the day of the moon in one lunar cycle of 29 days).

```F#
open System
LunaZodiaco.computeMoonPhase (DateTime (2021, 05, 06))
```

```ps1con
    Item1  Item2
      315    360
```

THe function `LunaZodiaco.computeLongitude` calculates the ecliptic longitude of the moon at the given day (well, an angle that it has during the day).

```F#
LunaZodiaco.computeLongitude (DateTime (2021, 05, 06))
```

```ps1con
    340.9314455776453
```

To get the moon phase and moon zodiac of a day, use the function `LunaZodiaco.getMoonDay`, that returns the moon phase and the zodiac as a `MoonDay` record.

```F#
LunaZodiaco.getMoonDay (DateTime (2021, 05, 06))
```

```ps1con
         Phase        Zodiac
    Luna menguante    Piscis
```

&copy; 2021 Roland Csaszar, licensed under the MIT license.
