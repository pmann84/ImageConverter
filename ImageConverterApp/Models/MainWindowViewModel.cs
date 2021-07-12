using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ImageConverterApp.Models
{
   class MainWindowViewModel : INotifyPropertyChanged
   {
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

      public event PropertyChangedEventHandler? PropertyChanged;

      // Move this to a base class
      //protected bool RaiseAndSetIfChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
      //{
      //   if (!EqualityComparer<T>.Default.Equals(field, value))
      //   {
      //      field = value;
      //      RaisePropertyChanged(propertyName);
      //      return true;
      //   }
      //   return false;
      //}

      protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
   }
}
