using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Runway.Extensions
{
   public static class WindowExtensions
   {
      private const double _fadeDuration = 100;

      public static void FadeOut( this Window window )
      {
         var doubleAnimation = new DoubleAnimation( 1, 0, new Duration( TimeSpan.FromMilliseconds( _fadeDuration ) ) );
         
         doubleAnimation.Completed += ( _, __ ) => window.Visibility = Visibility.Hidden;

         window.BeginAnimation( UIElement.OpacityProperty, doubleAnimation );
      }

      public static void FadeIn( this Window window )
      {
         var doubleAnimation = new DoubleAnimation( 0, 1, new Duration( TimeSpan.FromMilliseconds( _fadeDuration ) ) );
         
         doubleAnimation.Completed += ( _, __ ) => window.Activate();

         window.BeginAnimation( UIElement.OpacityProperty, doubleAnimation );
      }
   }
}
