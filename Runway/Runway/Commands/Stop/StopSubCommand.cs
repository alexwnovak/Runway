using System.Diagnostics;
using Runway.ExtensibilityModel;

namespace Runway.Commands.Stop
{
   public class StopSubCommand : ILaunchableCommand
   {
      public string CommandText
      {
         get;
      }

      public string Description => null;

      private readonly Process _process;

      public StopSubCommand( string commandText, Process process )
      {
         CommandText = commandText;
         _process = process;
      }

      public void Launch()
      {
         _process.Kill();
      }
   }
}
