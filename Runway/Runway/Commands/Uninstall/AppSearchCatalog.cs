using System;
using System.Collections.Generic;

namespace Runway.Commands.Uninstall
{
   public class AppSearchCatalog : IAppSearchCatalog
   {
      private readonly IRegistry _registry;
      private readonly IProcess _process;

      public AppSearchCatalog( IRegistry registry, IProcess process )
      {
         _registry = registry;
         _process = process;
      }

      public IMatchResult[] Search( string searchText )
      {
         const string uninstallKeyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

         var matchResults = new List<AppMatchResult>();
         var subKeys = _registry.GetSubKeysFromLocalMachine( uninstallKeyName );

         foreach ( string subKey in subKeys )
         {
            string displayName = _registry.GetValueFromLocalMachine( subKey, "DisplayName" );

            if ( !string.IsNullOrEmpty( displayName ) && displayName.StartsWith( searchText, StringComparison.InvariantCultureIgnoreCase ) )
            {
               var appEntry = new AppMatchResult( displayName, subKey );

               matchResults.Add( appEntry );
            }
         }

         return matchResults.ToArray();
      }

      public void Uninstall( string path )
      {
         string uninstallString = _registry.GetValueFromLocalMachine( path, "UninstallString" );

         var parsedUninstallString = UninstallStringParser.Parse( uninstallString );

         _process.Start( parsedUninstallString.Item1, parsedUninstallString.Item2 );
      }
   }
}
