// SPDX-License-Identifier: MIT
// Copyright (C) 2021 Roland Csaszar
//
// Project:  TzolkinWaves
// File:     NineWaveWidgets.fs
// Date:     5/11/2021 2:36:52 PM
//==============================================================================

/// The project is contained in this namespace.
namespace TzolkinWaves

open System
open Avalonia
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.Layout
open Avalonia.Media
open Avalonia.Controls
open Avalonia.Controls
open Avalonia.Controls.Shapes

open RC.Maya

/// All widgets to display information about Calleman's nine Maya waves.
[<AutoOpen>]
module NineWaveWidgets=

    /// Convert days to years.
    ///
    /// Params:
    ///         `days` The number of days to convert to years
    ///
    /// Returns:
    ///             The days as years, as a string.
    let daysToYears (days: int64) =
        match days with
        | i when i < 365L -> sprintf "%dd" i
        | _ ->  sprintf "%da" (int64 <| float days / 365.25)

    /// Display a title line of a nine wave description.
    ///
    /// Params:
    ///         `title` The nine waves description title to display.
    ///
    /// Returns:
    ///         The `TextBlock` containing the description.
    let nineWaveDescTitle (waveDay: NineWaves.WaveDay) wave =
        TextBlock.create [
            TextBlock.textAlignment TextAlignment.Left
            TextBlock.foreground foregroundLight
            TextBlock.fontSize wavesFontSize
            TextBlock.fontStyle wavesFontStyle
            TextBlock.fontWeight wavesFontWeight
            TextBlock.classes [ "NineWaveTitle" ]
            TextBlock.text (sprintf "onda %i (%i.): " wave waveDay.WaveNumber)
        ]

    /// Display a line of a nine waves description.
    ///
    /// Params:
    ///         `info` The nine wave description to display.
    ///
    /// Returns:
    ///         The `TextBlock` containing the description.
    let nineWaveDescValue (waveDay: NineWaves.WaveDay) =
        TextBlock.create [
            TextBlock.textAlignment TextAlignment.Center
            TextBlock.foreground accentDarkRed
            TextBlock.fontSize wavesFontSize
            TextBlock.fontStyle wavesFontStyle
            TextBlock.fontWeight wavesFontWeight
            TextBlock.classes [ "NineWaveDesc" ]
            TextBlock.text (sprintf
                                "%s, %s / %s\n"
                                (if waveDay.IsNight then "noche" else "día")
                                (daysToYears waveDay.DayNumber)
                                (daysToYears waveDay.OfDays)
                                )
        ]

    /// Display info about the status of the nine Maya waves of the current date.
    ///
    /// Params:
    ///         `model` The MVU model
    ///         `dispatch` The message dispatch function.
    ///
    /// Returns:
    ///         A `Border` containing the Tzolk’in glyph description.
    let nineWaveInfo model dispatch =
        let waveDay9 = NineWaves.getWaveday9 model.Date
        let waveDay8 = NineWaves.getWaveday8 model.Date
        let waveDay7 = NineWaves.getWaveday7 model.Date
        let waveDay6 = NineWaves.getWaveday6 model.Date
        let waveDay5 = NineWaves.getWaveday5 model.Date
        let waveDay4 = NineWaves.getWaveday4 model.Date
        let waveDay3 = NineWaves.getWaveday3 model.Date
        let waveDay2 = NineWaves.getWaveday2 model.Date
        let waveDay1 = NineWaves.getWaveday1 model.Date
        Border.create [
            Border.borderThickness 2.
            Border.borderBrush backgroundBrownDark
            Border.padding (10., 10., 10., 10.)
            Border.horizontalAlignment HorizontalAlignment.Center
            Border.verticalAlignment VerticalAlignment.Top
            Border.child (
                StackPanel.create [
                    StackPanel.dock Dock.Top
                    StackPanel.orientation Orientation.Vertical
                    StackPanel.margin 5.0
                    StackPanel.spacing 5.0
                    StackPanel.children [
                        nineWaveDescTitle waveDay9 9
                        nineWaveDescValue waveDay9
                        nineWaveDescTitle waveDay8 8
                        nineWaveDescValue waveDay8
                        nineWaveDescTitle waveDay7 7
                        nineWaveDescValue waveDay7
                        nineWaveDescTitle waveDay6 6
                        nineWaveDescValue waveDay6
                        nineWaveDescTitle waveDay5 5
                        nineWaveDescValue waveDay5
                        nineWaveDescTitle waveDay4 4
                        nineWaveDescValue waveDay4
                        nineWaveDescTitle waveDay3 3
                        nineWaveDescValue waveDay3
                        nineWaveDescTitle waveDay2 2
                        nineWaveDescValue waveDay2
                        nineWaveDescTitle waveDay1 1
                        nineWaveDescValue waveDay1
                    ]
                ]

            )
        ]

    /// <summary>Draw text with a brown (transparent ;) background, try to keep
    ///  the whole text on screen.</summary>
    /// <param name="args">`SKPaintSurfaceEventArgs` used for the available
    /// width and height.</param>
    /// <param name="x">X coordinate to draw the text to (lower left corner).</param>
    /// <param name="y">Y coordinate to draw the text to (lower left corner).</param>
    /// <param name="text">The text to draw.</param>
    /// <param name="canvas">`SKCanvas` to use to paint.</param>
    /// <param name="painter">`SKPaint`to use to paint the text with.</param>
    /// <param name="painterBackground">`SKPaint`to use to paint the text's
    /// background with.</param>
    let drawText
        unitWidth
        unitHeight
        (getWaveDay : DateTime -> NineWaves.WaveDay)
        (waveFunc : float -> float)
        model
        starty
        =
        let maxW = graphCanvasWidth
        let maxH = graphCanvasHeight
        let day = getWaveDay model.Date
        let isNight = day.IsNight
        let numDays = day.OfDays
        let currDay =
            day.DayNumber
            |> (fun d -> if isNight then d + numDays else d)

        let canvasX idx =
            unitWidth * (idx + 20.)

        let getSpecialDate idx =
            idx * float numDays - float currDay

        let getSpecialPoints idx =
            18. * (idx - float currDay / float numDays)

        let getWaveY canvasX =
            float canvasX
            |> ( * ) (float numDays / 18.)
            |> ( + ) (float currDay)
            |> waveFunc
            |> float
            |> ( + ) starty
            |> ( * ) unitHeight

        let getTextPoint (x,idx) =
            let date = model.Date + TimeSpan.FromDays (getSpecialDate idx)
            ((canvasX x), (getWaveY x), (sprintf "%s" (date.ToShortDateString ())))

        let dayQuotient = Math.Floor (float currDay / float numDays)

        [ for idx in dayQuotient - 1.5 .. 0.5 .. dayQuotient + 1.5 -> getSpecialPoints idx, idx ]
        |> List.filter (fun (e, _) -> -20. <= e && e <= 20.)
        |> List.map getTextPoint



    let getSpecialPoints
        (getWaveDay : DateTime -> NineWaves.WaveDay)
        model
        =
        let day = getWaveDay model.Date
        let isNight = day.IsNight
        let numDays = day.OfDays
        let currDay =
            day.DayNumber
            |> (fun d -> if isNight then d + numDays else d)

        let getSpecialPoints idx =
                     18. * (idx - float currDay / float numDays)

        let dayQuotient = Math.Floor (float currDay / float numDays)

        [ for idx in dayQuotient - 1.5 .. 0.5 .. dayQuotient + 1.5 -> getSpecialPoints idx, idx ]
        |> List.filter (fun (e, _) -> -20. <= e && e <= 20.)

    let drawWaveDot
        unitWidth
        unitHeight
        (getWaveDay : DateTime -> NineWaves.WaveDay)
        (waveFunc : float -> float)
        model
        starty
        (color: string)
        (background: string)
        x
        =
        let day = getWaveDay model.Date
        let isNight = day.IsNight
        let numDays = day.OfDays
        let currDay =
            day.DayNumber
            |> (fun d -> if isNight then d + numDays else d)

        let getWaveY canvasX =
            float canvasX
            |> ( * ) (float numDays / 18.)
            |> ( + ) (float currDay)
            |> waveFunc
            |> float
            |> ( + ) starty
            |> ( * ) unitHeight

        Ellipse.create [
            Ellipse.width (2. * dotCirceRadius)
            Ellipse.height (2. * dotCirceRadius)
            Ellipse.left ((x + 20.) * unitWidth - dotCirceRadius)
            Ellipse.top ((getWaveY x) - dotCirceRadius)
            Ellipse.stroke color
            Ellipse.fill background
            Ellipse.strokeThickness dotCircleThickness
        ]


    /// Draw the graph of a wave, including dots at the changes of night and day
    /// and the highest and lowest points of the days and nights.
    /// <param name="unitWidth">Factor to scale all x values with.</param>
    /// <param name="unitHeight">Factor to scale all y values with.</param>
    /// <param name="getWaveDay">The `WaveDay` function to use to get data about
    /// the wave</param>
    /// <param name="waveFunc">The sine function of the wave to use.</param>
    /// <param name="model">The MVU model.</param>
    /// <param name="starty">The y offset of the x axis to use.</param>
    let drawWave
        unitWidth
        unitHeight
        (getWaveDay : DateTime -> NineWaves.WaveDay)
        (waveFunc : float -> float)
        model
        starty
        =
        let day = getWaveDay model.Date
        let isNight = day.IsNight
        let numDays = day.OfDays
        let currDay =
            day.DayNumber
            |> (fun d -> if isNight then d + numDays else d)

        let canvasX idx =
            unitWidth * (idx + 20.)

        let getWaveY canvasX =
            float canvasX
            |> ( * ) (float numDays / 18.)
            |> ( + ) (float currDay)
            |> waveFunc
            |> float
            |> ( + ) starty
            |> ( * ) unitHeight

        Polyline.create [
            Polyline.points [ for idx in -20. .. 20. ->  Point (canvasX idx, getWaveY idx) ]
            Polyline.stroke accentDarkRed
            Polyline.strokeThickness graphThickness
        ]

    let drawXAxis unitWidth unitHeight y =
        Line.create [
            Line.startPoint (0., y * unitHeight)
            Line.endPoint (40. * unitWidth, y * unitHeight)
            Line.stroke backgroundBrownDark
            Line.strokeThickness axisThickness
        ]

    let nineWavesGraph model dispatch =
        Canvas.create [
            Canvas.background backgroundBrown
            Canvas.width graphCanvasWidth
            Canvas.height graphCanvasHeight
            Canvas.classes [ "WavesGraph" ]
            Canvas.children [
                let unitHeight = graphCanvasHeight / 9.
                let unitWidth = graphCanvasWidth / 40.

                drawXAxis unitWidth unitHeight 1.5
                drawXAxis unitWidth unitHeight 4.5
                drawXAxis unitWidth unitHeight 7.5

                Line.create [
                    Line.startPoint (20. * unitWidth, 0.)
                    Line.endPoint (20. * unitWidth, 9. * unitHeight)
                    Line.stroke backgroundBrownDark
                    Line.strokeThickness axisThickness
                ]

                drawWave unitWidth unitHeight NineWaves.getWaveday9 NineWaves.wavefunc9 model 1.5
                drawWave unitWidth unitHeight NineWaves.getWaveday8 NineWaves.wavefunc8 model 4.5
                drawWave unitWidth unitHeight NineWaves.getWaveday7 NineWaves.wavefunc7 model 7.5

                for (x, _) in getSpecialPoints NineWaves.getWaveday9 model do
                    drawWaveDot unitWidth unitHeight NineWaves.getWaveday9 NineWaves.wavefunc9 model 1.5 accentDarkRed backgroundBrown x

                for (x, _) in getSpecialPoints NineWaves.getWaveday8 model do
                    drawWaveDot unitWidth unitHeight NineWaves.getWaveday8 NineWaves.wavefunc8 model 4.5 accentDarkRed backgroundBrown x

                for (x, _) in getSpecialPoints NineWaves.getWaveday7 model do
                    drawWaveDot unitWidth unitHeight NineWaves.getWaveday7 NineWaves.wavefunc7 model 7.5 accentDarkRed backgroundBrown x

                drawWaveDot unitWidth unitHeight NineWaves.getWaveday9 NineWaves.wavefunc9 model 1.5 foregroundLight backgroundBrown 0.
                drawWaveDot unitWidth unitHeight NineWaves.getWaveday8 NineWaves.wavefunc8 model 4.5 foregroundLight backgroundBrown 0.
                drawWaveDot unitWidth unitHeight NineWaves.getWaveday7 NineWaves.wavefunc7 model 7.5 foregroundLight backgroundBrown 0.

                for (x, y, text) in drawText unitWidth unitHeight NineWaves.getWaveday9 NineWaves.wavefunc9 model 1.5 do
                    TextBlock.create [
                        TextBlock.left x
                        TextBlock.top y
                        TextBlock.background backgroundBrown
                        TextBlock.text text
                        TextBlock.fontSize graphDateTextSize
                    ]

                for (x, y, text) in drawText unitWidth unitHeight NineWaves.getWaveday8 NineWaves.wavefunc8 model 4.5 do
                    TextBlock.create [
                        TextBlock.left x
                        TextBlock.top y
                        TextBlock.background backgroundBrown
                        TextBlock.text text
                        TextBlock.fontSize graphDateTextSize
                    ]

                for (x, y, text) in drawText unitWidth unitHeight NineWaves.getWaveday7 NineWaves.wavefunc7 model 7.5 do
                    TextBlock.create [
                        TextBlock.left x
                        TextBlock.top y
                        TextBlock.background backgroundBrown
                        TextBlock.text text
                        TextBlock.fontSize graphDateTextSize
                    ]
            ]
        ]
