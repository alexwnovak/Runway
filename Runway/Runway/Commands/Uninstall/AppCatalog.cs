//using System;
//using System.Collections.Generic;

//namespace Runway.Commands.Uninstall
//{
//   public class AppCatalog : IAppCatalog
//   {
//      public static AppEntry[] EmptyResults = new AppEntry[0];

//      private readonly IRegistry _registry;
//      private readonly IProcess _process;

//      public AppCatalog( IRegistry registry, IProcess process )
//      {
//         _registry = registry;
//         _process = process;
//      }

//      public AppEntry[] Find( string name )
//      {
//         const string uninstallKeyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

//         var appEntries = new List<AppEntry>();
//         var subKeys = _registry.GetSubKeysFromLocalMachine( uninstallKeyName );

//         foreach ( string subKey in subKeys )
//         {
//            string displayName = _registry.GetValueFromLocalMachine( subKey, "DisplayName" );

//            if ( !string.IsNullOrEmpty( displayName ) && displayName.StartsWith( name, StringComparison.InvariantCultureIgnoreCase ) )
//            {
//               var appEntry = new AppEntry( displayName, subKey );

//               appEntries.Add( appEntry );
//            }
//         }
         
//         return appEntries.ToArray();
//      }
      
//      public void Uninstall( string path )
//      {
//         string uninstallString = _registry.GetValueFromLocalMachine( path, "UninstallString" );

//         var parsedUninstallString = UninstallStringParser.Parse( uninstallString );

//         _process.Start( parsedUninstallString.Item1, parsedUninstallString.Item2 );
//      }
//   }
//}
