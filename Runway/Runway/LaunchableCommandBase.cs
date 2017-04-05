using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

      public abstract void Launch( object[] parameters );
   }
}
