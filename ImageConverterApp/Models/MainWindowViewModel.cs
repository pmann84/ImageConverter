using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ImageConverterApp.Models
{
   class MainWindowViewModel : ViewModelBase
   {
      public ObservableCollection<ImageConversion> ImageConversions { get; }

      public MainWindowViewModel()
      {
         ImageConversions = new ObservableCollection<ImageConversion>();
      }

      public void AddImageToConvert(ImageConversion conversion)
      {
         ImageConversions.Add(conversion);
      }

      public bool CanAddImage(ImageConversion conversion)
      {
         return !ImageConversions.Contains(conversion);
      }
   }
}