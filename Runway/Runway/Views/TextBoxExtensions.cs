using System.Windows.Controls;
using Runway.ViewModels;

namespace Runway.Views
{
   public static class TextBoxExtensions
   {
      public static void MoveCaret( this TextBox textBox, CaretPosition caretPosition )
      {
         if ( caretPosition == CaretPosition.Start )
         {
            textBox.SelectionStart = 0;
         }
         else if ( caretPosition == CaretPosition.End )
         {
            textBox.SelectionStart = textBox.Text.Length;
         }
      }
   }
}
