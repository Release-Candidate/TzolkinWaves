// SPDX-License-Identifier: MIT
// Copyright (C) 2021 Roland Csaszar
//
// Project:  TzolkinWaves
// File:     Shell.fs
// Date:     07.May.2021
//
//=============================================================================

namespace TzolkinWaves

module Shell =
    open Elmish
    open Avalonia
    open Avalonia.Controls
    open Avalonia.Input
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Components.Hosts
    open Avalonia.FuncUI.Elmish

    type State =
        { AboutState: About.State; CounterState: Counter.State;}

    type Msg =
        | AboutMsg of About.Msg
        | CounterMsg of Counter.Msg

    let init =
        let aboutState, aboutCmd = About.init
        let counterState = Counter.init
        { AboutState = aboutState; CounterState = counterState },
        Cmd.batch [ aboutCmd ]

    let update (msg: Msg) (state: State): State * Cmd<_> =
        match msg with
        | AboutMsg bpmsg ->
            let aboutState, cmd =
                About.update bpmsg state.AboutState
            { state with AboutState = aboutState },
            Cmd.map AboutMsg cmd

        | CounterMsg countermsg ->
            let counterMsg =
                Counter.update countermsg state.CounterState
            { state with CounterState = counterMsg },
            Cmd.none

    let view (state: State) (dispatch) =
        DockPanel.create
            [ DockPanel.children
                [ TabControl.create
                    [ TabControl.tabStripPlacement Dock.Top
                      TabControl.viewItems
                          [ TabItem.create
                                [ TabItem.header "Counter Sample"
                                  TabItem.content (Counter.view state.CounterState (CounterMsg >> dispatch)) ]
                            TabItem.create
                                [ TabItem.header "About"
                                  TabItem.content (About.view state.AboutState (AboutMsg >> dispatch)) ] ] ] ] ]

    type MainWindow() as this =
        inherit HostWindow()
        do
            base.Title <- "TzolkinWaves"
            base.Width <- 800.0
            base.Height <- 600.0
            base.MinWidth <- 800.0
            base.MinHeight <- 600.0
#if DEBUG
            this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
            this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
#endif

            Elmish.Program.mkProgram (fun () -> init) update view
            |> Program.withHost this
            |> Program.run
