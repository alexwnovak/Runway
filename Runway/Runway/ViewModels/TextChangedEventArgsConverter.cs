using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;

namespace Runway.ViewModels
{
   public class TextChangedEventArgsConverter : IEventArgsConverter
   {
      public object Convert( object value, object parameter )
      {
         var textChangedEventArgs = (TextChangedEventArgs) value;
         return ((TextBox) textChangedEventArgs.Source).Text;
      }
   }
}
