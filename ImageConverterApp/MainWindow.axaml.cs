using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ImageConverterApp.Models;
using System;

namespace ImageConverterApp
{
   public partial class MainWindow : Window
   {
      private int _pressedCount = 0;

      public MainWindow()
      {
         InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

         DataContext = new MainWindowViewModel() { InfoText = "Pressed 0 times!"};
      }

      private void InitializeComponent()
      {
         AvaloniaXamlLoader.Load(this);
      }

      private MainWindowViewModel GetWindowViewModel()
      {
         if (DataContext is not MainWindowViewModel dataContext)
         {
            throw new Exception("Could not find MainWindow DataContext");
         }
         return dataContext;
      }

      public void OnConvertBtnClicked(object sender, RoutedEventArgs args)
      {
         var context = GetWindowViewModel();
         _pressedCount += 1;
         context.InfoText = $"Pressed {_pressedCount} times!";
      }
   }
}
