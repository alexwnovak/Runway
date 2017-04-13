using System.Diagnostics;

namespace Runway.Commands.Restart
{
   public class RestartCommand : LaunchableCommandBase
   {
      public RestartCommand()
         : base( "restart", "Restarts the computer" )
      {
      }

      public override void Launch()
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
