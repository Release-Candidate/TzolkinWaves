// SPDX-License-Identifier: MIT
// Copyright (C) 2021 Roland Csaszar
//
// Project:  TzolkinWaves
// File:     View.fs
// Date:     07.May.2021
//
//=============================================================================

namespace TzolkinWaves


[<AutoOpen>]
module View =

    open System
    open Elmish
    open Avalonia
    open Avalonia.Layout
    open Avalonia.Controls
    open Avalonia.Input
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Components.Hosts
    open Avalonia.FuncUI.Elmish

    /// Display a calendar to pick the date.
    ///
    /// Params:
    ///         `model` The MVU model.
    ///         `dispatch` The message dispatch function.
    ///
    /// Returns:
    ///         A calendar widget.
    let calendarView model dispatch =
        Calendar.create [
            Calendar.verticalAlignment VerticalAlignment.Top
            Calendar.horizontalAlignment HorizontalAlignment.Center
            Calendar.displayDate model.Date
            Calendar.background backgroundBrownLight
            Calendar.onSelectedDateChanged (fun args ->
                match args.HasValue with
                | false -> ()
                | true -> dispatch <| ChangeDate (args.Value.Date))
        ]

    /// The view function of MVU.
    ///
    /// Params:
    ///         `model` The MVU model instance holding the state.
    ///         `dispatch` The message dispatch function.
    ///
    /// Returns:
    ///         The GUI of the program.
    let view model dispatch =
        DockPanel.create [
            DockPanel.background backgroundBrown
            DockPanel.children [
                StackPanel.create [
                    StackPanel.dock Dock.Left
                    StackPanel.orientation Orientation.Vertical
                    StackPanel.margin 5.0
                    StackPanel.spacing 5.0
                    StackPanel.children [
                        calendarView model dispatch
                        Border.create [
                            Border.borderBrush backgroundBrownDark
                            Border.borderThickness 2.
                            Border.child (
                                StackPanel.create [
                                    StackPanel.orientation Orientation.Vertical
                                    StackPanel.children [
                                        tzolkinDateDisplay model dispatch
                                        tzolkinDescDisplay model dispatch
                                    ]
                                ]
                            )
                        ]
                    ]
                ]
                StackPanel.create [
                    StackPanel.dock Dock.Left
                    StackPanel.orientation Orientation.Vertical
                    StackPanel.margin 5.0
                    StackPanel.spacing 5.0
                    StackPanel.children [
                        moonView model dispatch
                        nineWaveInfo model dispatch
                    ]
                ]
                nineWavesGraph model dispatch
            ]
        ]

    /// The main application window.
    type MainWindow() as this =
        inherit HostWindow()
        do
            base.Title <- "TzolkinWaves"
            base.Width <- globalWidth
            base.Height <- globalHeight
            base.MinWidth <- globalWidth
            base.MinHeight <- globalHeight
#if DEBUG
            this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
            this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
#endif

            Elmish.Program.mkProgram init update view
            |> Program.withHost this
#if DEBUG
            |> Program.withConsoleTrace
#endif
            |> Program.run
