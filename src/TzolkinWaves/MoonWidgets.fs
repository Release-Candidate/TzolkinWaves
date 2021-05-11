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

    open Avalonia.Layout
    open Avalonia.Media
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL

    open RC.Moon

    let moonSymbol row column string =
        TextBlock.create [
            TextBlock.row row
            TextBlock.column column
            TextBlock.textAlignment TextAlignment.Center
            TextBlock.fontSize moonSymbolSize
            TextBlock.classes [ "MoonSymbol" ]
            TextBlock.text (sprintf "%s" string)
        ]

    let moonPhaseSymbol phase =
        MoonPhase.toUnicode phase
        |> moonSymbol 0 0

    let moonZodiacSymbol zodiac =
        Zodiac.toUnicode zodiac
        |> moonSymbol 0  1

    let moonString row column string =
        TextBlock.create [
            TextBlock.row row
            TextBlock.column column
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
        |> moonString 1 1

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
            Border.borderThickness 1.
            Border.padding (10., 10., 10., 10.)
            Border.horizontalAlignment HorizontalAlignment.Left
            Border.verticalAlignment VerticalAlignment.Top
            Border.child (
                Grid.create [
                    StackPanel.dock Dock.Left
                    Grid.columnDefinitions (sprintf "%f, %f" moonSymbolHeight moonSymbolHeight)
                    Grid.rowDefinitions (sprintf "%f, 20." moonSymbolHeight)
                    Grid.children [
                        moonPhaseSymbol phase
                        moonPhaseString phase
                        moonZodiacSymbol zodiac
                        moonZodiacString zodiac
                    ]
                ]
            )
        ]
