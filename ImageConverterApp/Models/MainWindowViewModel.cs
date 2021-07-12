using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ImageConverterApp.Models
{
   class MainWindowViewModel : ViewModelBase
   {
      public ObservableCollection<ImageConversion> ImageConversions { get; }

      private string _infoText;
      public string InfoText
      {
         get { return _infoText; }
         set
         {
            if (value != _infoText)
            {
               _infoText = value;
               OnPropertyChanged();
            }
         }
      }

      public MainWindowViewModel()
      {
         ImageConversions = new ObservableCollection<ImageConversion>(GenerateTestConversions());
      }

      private IEnumerable<ImageConversion> GenerateTestConversions()
      {
         int numConversions = 30;

         var testConversions = new List<ImageConversion>();
         for (var i = 0; i < numConversions; i++)
         {
            testConversions.Add(new ImageConversion()
            {
               InputPath = $"C:\\Projects\\ImageConverter\\TestData\\IMG_{i}.HEIC"
            });
         }
         return testConversions;
      }
   }
}