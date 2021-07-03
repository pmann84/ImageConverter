using CommandLine;
using ImageConverterLib;
using ImageMagick;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ImageConverterCLI
{
   class CommandLineOptions
   {
      [Value(index: 0, Required = true, HelpText = "Image file Path to convert.")]
      public IEnumerable<string> Paths { get; set; }

      [Option(shortName: 'f', longName: "format", Required = true, HelpText = "Output image format to convert to.")]
      public MagickFormat OutputFormat { get; set; }

      [Option(shortName: 'o', longName: "outputDir", Required = false, HelpText = "The directory to output the image to.", Default = null)]
      public string OutputDirectory { get; set; }

      [Option(shortName: 't', longName: "threads", Required = false, HelpText = "The number of images to process in parallel at one time.", Default = 4)]
      public int MaxConcurrency { get; set; }

      public override string ToString()
      {
         var sb = new StringBuilder("Image Path(s):");
         foreach (var path in Paths)
         {
            sb.Append($"{path},");
         }
         sb.Append($";Output Format: {OutputFormat};");
         if (OutputDirectory != null)
         {
            sb.Append($"OutputDirectory: {OutputDirectory}");
         }
         return sb.ToString();
      }

      public IEnumerable<ImageConversionOptions> ToImageConversion()
      {
         return Paths.Select(path => new ImageConversionOptions
         {
            ImagePath = path,
            DestinationFormat = OutputFormat,
            OutputDirectory = OutputDirectory
         });
      }
   }
}
