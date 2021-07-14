using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ImageConverterApp.Models;
using ImageConverterLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageConverterApp
{
   public partial class MainWindow : Window
   {
      private readonly ImageConverter _imageConverter;
      private Task<IEnumerable<ImageConversionResults>>? _conversionTasks;

      public MainWindow()
      {
         InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

         _imageConverter = new ImageConverter();
         //_conversionTasks = new Task<IEnumerable<ImageConversionResults>>();
         DataContext = new MainWindowViewModel();
      }

      private void InitializeComponent()
      {
         AvaloniaXamlLoader.Load(this);

         AddHandler(DragDrop.DropEvent, OnDrop);
         AddHandler(DragDrop.DragOverEvent, OnDragOver);
      }

      private MainWindowViewModel GetWindowViewModel()
      {
         if (DataContext is not MainWindowViewModel dataContext)
         {
            throw new Exception("Could not find MainWindow DataContext");
         }
         return dataContext;
      }

      // https://stackoverflow.com/questions/9602567/how-to-update-ui-from-another-thread-running-in-another-class
      public void OnConvertBtnClicked(object sender, RoutedEventArgs args)
      {
         // TODO: Check if there are tasks that are running first, if all complete then check we arent re running it
         if (_conversionTasks.IsCompleted)
         var context = GetWindowViewModel();
         var conversions = context.ImageConversions.Select(c => c.ToImageConversionOption()).ToList();
         // Start progress spinners - https://stackoverflow.com/questions/48544635/wpf-how-can-i-display-a-column-in-a-datagrid-with-animated-gif-and-normal-png
         // Convert!
         _conversionTasks = _imageConverter.ConvertAsync(conversions, 4);
      }

      public void OnAddBtnClicked(object sender, RoutedEventArgs args)
      {
         // Launch file dialog
      }

      public void OnDrop(object? sender, DragEventArgs e)
      {
         var files = e.Data.GetFileNames();
         // TODO: Validate files and add to the view model
         if (files != null)
         {
            var context = GetWindowViewModel();
            foreach (var file in files)
            {
               var conv = new ImageConversion() { InputPath = file };
               if (context.CanAddImage(conv))
               {
                  context.AddImageToConvert(conv);
               }
            }
         }
      }

      public void OnDragOver(object? sender, DragEventArgs e)
      {

      }
   }
}


//protected override void OnDrop(DragEventArgs e)
//{
//   base.OnDrop(e);

//   // If the DataObject contains string data, extract it.
//   if (e.Data.GetDataPresent(DataFormats.StringFormat))
//   {
//      string dataString = (string)e.Data.GetData(DataFormats.StringFormat);

//      // If the string can be converted into a Brush,
//      // convert it and apply it to the ellipse.
//      BrushConverter converter = new BrushConverter();
//      if (converter.IsValid(dataString))
//      {
//         Brush newFill = (Brush)converter.ConvertFromString(dataString);
//         circleUI.Fill = newFill;

//         // Set Effects to notify the drag source what effect
//         // the drag-and-drop operation had.
//         // (Copy if CTRL is pressed; otherwise, move.)
//         if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
//         {
//            e.Effects = DragDropEffects.Copy;
//         }
//         else
//         {
//            e.Effects = DragDropEffects.Move;
//         }
//      }
//   }
//   e.Handled = true;
//}