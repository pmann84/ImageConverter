﻿using ImageMagick;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ImageConverterLib
{
   public class ImageConverter
   {
      private static readonly List<MagickFormat> _supportedFormats = new() { MagickFormat.Jpg, MagickFormat.Jpeg, MagickFormat.Heic, MagickFormat.Heif };
      public static List<MagickFormat> SupportedFormats { get => _supportedFormats; }

      private readonly ILogger? _logger;

      public ImageConverter()
      {

      }

      public ImageConverter(ILogger logger)
      {
         _logger = logger;
      }

      private async Task ConvertImageAsync(ImageConversionOptions opts)
      {
         _logger?.Information($"Starting conversion of [{opts.ImagePath}]...");
         using (MemoryStream stream = new())
         {
            using MagickImage image = new(opts.ImagePath);
            // TODO: Preserve Bit depth
            image.Format = opts.DestinationFormat;
            await image.WriteAsync(stream);
            stream.Position = 0;
            MagickImage converted = new(stream);
            await converted.WriteAsync(opts.OutputImagePath);
         }
         _logger?.Information($"Conversion complete. [{opts.ImagePath}] -> [{opts.OutputImagePath}]");
      }

      public async Task<ImageConversionResults> ConvertAsync(ImageConversionOptions imageToConvert)
      {
         // Check imagePath actually exists!
         if (!imageToConvert.IsValid())
         {
            _logger?.Error($"Image [{imageToConvert.ImagePath}] does not exist!");
            return new ImageConversionResults(imageToConvert, false, TimeSpan.Zero);
         }

         // Read input
         try
         {
            var watch = Stopwatch.StartNew();
            await ConvertImageAsync(imageToConvert);
            watch.Stop();
            var elapsedSeconds = watch.ElapsedMilliseconds / 1000.0;
            _logger?.Information($"Image {imageToConvert.ImagePath} converted in {elapsedSeconds}s");
            return new ImageConversionResults(imageToConvert, true, TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
         }
         catch (Exception e)
         {
            _logger?.Error($"Could not convert image [{imageToConvert.ImagePath}]: {e.Message}");
            return new ImageConversionResults(imageToConvert, false, TimeSpan.Zero);
         }
      }

      public async Task<IEnumerable<ImageConversionResults>> ConvertAsync(IEnumerable<ImageConversionOptions> images, int maxConcurrency = 2)
      {
         using (SemaphoreSlim concurrencySemaphore = new(maxConcurrency))
         {
            var tasks = new List<Task<ImageConversionResults>>();
            foreach (var image in images)
            {
               _logger?.Information($"Waiting to start conversion for image [{image.ImagePath}]");
               concurrencySemaphore.Wait();
               var t = Task.Run(() => 
               {
                  try
                  {
                     return ConvertAsync(image);
                  }
                  finally
                  {
                     concurrencySemaphore.Release();
                  }
               });

               tasks.Add(t);
            }

            var results = await Task.WhenAll(tasks.ToArray());
            return results;
         }
      }
   }
}
