using System;
using System.Collections.Generic;
using System.Linq;

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

         var appEntries = new List<AppEntry>();
         var subKeys = _registry.GetSubKeysFromLocalMachine( uninstallKeyName );

         foreach ( string subKey in subKeys )
         {
            string displayName = _registry.GetValueFromLocalMachine( subKey, "DisplayName" );

            if ( !string.IsNullOrEmpty( displayName ) && displayName.StartsWith( searchText, StringComparison.InvariantCultureIgnoreCase ) )
            {
               var appEntry = new AppEntry( displayName, subKey );

               appEntries.Add( appEntry );
            }
         }

         return appEntries.Select( ae => new AppMatchResult( ae.Name ) ).ToArray();
      }

      public void Uninstall( string path )
      {
         string uninstallString = _registry.GetValueFromLocalMachine( path, "UninstallString" );

         var parsedUninstallString = UninstallStringParser.Parse( uninstallString );

         _process.Start( parsedUninstallString.Item1, parsedUninstallString.Item2 );
      }
   }
}
