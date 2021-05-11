// SPDX-License-Identifier: MIT
// Copyright (C) 2021 Roland Csaszar
//
// Project:  TzolkinWaves
// File:     Update.fs
// Date:     5/10/2021 8:35:08 PM
//==============================================================================

namespace TzolkinWaves

open Elmish

[<AutoOpen>]
module Update =

    /// The MVU update function.
    ///
    /// Params:
    ///         `msg` The message to process
    ///         `model` The MVU model instance that hold's the program's state.
    ///
    /// Returns:
    ///          The updated model and a `Cmd` to execute.
    let update msg model =
       match msg with
       | ChangeDate date -> { model with Date = date }, Cmd.none
