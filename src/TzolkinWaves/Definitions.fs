// SPDX-License-Identifier: MIT
// Copyright (C) 2021 Roland Csaszar
//
// Project:  TzolkinWaves
// File:     Definitions.fs
// Date:     5/10/2021 7:50:25 PM
//==============================================================================

/// The program is contained in this namespace.
namespace TzolkinWaves

/// All
[<AutoOpen>]
module Definitions=

    open RC.Maya

    open System
    open Elmish
    open Svg.Skia
    open Avalonia
    open Avalonia.Media
    open Avalonia.Media.Imaging

    // Global constants and instances to use =======================================================

    /// Width of the main window.
    let globalWidth = 1300.

    /// height of the main window.
    let globalHeight = 800.

    let backgroundBrownDark = "#BFAB91"

    let backgroundBrown = "#F2D8B8"

    let backgroundBrownLight = "#F6DCBC"

    let backgroundDark = "#1F1B24"

    let foregroundLight = "Black"

    let foregroundDark = "White"

    let accentDarkRed = "#8B2A02"

    let linkColorBlue = "Blue"

    let linkSymbol = "\U0001F517"

    // Tzolk’in date display =======================================================================

    /// The height of the Tzolk’in day number and glyph.
    let tzolkinImageHeight = 100.f

    /// The font size of the Tzolk’in day number and glyph strings.
    let tzolkinFontSize = 18.

    /// The font style of the Tzolk’in day number and glyph strings.
    let tzolkinFontStyle = FontStyle.Oblique

    /// The font weight of the Tzolk’in day number and glyph strings.
    let tzolkinFontWeight = FontWeight.Bold

    // Moon display ================================================================================

    /// The size of the moon symbols.
    let moonSymbolHeight = 140.

    /// The size of the moon symbol font.
    let moonSymbolSize = 100.

    /// The size of the moon text font.
    let moonFontSize = 18.

    /// The font style of the Tzolk’in day number and glyph strings.
    let moonFontStyle = FontStyle.Oblique

    /// The font weight of the Tzolk’in day number and glyph strings.
    let moonFontWeight = FontWeight.Bold

    // Nine waves display ==========================================================================

    let wavesFontSize = 18.

    let wavesFontStyle = FontStyle.Oblique

    let wavesFontWeight = FontWeight.Bold

    let graphCanvasWidth = 395.

    let graphCanvasHeight = 750.

    let dotCirceRadius = 7.5

    let dotCircleThickness = 3.

    let graphThickness = 4.

    let axisThickness = 3.

    let graphDateTextSize = 12.

    // MVU model ===================================================================================

    /// The MVU model.
    type Model =
     { Date: DateTime }

    // MVU messages ================================================================================

    /// The MVU message.
    type Msg =
     | ChangeDate of DateTime

    // Init function ===============================================================================

    /// Initialize the model to the default starting state.
    ///
    /// Returns:
    ///          The initialized model and the commands to execute on startup.
    let init () =
        { Date = DateTime.Today }, Cmd.none

    // GUI functions and resources =================================================================

    /// The program's asset loader, needed to load Avalonia resources.
    let assets = AvaloniaLocator.Current.GetService<Avalonia.Platform.IAssetLoader> ();

    /// Takes a relative path in the directory `TzolkinWaves/Resources` and
    /// returns a stream of the data at that path.
    ///
    /// Params:
    ///         `path` The relative path of the resource to load.
    ///
    /// Returns:
    ///         An `IO.Stream` instance to use the resource at the given path.
    let getStreamPath path =
        sprintf "avares://TzolkinWaves/Resources/%s" path
        |> System.Uri
        |> assets.Open

    /// Return an `IO.Stream` of SVG with filename `name`.svg in the directory
    /// `TzolkinWaves/Resources`.
    let inline getStreamSVG name =
        sprintf "%s.svg" name
        |> getStreamPath

    /// Return an `IO.Stream` of a Tzolk’in day glyph SVG.
    let inline getStreamTGlyph (glyph: TzolkinGlyph.T) =
        int glyph
        |> sprintf "glyph_%02d"
        |> getStreamSVG

    /// Return an `IO.Stream` of a Tzolk’in day number SVG.
    let inline getStreamTNumber (number: TzolkinNumber.T) =
        int number
        |> sprintf "number_%02d"
        |> getStreamSVG

    /// Convert a SVG given by its resource stream `stream` to a PNG with the
    /// given height `height`.
    let bitmapFromSVGStream height (stream: IO.Stream) =
        let svg = new SKSvg ()
        let svgPicture = svg.Load (stream)
        let scaleFac = height / svgPicture.CullRect.Height
        let bitmap = svgPicture.ToBitmap (SkiaSharp.SKColor.Empty,
                                            scaleFac,
                                            scaleFac,
                                            SkiaSharp.SKColorType.Rgba8888,
                                            SkiaSharp.SKAlphaType.Premul,
                                            SkiaSharp.SKColorSpace.CreateSrgb () )
        let image =  SkiaSharp.SKImage.FromBitmap (bitmap)
        let data = image.Encode(SkiaSharp.SKEncodedImageFormat.Png, 100)
        let stream = data.AsStream (true)
        ImageBrush (new Bitmap (stream))

    /// Generic function to get an `ImageBrush` from a resource.
    let inline private getPNGBrush streamFunc height instance =
        streamFunc instance
        |> bitmapFromSVGStream height

    /// Get an `ImageBrush` of a SVG converted to PNG with the given height and
    /// the name of the SVG without the suffix (.svg).
    let getPNGBrushSVG = getPNGBrush getStreamSVG

    /// Get a `ImageBrush` of a PNG from a Tzolk’in day glyph.
    let getPNGBrushGlyph = getPNGBrush getStreamTGlyph tzolkinImageHeight

    /// Get a `ImageBrush` of a PNG from a Tzolk’in day number.
    let getPNGBrushNumber = getPNGBrush getStreamTNumber tzolkinImageHeight

    /// The list of all Tzolk’in day glyph PNG images as `ImageBrush`.
    let cacheGlyphs = [ for i in [1 .. 20] -> getPNGBrushGlyph <| TzolkinGlyph.T.TzolkinGlyph i ]

    /// The list of all Tzolk’in day number PNG images as `ImageBrush`.
    let cacheNumbers = [ for i in [1 .. 13] -> getPNGBrushNumber <| TzolkinNumber.T.TzolkinNumber i ]
