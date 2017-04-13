using Shell32;
using Runway.ExtensibilityModel;

namespace Runway.ShellCommands
{
   public class RunCommand : ILaunchableCommand
   {
      public string CommandText => "run";
      public string Description => "Launches the Windows Run dialog"; 

      public void Launch()
      {
         var shell = new Shell();
         shell.FileRun();
      }
   }
}
