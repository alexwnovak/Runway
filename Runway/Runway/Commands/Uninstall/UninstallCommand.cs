using System;

namespace Runway.Commands.Uninstall
{
   public class UninstallCommand : LaunchableCommandBase
   {
      public UninstallCommand( IAppSearchCatalog appCatalog ) : base( "uninstall", appCatalog )
      {
      }

      public override void Launch( object[] parameters )
      {
         throw new NotImplementedException();
      }
   }
}
