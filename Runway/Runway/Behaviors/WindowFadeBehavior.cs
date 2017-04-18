using System;
using System.Windows;
using System.Windows.Interactivity;
using Runway.Extensions;

namespace Runway.Behaviors
{
   public class WindowFadeBehavior : Behavior<Window>
   {
      protected override void OnAttached() => AssociatedObject.Activated += OnActivated;
      protected override void OnDetaching() => AssociatedObject.Activated -= OnActivated;

      private void OnActivated( object sender, EventArgs e )
      {
         AssociatedObject.Opacity = 0;
         AssociatedObject.Visibility = Visibility.Visible;
         AssociatedObject.FadeIn();
      }
   }
}
