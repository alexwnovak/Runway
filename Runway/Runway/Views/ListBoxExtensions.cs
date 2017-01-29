using System.Windows.Controls;

namespace Runway.Views
{
   public static class ListBoxExtensions
   {
      public static void SelectNext( this ListBox listBox )
      {
         int selectedIndex = listBox.SelectedIndex;

         if ( selectedIndex + 1 >= listBox.Items.Count )
         {
            listBox.SelectedIndex = 0;
         }
         else
         {
            listBox.SelectedIndex++;
         }
      }

      public static void SelectPrevious( this ListBox listBox )
      {
         int selectedIndex = listBox.SelectedIndex;

         if ( selectedIndex - 1 < 0 )
         {
            listBox.SelectedIndex = listBox.Items.Count - 1;
         }
         else
         {
            listBox.SelectedIndex--;
         }
      }
   }
}
