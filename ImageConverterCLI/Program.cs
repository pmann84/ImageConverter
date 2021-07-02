using CommandLine;
using CommandLine.Text;
using ImageConverterLib;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageConverterCLI
{
   class Program
   {
      static async Task<int> Main(string[] args)
      {
         Console.WriteLine("Starting ImageConverter!");
         var parser = new Parser(cfg => cfg.CaseInsensitiveEnumValues = true);
         return await parser.ParseArguments<CommandLineOptions>(args)
            .MapResult(async (CommandLineOptions opts) =>
            {
               Console.WriteLine($"Read arguments: {opts}");
               List<MagickFormat> SupportedFormats = new List<MagickFormat> { MagickFormat.Jpg, MagickFormat.Jpeg, MagickFormat.Heic, MagickFormat.Heif };
               if (!SupportedFormats.Contains(opts.OutputFormat))
               {
                  Console.WriteLine("Error! Invalid Image format");
                  return -1; // Unhandled error
               }
               try
               {
                  // We have the parsed arguments, so let's just pass them down
                  Console.WriteLine($"Attempting to convert image {opts.Path} to format {opts.OutputFormat}...");
                  return await ImageConverter.ConvertAsync(opts.ToImageConversion());
               }
               catch (Exception e)
               {
                  Console.WriteLine($"Error! {e.Message}");
                  return -3; // Unhandled error
               }
            },
            errs =>
            {
               foreach (var err in errs)
               {
                  // TODO: show help text
                  Console.WriteLine($"Invalid arguments: {err}");
               }
               return Task.FromResult(-1);
            }); // Invalid arguments
      }
   }
}
