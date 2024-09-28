﻿// Copyright (c) Georg Jung. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BaseApp.App.Utils;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.ML.OnnxRuntime;
using System.IO;

namespace FaceAiSharp;

public static class FaceAiSharpBundleFactory
{
    private static readonly Lazy<IMemoryCache> _scrfdCache = new(CreateMemoryCache);

    public static IFaceDetector CreateFaceDetector(SessionOptions? sessionOptions = null) => CreateFaceDetectorWithLandmarks(sessionOptions);

    public static IFaceLandmarksDetector CreateFaceLandmarksDetector(SessionOptions? sessionOptions = null) => CreateFaceDetectorWithLandmarks(sessionOptions);

    public static IFaceDetectorWithLandmarks CreateFaceDetectorWithLandmarks(SessionOptions? sessionOptions = null)
    {
        var c = _scrfdCache.Value;
        var modelPath = Path.Combine(GetExeDir(), "resources", "onnx", "scrfd_2.5g_kps.onnx");
        var opt = new ScrfdDetectorOptions() { ModelPath = modelPath };
        return new ScrfdDetector(c, opt, sessionOptions);
    }

    public static IFaceEmbeddingsGenerator CreateFaceEmbeddingsGenerator(SessionOptions? sessionOptions = null)
    {
        var modelPath = Path.Combine(GetExeDir(), "resources", "onnx", "arcfaceresnet100-11-int8.onnx");
        var opt = new ArcFaceEmbeddingsGeneratorOptions() { ModelPath = modelPath };
        return new ArcFaceEmbeddingsGenerator(opt, sessionOptions);
    }

    public static IEyeStateDetector CreateEyeStateDetector(SessionOptions? sessionOptions = null)
    {
        var modelPath = Path.Combine(GetExeDir(), "resources", "onnx", "open_closed_eye.onnx");
        var opt = new OpenVinoOpenClosedEye0001Options() { ModelPath = modelPath };
        return new OpenVinoOpenClosedEye0001(opt, sessionOptions);
    }

    public static FaceAntiSpoofing CreateFaceAntiSpoofingDetector(SessionOptions? sessionOptions = null)
    {
        var modelPath = Path.Combine(GetExeDir(), "resources", "onnx", "v2.onnx");
        return new FaceAntiSpoofing(modelPath);
    }

    private static IMemoryCache CreateMemoryCache()
    {
        var opts = new MemoryCacheOptions();
        var iopts = Options.Create(opts);
        return new MemoryCache(iopts);
    }

    private static string GetExeDir() => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;


    public static byte[] GetDepthGooglenetSlim(SessionOptions? sessionOptions = null)
    {
        var modelPath = Path.Combine(GetExeDir(), "resources", "onnx", "depth_googlenet_slim.onnx");
        return File.ReadAllBytes(modelPath);
    }
}
