using CommandLine;
using ImageConverterLib;
using ImageMagick;
using System.Collections.Generic;

namespace ImageConverterCLI
{
   class CommandLineOptions
   {
      [Value(index: 0, Required = true, HelpText = "Image file Path to convert.")]
      public string Path { get; set; }

      [Option(shortName: 'f', longName: "format", Required = true, HelpText = "Output image format to convert to.")]
      public MagickFormat OutputFormat { get; set; }

      [Option(shortName: 'o', longName: "outputDir", Required = false, HelpText = "The directory to output the image to.", Default = null)]
      public string? OutputDirectory { get; set; }

      public override string ToString()
      {
         var str = $"Image Path: {Path}\nOutput Format: {OutputFormat}";
         if (OutputDirectory != null)
         {
            str += $"\nOutputDirectory: {OutputDirectory}";
         }
         return str;
      }

      public ImageConversionOptions ToImageConversion()
      {
         return new ImageConversionOptions
         {
            ImagePath = Path,
            DestinationFormat = OutputFormat,
            OutputDirectory = OutputDirectory
         };
      }
   }
}
