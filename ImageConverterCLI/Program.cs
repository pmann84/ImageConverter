using CommandLine;
using ImageConverterLib;
using ImageMagick;
using Serilog;
using Serilog.Formatting.Display;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ImageConverterCLI
{
   class Program
   {
      static ILogger CreateLogger()
      {
         return new LoggerConfiguration()
             .WriteTo.Console(outputTemplate: "[{Timestamp:dd/MM/yy HH:mm:ss}][{Level:u3}] {Message:Ij}{NewLine}{Exception}")
             .CreateLogger();
      }

      static async Task<int> Main(string[] args)
      {
         var logger = CreateLogger();
         var imageConverter = new ImageConverter(logger);
         logger.Information("Starting Image Conversion...");
         var parser = new Parser(cfg => cfg.CaseInsensitiveEnumValues = true);
         return await parser.ParseArguments<CommandLineOptions>(args)
            .MapResult(async (CommandLineOptions opts) =>
            {
               logger.Information($"Read arguments: {opts}");
               List<MagickFormat> SupportedFormats = new List<MagickFormat> { MagickFormat.Jpg, MagickFormat.Jpeg, MagickFormat.Heic, MagickFormat.Heif };
               if (!SupportedFormats.Contains(opts.OutputFormat))
               {
                  logger.Error("Error! Invalid Image format");
                  return -1; // Unhandled error
               }
               try
               {
                  var watch = Stopwatch.StartNew();
                  // We have the parsed arguments, so let's just pass them down
                  var imageConversionOpts = opts.ToImageConversion();
                  if (imageConversionOpts.Count() == 1)
                  {
                     var result = await imageConverter.ConvertAsync(imageConversionOpts.Single());
                     var successCount = result == 0 ? 1 : 0;
                     var failedCount = result == 1 ? 1 : 0;
                     watch.Stop();
                     var elapsedSeconds = watch.ElapsedMilliseconds / 1000.0;
                     logger.Information($"Finished converting 1 image in {elapsedSeconds}s ({elapsedSeconds}s/image). {successCount} succeeded, {failedCount} failed.");
                     return result;
                  }
                  else
                  {
                     var results = await imageConverter.ConvertAsync(imageConversionOpts, opts.MaxConcurrency);
                     watch.Stop();
                     var elapsedSeconds = watch.ElapsedMilliseconds / 1000.0;
                     logger.Information($"Finished converting {results.Count()} images in {elapsedSeconds}s ({elapsedSeconds/results.Count()}s/image). {results.Count(r => r == 0)} succeeded, {results.Count(r => r == 1)} failed.");
                     return results.Max();
                  }
               }
               catch (Exception e)
               {
                  logger.Error($"Error! {e.Message}");
                  return -3; // Unhandled error
               }
            },
            errs =>
            {
               foreach (var err in errs)
               {
                  // TODO: show help text
                  logger.Error($"Invalid arguments: {err}");
               }
               return Task.FromResult(-1);
            }); // Invalid arguments
      }
   }
}
