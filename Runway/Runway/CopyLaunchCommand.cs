using System.Windows;

namespace Runway
{
   public class CopyLaunchCommand : LaunchableCommandBase
   {
      public CopyLaunchCommand() : base( "copy" )
      {
      }

      public override void Launch( object[] parameters )
      {
         if ( parameters == null || parameters.Length == 0 )
         {
            return;
         }

         string argument = parameters[0].ToString();
         Clipboard.SetText( argument );
      }
   }
}
