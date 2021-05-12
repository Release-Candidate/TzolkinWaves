// SPDX-License-Identifier: MIT
// Copyright (C) 2021 Roland Csaszar
//
// Project:  TzolkinWaves
// File:     MoonWidgets.fs
// Date:     5/11/2021 2:00:31 PM
//==============================================================================

/// The project is contained in this namespace.
namespace TzolkinWaves

/// Module holding all moon phase and zodiac related widgets.
[<AutoOpen>]
module MoonWidgets=

    open System.Runtime.InteropServices
    open Avalonia.Layout
    open Avalonia.Media
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL

    open RC.Moon

    let moonSymbol row column string =
        TextBlock.create [
            TextBlock.row row
            TextBlock.column column
            TextBlock.textAlignment (if RuntimeInformation.IsOSPlatform OSPlatform.OSX then
                                        TextAlignment.Left
                                     else
                                        TextAlignment.Center)
            TextBlock.fontSize moonSymbolSize
            TextBlock.classes [ "MoonSymbol" ]
            TextBlock.text (sprintf "%s" string)
        ]

    let moonPhaseSymbol phase =
        MoonPhase.toUnicode phase
        |> moonSymbol 0 0

    let moonZodiacSymbol zodiac =
        Zodiac.toUnicode zodiac
        |> moonSymbol 2  0

    let moonString row column string =
        TextBlock.create [
            TextBlock.row row
            TextBlock.column column
            TextBlock.horizontalAlignment HorizontalAlignment.Stretch
            TextBlock.textAlignment TextAlignment.Center
            TextBlock.foreground foregroundLight
            TextBlock.fontSize moonFontSize
            TextBlock.fontStyle moonFontStyle
            TextBlock.fontWeight moonFontWeight
            TextBlock.classes [ "MoonText" ]
            TextBlock.text (sprintf "%s" string)
        ]

    let moonPhaseString phase =
        MoonPhase.toString phase
        |> moonString 1 0

    let moonZodiacString zodiac =
        Zodiac.toString zodiac
        |> moonString 3 0

    /// Display the moon phase and moon zodiac of the current date.
    ///
    /// Params:
    ///         `model` The MVU model instance.
    ///         `dispatch` The message dispatch function.
    ///
    /// Returns:
    ///         A `Border` containing the moon phase and zodiac widget.
    let moonView model dispatch =
        let { LunaZodiaco.MoonDay.Zodiac = zodiac
              LunaZodiaco.MoonDay.Phase = phase } = LunaZodiaco.getMoonDay model.Date
        Border.create [
            Border.borderThickness 2.
            Border.borderBrush backgroundBrownDark
            Border.padding (10., 10., 10., 0.)
            Border.width 200.
            Border.horizontalAlignment HorizontalAlignment.Center
            Border.verticalAlignment VerticalAlignment.Top
            Border.child (
                Grid.create [
                    Grid.horizontalAlignment HorizontalAlignment.Center
                    Grid.columnDefinitions ("*")
                    Grid.rowDefinitions (sprintf "%f, 20., %f, 20." moonSymbolHeight moonSymbolHeight)
                    Grid.children [
                        moonPhaseSymbol phase
                        moonPhaseString phase
                        moonZodiacSymbol zodiac
                        moonZodiacString zodiac
                    ]
                ]
            )
        ]
