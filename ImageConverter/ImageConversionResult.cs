using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageConverterLib
{
   public class ImageConversionResult
   {
      public ImageConversionResult(ImageConversionOptions opts, bool succeeded, TimeSpan timeTaken)
      {
         InputImage = opts.ImagePath;
         InputFormat = (MagickFormat) Enum.Parse(typeof(MagickFormat), Path.GetExtension(InputImage).Replace(".", ""), true);
         OutputImage = opts.OutputImagePath;
         OutputFormat = opts.DestinationFormat;
         Succeeded = succeeded;
         TimeTaken = timeTaken;
      }

      public bool Succeeded { get; }
      public TimeSpan TimeTaken { get; }
      public string InputImage { get; }
      public MagickFormat InputFormat { get; }
      public string OutputImage { get; }
      public MagickFormat OutputFormat { get; }
   }
}
