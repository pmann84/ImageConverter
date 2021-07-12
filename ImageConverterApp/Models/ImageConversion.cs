using ImageConverterLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageConverterApp.Models
{
   class ImageConversion : IEquatable<ImageConversion>
   {
      public string InputPath { get; set; } = null!;

      public bool Equals(ImageConversion? other)
      {
         if (other == null) return false;
         bool isEqual = InputPath == other.InputPath;
         return isEqual;
      }

      public ImageConversionOptions ToImageConversionOption()
      {
         return new ImageConversionOptions()
         {
            ImagePath = InputPath,
            DestinationFormat = ImageMagick.MagickFormat.Jpeg
         };
      }
   }
}
