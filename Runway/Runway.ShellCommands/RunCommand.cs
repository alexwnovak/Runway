using Runway.ExtensibilityModel;
using Shell32;

namespace Runway.ShellCommands
{
   public class RunCommand : ILaunchableCommand
   {
      public string CommandText => "run";
      public string Description => "Launches the Windows Run dialog"; 

      public void Launch( object[] parameters )
      {
         var shell = new Shell();
         shell.FileRun();
      }
   }
}
