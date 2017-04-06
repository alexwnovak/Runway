using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Runway.ExtensibilityModel;
using Ico = System.Drawing.Icon;

namespace Runway
{
   public abstract class LaunchableCommandBase : ILaunchableCommand
   {
      public string CommandText
      {
         get;
      }

      public string Description
      {
         get;
      }

      public ImageSource Icon
      {
         get;
         protected set;
      }

      protected LaunchableCommandBase( string commandText, string description )
      {
         CommandText = commandText;
         Description = description;

         Icon = new BitmapImage( new Uri( "/Resources/app.png", UriKind.Relative ) );
      }

      protected ImageSource GetIconFromFile( string path )
      {
         var icon = Ico.ExtractAssociatedIcon( path );
         return Imaging.CreateBitmapSourceFromHIcon( icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions() );
      }

      public abstract void Launch( object[] parameters );
   }
}
