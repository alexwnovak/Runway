using System.Diagnostics;

namespace Runway.Commands.Restart
{
   public class RestartCommand : LaunchableCommandBase
   {
      public RestartCommand() : base( "restart" )
      {
      }

      public override void Launch( object[] parameters )
      {
         var startInfo = new ProcessStartInfo
         {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = "cmd",
            Arguments = "/C shutdown -r -t 0"
         };

         Process.Start( startInfo );
      }
   }
}
