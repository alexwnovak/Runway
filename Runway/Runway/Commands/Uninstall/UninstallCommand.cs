using System;

namespace Runway.Commands.Uninstall
{
   public class UninstallCommand : LaunchableCommandBase
   {
      private readonly IAppCatalog _appCatalog;

      public UninstallCommand( IAppCatalog appCatalog ) : base( "uninstall", null )
      {
         _appCatalog = appCatalog;
      }

      public override void Launch( object[] parameters )
      {
         throw new NotImplementedException();
      }
   }
}
