// SPDX-License-Identifier: MIT
// Copyright (C) 2021 Roland Csaszar
//
// Project:  TzolkinWaves
// File:     build.fsx
// Date:     07.May.2021
//
//=============================================================================

#r "paket:
nuget Fake.Tools.Git
nuget Fake.DotNet.NuGet
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.Core.Target
nuget Fake.Core.ReleaseNotes //"
#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.DotNet.NuGet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

Target.initEnvironment ()

// set the path to the changelog file to parse
let changelogPath = "./CHANGELOG.md"

// set output paths ============================================================
let packageOutputPath = "nuget" // Nuget packages go here
let testOutputPath = "test_results" // test results and coverage data
let exeOutPath = "bin" // dotnet publish into this

// set absolute output paths ===================================================
let rootDirAbs = Path.getFullName "." // current working dir, absolute
let testOutAbs = Path.combine rootDirAbs testOutputPath // test result dir, absolute
let coverageFile = Path.combine testOutAbs "coverage.xml" // absolute path to coverage file

// set the projects to pass to each target =====================================

// projects to build, this should be all projects ...
let buildProjs =
    !! "src/**/*.*proj" ++ "tests/**/*.*proj"

// projects to test / test projects in `tests`, not the ones in `src`
let testProjs = !! "tests/**/*.*proj"

// test coverage packages
let coverageProjs = testProjs

// projects to generate Nuget packages of
let packageProjs = []

// projects to publish (`dotnet publish`)
let publishProjs = !! "src/**/*.*proj" ++ "tests/**/*.*proj"

// projects that should be linted
let lintProjs =
    !! "src/**/*.*proj" ++ "tests/**/*.*proj"

// projects to run - should be only one.
let runProjs = !! "src/TzolkinWaves/*.*proj"

//==============================================================================

// set common build options and `debug` and `release` configuration ============
let commonDotNetOpts = DotNet.Options.Create()

let commonBuildOpts = commonDotNetOpts


let uploadOpts = NuGet.NuGetPushParams.Create()
let release = DotNet.BuildConfiguration.Release
let debug = DotNet.BuildConfiguration.Debug

// Build options for the targets `Build` and `BuildDeb` ========================
let setBuildOpts config (opts: DotNet.BuildOptions) =
    { opts with
          NoLogo = true
          Common = commonBuildOpts
          Configuration = config }

let setBuildOptsDeb = setBuildOpts debug
let setBuildOptsRel = setBuildOpts release

// Test options for the target `Tests` =========================================
let setTestOpts config (opts: DotNet.TestOptions) =
    { opts with
          Configuration = config
          NoBuild = false
          NoLogo = true
          Common =
              { commonDotNetOpts with
                    Verbosity = Some DotNet.Verbosity.Normal }
          ResultsDirectory = Some testOutputPath
          Logger = Some "trx" }

let setTestOptsRel = setTestOpts release
let setTestOptsDeb = setTestOpts debug

// Coverage test options for the targets `TestCoverage` and `TestCoverageDeb` ==
let setCoverageOpts config (opts: DotNet.TestOptions) =
    { opts with
          Configuration = config
          NoBuild = false
          NoLogo = true
          Common =
              { commonDotNetOpts with
                    CustomParams = Some(sprintf "/p:AltCover=true /p:AltCoverReport=%s" coverageFile)
                    Verbosity = Some DotNet.Verbosity.Normal }
          ResultsDirectory = Some testOutputPath }

let setCoverageOptsRel = setCoverageOpts release
let setCoverageOptsDeb = setCoverageOpts debug

// Nuget packaging options for target `Packages` ===============================
let setPackageOpts version (opts: DotNet.PackOptions) =
    { opts with
          NoLogo = true
          NoBuild = true
          OutputPath = Some packageOutputPath
          IncludeSymbols = false
          Configuration = release
          Common =
              { commonDotNetOpts with
                    CustomParams = Some(sprintf "/property:Version=%s  -p:FileVersion=%s" version version) } }

// Nuget upload options for `Upload` ===========================================
let setUploadOpts (opts: DotNet.NuGetPushOptions) =
    { opts with
          Common = commonDotNetOpts
          PushParams =
              { uploadOpts with
                    Source = Some "https://api.nuget.org/v3/index.json"
                    NoSymbols = true
                    DisableBuffering = true
                    ApiKey = None
              //  NoServiceEndpoint = false
              }
    //  NoServiceEndpoint = false
    }

// Publish options for Target `Publish` ========================================
let setPublishOptions rid (opts: DotNet.PublishOptions) =
    { opts with
          NoLogo = true
          NoBuild = false
          OutputPath = Some exeOutPath
          Configuration = release
          SelfContained = Some true
          Runtime = Some rid
          Common = commonBuildOpts
          VersionSuffix = Some rid }

//==============================================================================


//==============================================================================
//                             Changelog
//
let getChangelogVersion () =
    // load the changelog and get the version number
    try
        let changelog = ReleaseNotes.load changelogPath
        let changelogVersion = changelog.SemVer
        Trace.trace (sprintf "Changelog version '%s'" changelogVersion.AsString)
        Some changelogVersion
    with
    | excp
    | excp ->
        (Trace.traceError (sprintf "Error '%s' parsing changelog '%s'" excp.Message changelogPath)
         failwith "Error: no version found in the Changelog! Did you forget setting it?")

let getVersion (changelogVers: SemVerInfo option) (argVersion: SemVerInfo) =
    match changelogVers with
    | None -> failwith "Error: no version in Changelog found!"
    | Some vers ->
        if vers <> argVersion then
            failwith (
                sprintf
                    "Error: The version in the Changelog is %s from command line is %s"
                    vers.AsString
                    argVersion.AsString
            )
        else
            vers.AsString

//==============================================================================
//                             Set OS Name
//
let getRID () =
    if Environment.isLinux then "linux-x64"
    elif Environment.isMacOS then "osx-x64"
    else "win-x64"

//==============================================================================

//==============================================================================
//                               Clean
//
Target.create
    "Clean"
    (fun _ ->
        !! "src/**/bin"
        ++ "src/**/obj"
        ++ "tests/**/bin"
        ++ "tests/**/obj"
        ++ packageOutputPath
        ++ testOutputPath
        ++ exeOutPath
        |> Shell.cleanDirs

        !! "src/**/*.exn"
        ++ "tests/**/*.exn"
        ++ "tests/**/coverage.xml"
        |> Seq.iter File.delete)

//==============================================================================
//                               Distclean
//
Target.create
    "Distclean"
    (fun _ ->
        !! "site/"
        ++ ".ionide/"
        ++ ".fake"
        ++ ".paket"
        ++ ".paket-files"
        ++ ".vs"
        |> Shell.deleteDirs)


//==============================================================================
//                               Lint
//
let execFSharpLint projFile =
    let result =
        DotNet.exec id "fsharplint" (sprintf "lint %s" projFile)

    if result.ExitCode <> 0 then
        failwith "Lint exited with an error!"

Target.create "Lint" (fun _ -> lintProjs |> Seq.iter execFSharpLint)


//==============================================================================
//                               Build
//
Target.create
    "Build"
    (fun _ ->
        buildProjs
        |> Seq.iter (DotNet.build setBuildOptsRel))

//==============================================================================
//                              Debug Build
//
Target.create
    "BuildDeb"
    (fun _ ->
        buildProjs
        |> Seq.iter (DotNet.build setBuildOptsDeb))

//==============================================================================
//                               Run
//
let execRun projFile =
    let result =
        DotNet.exec id "run" (sprintf "-p %s" projFile)

    if result.ExitCode <> 0 then
        failwith "dotnet run exited with an error!"

Target.create "Run" (fun _ ->  runProjs |> Seq.iter execRun)


//==============================================================================
//                               Docs
//
Target.create
    "Docs"
    (fun _ ->
        CreateProcess.fromRawCommand "pipenv" [ "run"; "mkdocs"; "build" ]
        |> CreateProcess.ensureExitCode // will make sure to throw on error
        |> CreateProcess.withWorkingDirectory "."
        |> Proc.run
        |> ignore)

//==============================================================================
//                               Tests
//
Target.create "Tests" (fun _ -> testProjs |> Seq.iter (DotNet.test setTestOptsRel))

//==============================================================================
//                             Debug Tests
//
Target.create "TestsDeb" (fun _ -> testProjs |> Seq.iter (DotNet.test setTestOptsDeb))

//==============================================================================
//                     Tests & Coverage (Always Debug)
//
Target.create
    "TestsCoverage"
    (fun _ ->
        coverageProjs
        |> Seq.iter (DotNet.test setCoverageOptsDeb))

//==============================================================================
//                           Debug Tests & Coverage
//
Target.create
    "TestsCoverageDeb"
    (fun _ ->
        coverageProjs
        |> Seq.iter (DotNet.test setCoverageOptsDeb))


//==============================================================================
//                             Packages
//
Target.create
    "Packages"
    (fun p ->
        let changelogVers = getChangelogVersion ()
        let argVersion = SemVer.parse p.Context.Arguments.Head

        packageProjs
        |> Seq.iter (DotNet.pack (setPackageOpts (getVersion changelogVers argVersion))))

//==============================================================================
//                          Upload Packages
//
Target.create
    "Upload"
    (fun _ ->
        !!(Path.combine packageOutputPath "*.nupkg")
        |> Seq.iter (DotNet.nugetPush setUploadOpts))


//==============================================================================
//                             Publish
//
Target.create
    "Publish"
    (fun _ ->
        publishProjs
        |> Seq.iter (DotNet.publish (setPublishOptions (getRID ()) ) ) )


//==============================================================================
//                      Package Fake 4 API
//
//open Fake.DotNet.NuGet
//Target.create "Package" (fun _ ->
//    copyFiles packagingOutputDirectory allFilesToPackage
//    NuGet.NuGet (fun p ->
//        { p with
//            Version = buildVersion
//            Authors = authors
//            Project = projectName
//            Summary = projectSummary
//            Description = projectDescription
//            WorkingDir = packagingDirectory
//            OutputPath = artifactOutputDirectory
//            AccessKey = myAccessKey
//            Publish = true })
//        "template.nuspec"
//)

//==============================================================================
//                                 All
//
Target.create "All" ignore

//==============================================================================
//                                Release
//
Target.create "Release" ignore


//==============================================================================
//                             Target Dependencies
//
"Clean" ==> "Distclean"

"Clean" ==> "Build" ==> "Lint"

"Clean"
==> "Build"
==> "Packages"
==> "Upload"
==> "Release"

"Clean" ==> "Build" ==> "Docs" ==> "Release"

"Clean" ==> "Publish" ==> "Release"

"Clean" ==> "Tests"

"Clean" ==> "TestsDeb"

"Clean" ==> "TestsCoverage"

"Clean" ==> "TestsCoverageDeb"

"Clean" ==> "Build" ==> "Run"

"Clean" ==> "Build" ==> "Docs" ==> "All"

Target.runOrDefaultWithArguments "All"
