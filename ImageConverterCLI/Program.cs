using CommandLine;
using ImageConverterLib;
using Serilog;
using System;
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
              
               if (!ImageConverter.SupportedFormats.Contains(opts.OutputFormat))
               {
                  logger.Error("Error! Invalid Image format");
                  return -1; // Unhandled error
               }
               try
               {
                  // We have the parsed arguments, so let's just pass them down
                  var imageConversionOpts = opts.ToImageConversion();
                  if (imageConversionOpts.Count() == 1)
                  {
                     var result = await imageConverter.ConvertAsync(imageConversionOpts.Single());
                     var successCount = result.Succeeded ? 1 : 0;
                     var failedCount = !result.Succeeded ? 1 : 0;
                     logger.Information($"Finished converting 1 image in {result.TimeTaken.TotalSeconds}s ({result.TimeTaken.TotalSeconds}s/image). {successCount} succeeded, {failedCount} failed.");
                     return result.Succeeded ? 0 : 1;
                  }
                  else
                  {
                     var results = await imageConverter.ConvertAsync(imageConversionOpts, opts.MaxConcurrency);
                     var totalTimeTaken = TimeSpan.FromTicks(results.Sum(r => r.TimeTaken.Ticks));
                     logger.Information($"Finished converting {results.Count()} images in {totalTimeTaken.TotalSeconds}s ({totalTimeTaken.TotalSeconds / results.Count()}s/image). {results.Count(r => r.Succeeded)} succeeded, {results.Count(r => !r.Succeeded)} failed.");
                     return results.Any(r => r.Succeeded == false) ? 1 : 0;
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
