using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImageConverterLib
{
   public class ImageConverter
   {

      private static async Task ConvertImageAsync(ImageConversionOptions opts)
      {
         using (MemoryStream stream = new MemoryStream())
         {
            using (MagickImage image = new MagickImage(opts.ImagePath))
            {
               // TODO: Preserve Bit depth
               image.Format = opts.DestinationFormat;
               await image.WriteAsync(stream);
               stream.Position = 0;
               MagickImage converted = new MagickImage(stream);
               await converted.WriteAsync(opts.OutputImagePath);
            }
         }
      }

      public static async Task<int> ConvertAsync(ImageConversionOptions imageToConvert)
      {
         // Check imagePath actually exists!
         if (!imageToConvert.IsValid())
         {
            Console.WriteLine($"Image [{imageToConvert.ImagePath}] does not exist!");
            return 1;
         }

         // Read input
         try
         {
            await ConvertImageAsync(imageToConvert);
         }
         catch (Exception e)
         {
            Console.WriteLine($"Could not convert image [{imageToConvert.ImagePath}]: {e.Message}");
            return 1;
         }
         return 0;
      }

      //public static async Task ConvertAsync(IEnumerable<ImageConversionOptions> images)
      //{
         //int maxConcurrency=10;
         //var messages = new List<string>();
         //using(SemaphoreSlim concurrencySemaphore = new SemaphoreSlim(maxConcurrency))
         //{
         //    List<Task> tasks = new List<Task>();
         //    foreach(var msg in messages)
         //    {
         //        concurrencySemaphore.Wait();
         //        var t = Task.Factory.StartNew(() =>
         //        {
         //            try
         //            {
         //                 Process(msg);
         //            }
         //            finally
         //            {
         //                concurrencySemaphore.Release();
         //            }
         //        });

         //        tasks.Add(t);
         //    }

         //    Task.WaitAll(tasks.ToArray());
         //}
      //}
   }
}
