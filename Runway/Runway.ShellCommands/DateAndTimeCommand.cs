using Runway.ExtensibilityModel;
using Shell32;

namespace Runway.ShellCommands
{
   public class DateAndTimeCommand : ILaunchableCommand
   {
      public string CommandText => "dateandtime";
      public string Description => "Opens the Windows Date and Time settings";

      public void Launch( object[] parameters )
      {
         var shell = new Shell();
         shell.ControlPanelItem( "timedate.cpl" );
      }
   }
}
