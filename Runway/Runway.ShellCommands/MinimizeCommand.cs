using Shell32;
using Runway.ExtensibilityModel;

namespace Runway.ShellCommands
{
   public class MinimizeCommand : ILaunchableCommand
   {
      public string CommandText => "minimize";
      public string Description => "Minimizes all windows";

      public void Launch( object[] parameters )
      {
         var shell = new Shell();
         shell.MinimizeAll();
      }
   }
}
