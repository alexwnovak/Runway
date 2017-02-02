using System;

namespace Runway.Commands.Uninstall
{
   public class UninstallCommand : LaunchableCommandBase
   {
      public UninstallCommand() : base( "uninstall" )
      {
      }

      public override void Launch( object[] parameters )
      {
         throw new NotImplementedException();
      }
   }
}
