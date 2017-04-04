using System.Windows;

namespace Runway
{
   public class CopyLaunchCommand : LaunchableCommandBase
   {
      public CopyLaunchCommand()
         : base( "copy", "Copies text to the clipboard" )
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
