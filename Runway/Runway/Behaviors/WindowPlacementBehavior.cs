using System.Windows;
using System.Windows.Interactivity;

namespace Runway.Behaviors
{
   public class WindowPlacementBehavior : Behavior<Window>
   {
      protected override void OnAttached() => AssociatedObject.Loaded += AssociatedObject_OnLoaded;
      protected override void OnDetaching() => AssociatedObject.Loaded -= AssociatedObject_OnLoaded;

      private void AssociatedObject_OnLoaded( object sender, RoutedEventArgs e )
      {
         AssociatedObject.WindowStartupLocation = WindowStartupLocation.Manual;
         AssociatedObject.Left = ( SystemParameters.FullPrimaryScreenWidth - AssociatedObject.Width ) / 2;
         AssociatedObject.Top = SystemParameters.FullPrimaryScreenHeight / 4;
      }
   }
}
