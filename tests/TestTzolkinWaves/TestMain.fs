// SPDX-License-Identifier: MIT
// Copyright (C) 2021 Roland Csaszar
//
// Project:  TzolkinWaves
// File:     TestMain.fs
// Date:     07.May.2021
//
//=============================================================================

namespace TestTzolkinWaves

open Expecto

/// Test module containing the main entry point for the `standalone` tests, when
/// called by `dotnet run` instead of `dotnet test`.
module TestMain=

    /// Main entry point of the tests, if called by `dotnet run` instead of
    /// `dotnet test`.
    [<EntryPoint>]
    let main argv =
        Tests.runTestsInAssembly defaultConfig argv
