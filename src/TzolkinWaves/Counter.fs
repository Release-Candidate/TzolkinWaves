// SPDX-License-Identifier: MIT
// Copyright (C) 2021 Roland Csaszar
//
// Project:  TzolkinWaves
// File:     Counter.fs
// Date:     07.May.2021
//
//=============================================================================

namespace TzolkinWaves

module Counter =
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout

    type State = { Count : int }
    let init = { Count = 0 }

    type Msg = Increment | Decrement | Reset

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Increment -> { state with Count = state.Count + 1 }
        | Decrement -> { state with Count = state.Count - 1 }
        | Reset -> init

    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                StackPanel.create [
                    StackPanel.dock Dock.Bottom
                    StackPanel.margin 5.0
                    StackPanel.spacing 5.0
                    StackPanel.children [
                        Button.create [
                            Button.onClick (fun _ -> dispatch Increment)
                            Button.content "+"
                            Button.classes [ "plus" ]
                        ]
                        Button.create [
                            Button.onClick (fun _ -> dispatch Decrement)
                            Button.content "-"
                            Button.classes [ "minus" ]
                        ]
                        Button.create [
                            Button.onClick (fun _ -> dispatch Reset)
                            Button.content "reset"
                        ]
                    ]
                ]

                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.fontSize 48.0
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.text (string state.Count)
                ]
            ]
        ]
