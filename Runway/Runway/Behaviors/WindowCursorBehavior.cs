using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interactivity;

namespace Runway.Behaviors
{
   public class WindowCursorBehavior : Behavior<Window>
   {
      [DllImport( "user32.dll" )]
      private static extern int ShowCursor( bool bShow );

      protected override void OnAttached() => AssociatedObject.Activated += OnActivated;

      private void OnActivated( object sender, EventArgs e )
      {
         ShowCursor( false );
      }
   }
}
