// SPDX-License-Identifier: MIT
// Copyright (C) 2021 Roland Csaszar
//
// Project:  TzolkinWaves
// File:     TzolkinWidgets.fs
// Date:     5/11/2021 12:21:02 PM
//==============================================================================

/// The project is contained in this namespace.
namespace TzolkinWaves

/// Contains widgets to display data and images of Tzolk’in dates.
[<AutoOpen>]
module TzolkinWidgets=
    open Avalonia.Layout
    open Avalonia.Media
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL

    open RC.Maya

    /// Display a Tzolk’in date, the day number and day glyph.
    ///
    /// Params:
    ///         `model` The MVU model
    ///         `dispatch` The message dispatch function.
    ///
    /// Returns:
    ///         A `Border` containing the Tzolk’in date display.
    let tzolkinDateDisplay model dispatch =
        let tzolkin = TzolkinDate.fromDate model.Date
        Border.create [
            Border.borderThickness 1.
            Border.padding (10., 0., 10., 0.)
            Border.horizontalAlignment HorizontalAlignment.Center
            Border.verticalAlignment VerticalAlignment.Top
            Border.child (
                Grid.create [
                    Grid.columnDefinitions "80., 150."
                    Grid.rowDefinitions (sprintf "%f, 20." tzolkinImageHeight)
                    Grid.children [
                        Border.create [
                            Border.row 0
                            Border.column 0
                            Border.classes [ "TzolkinImage" ]
                            Border.background (getPNGBrushNumber tzolkin.Number)
                        ]
                        Border.create [
                            Border.row 0
                            Border.column 1
                            Border.classes [ "TzolkinImage" ]
                            Border.background (getPNGBrushGlyph tzolkin.Glyph)
                        ]
                        TextBlock.create [
                            TextBlock.row 1
                            TextBlock.column 0
                            TextBlock.textAlignment TextAlignment.Right
                            TextBlock.foreground foregroundLight
                            TextBlock.fontSize tzolkinFontSize
                            TextBlock.fontStyle tzolkinFontStyle
                            TextBlock.fontWeight tzolkinFontWeight
                            TextBlock.classes [ "TzolkinText" ]
                            TextBlock.text (sprintf "%s" <| tzolkin.Number.ToString ())
                        ]
                        TextBlock.create [
                            TextBlock.row 1
                            TextBlock.column 1
                            TextBlock.textAlignment TextAlignment.Left
                            TextBlock.foreground foregroundLight
                            TextBlock.fontSize tzolkinFontSize
                            TextBlock.fontStyle tzolkinFontStyle
                            TextBlock.fontWeight tzolkinFontWeight
                            TextBlock.classes [ "TzolkinText" ]
                            TextBlock.text (sprintf " %s" <| tzolkin.Glyph.ToString ())
                        ]
                    ]
                ]
            )
        ]

    /// Display a title line of a Tzolk’in glyph description.
    ///
    /// Params:
    ///         `title` The Glyph description title to display.
    ///
    /// Returns:
    ///         The `TextBlock` containing the description.
    let tzolkinDescTitle title =
        TextBlock.create [
            TextBlock.textAlignment TextAlignment.Left
            TextBlock.foreground foregroundLight
            TextBlock.fontSize tzolkinFontSize
            TextBlock.fontStyle tzolkinFontStyle
            TextBlock.fontWeight tzolkinFontWeight
            TextBlock.classes [ "TzolkinDescTitle" ]
            TextBlock.text (sprintf "%s: " title)
        ]

    /// Display a line of a Tzolk’in glyph description.
    ///
    /// Params:
    ///         `info` The Glyph description to display.
    ///
    /// Returns:
    ///         The `TextBlock` containing the description.
    let tzolkinDescValue info =
        TextBlock.create [
            TextBlock.textAlignment TextAlignment.Center
            TextBlock.foreground accentDarkRed
            TextBlock.fontSize tzolkinFontSize
            TextBlock.fontStyle tzolkinFontStyle
            TextBlock.fontWeight tzolkinFontWeight
            TextBlock.classes [ "TzolkinDescValue" ]
            TextBlock.text (sprintf "%s" info)
        ]

    /// Display a Tzolk’in glyph description, the data of a Tzolk’in glyph.
    ///
    /// Params:
    ///         `model` The MVU model
    ///         `dispatch` The message dispatch function.
    ///
    /// Returns:
    ///         A `Border` containing the Tzolk’in glyph description.
    let tzolkinDescDisplay model dispatch =
        let tzolkinDesc =
            TzolkinGlyph.fromDate model.Date
            |> TzolkinGlyph.getDescription
        Border.create [
                    Border.borderThickness 1.
                    Border.padding (10., 10., 10., 10.)
                    Border.width 500.
                    Border.horizontalAlignment HorizontalAlignment.Left
                    Border.verticalAlignment VerticalAlignment.Top
                    Border.child (
                        StackPanel.create [
                            StackPanel.dock Dock.Top
                            StackPanel.orientation Orientation.Vertical
                            StackPanel.margin 5.0
                            StackPanel.spacing 5.0
                            StackPanel.children [
                                tzolkinDescTitle "Significado"
                                tzolkinDescValue tzolkinDesc.Meaning
                                tzolkinDescTitle "Elementos o animal asociados"
                                tzolkinDescValue tzolkinDesc.ElementOrAnimal
                                tzolkinDescTitle "Rumbo asociado"
                                tzolkinDescValue tzolkinDesc.Direction
                                tzolkinDescTitle "Color asociado"
                                tzolkinDescValue tzolkinDesc.Color
                                tzolkinDescTitle "Dioses patronos"
                                tzolkinDescValue tzolkinDesc.God
                            ]
                        ]

                    )
        ]
