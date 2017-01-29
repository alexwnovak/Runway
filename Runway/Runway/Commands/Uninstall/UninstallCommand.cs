using System;

namespace Runway.Commands.Uninstall
{
   public class UninstallCommand : ILaunchableCommand
   {
      public string CommandText => "uninstall";

      public void Launch( object[] parameters )
      {
         throw new NotImplementedException();
      }
   }
}
