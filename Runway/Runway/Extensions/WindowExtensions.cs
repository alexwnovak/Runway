using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Runway.Views;

namespace Runway.Extensions
{
   public static class WindowExtensions
   {
      private const double _fadeDuration = 700;

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

      public static void SlideOut( this Window window )
      {
         window.Top = -window.Height;
         return;
         var doubleAnimation = new DoubleAnimation( 0, -window.ActualHeight, new Duration( TimeSpan.FromMilliseconds( _fadeDuration ) ) );

         doubleAnimation.Completed += ( _, __ ) => window.Visibility = Visibility.Hidden;

         window.BeginAnimation( Window.TopProperty, doubleAnimation );
      }

      public static void SlideIn( this MainWindow window )
      {
         //var doubleAnimation = new DoubleAnimation( -window.ActualHeight, 0, new Duration( TimeSpan.FromMilliseconds( _fadeDuration ) ) )
         var doubleAnimation = new DoubleAnimation( -100, 0, new Duration( TimeSpan.FromMilliseconds( _fadeDuration ) ) )
         {
            EasingFunction = new CircleEase
            {
               EasingMode = EasingMode.EaseOut
            }
         };

         window.TranslateTransform.BeginAnimation( TranslateTransform.YProperty, doubleAnimation );

         //doubleAnimation.Completed += ( _, __ ) => window.Activate();

         //window.BeginAnimation( Window.TopProperty, doubleAnimation );
      }
   }
}
