using System;

namespace Runway.Commands.Uninstall
{
   public class UninstallCommand : LaunchableCommandBase
   {
      private readonly IAppCatalog _appCatalog;

      public UninstallCommand( IAppCatalog appCatalog )
         : base( "uninstall", "Removes installed applications" )
      {
         _appCatalog = appCatalog;
         Icon = GetIconFromFile( @"C:\Windows\System32\appwiz.cpl" );
      }

      public override void Launch( object[] parameters )
      {
         throw new NotImplementedException();
      }
   }
}
