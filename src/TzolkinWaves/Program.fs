// SPDX-License-Identifier: MIT
// Copyright (C) 2021 Roland Csaszar
//
// Project:  TzolkinWaves
// File:     Program.fs
// Date:     07.May.2021
//
//=============================================================================

namespace TzolkinWaves

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI
open Avalonia.Logging

/// The application's main class.
type App() =
    inherit Application ()

    override this.Initialize () =
        this.Styles.Load "avares://Avalonia.Themes.Fluent/FluentLight.xaml"
        this.Styles.Load "avares://Avalonia.Themes.Fluent/Accents/FluentBaseLight.xaml"
        //this.Styles.Load "avares://Avalonia.Themes.Fluent/FluentDark.xaml"
        //this.Styles.Load "avares://Avalonia.Themes.Fluent/Accents/FluentBaseDark.xaml"
        this.Styles.Load "avares://TzolkinWaves/Resources/Styles.xaml"

    override this.OnFrameworkInitializationCompleted () =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- View.MainWindow ()
        | _ -> ()

/// Holds the main entry point of the program, `main`.
module Program =

    [<EntryPoint>]
    let main args =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
#if DEBUG
            .LogToTrace(level=LogEventLevel.Debug, areas=[|LogArea.Property; LogArea.Layout|])
#endif
            .StartWithClassicDesktopLifetime(args=args)

