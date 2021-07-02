using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageConverterLib
{
   public class ImageConversionOptions
   {
      public string ImagePath { get; set; }
      public MagickFormat DestinationFormat { get; set; }
      public string? OutputDirectory { get; set; }

      /// <summary>
      ///   This property takes the input path and converts it to the output path using the
      ///   destination image format and an optional output directory. If output directory 
      ///   is not supplied then the output path will be the same directory as the input path.
      /// </summary>
      public string OutputImagePath {
         get {
               var filename = Path.GetFileName(ImagePath);
               var newFilename = Path.ChangeExtension(filename, DestinationFormat.ToString());
               return Path.Combine(OutputDirectory ?? Path.GetDirectoryName(ImagePath), newFilename);
         }
      }

      public bool IsValid()
      {
         return File.Exists(ImagePath);
      }
   }
}
