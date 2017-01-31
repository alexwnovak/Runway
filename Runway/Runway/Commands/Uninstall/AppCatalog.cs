using System.Collections.Generic;

namespace Runway.Commands.Uninstall
{
   public class AppCatalog : IAppCatalog
   {
      public static AppEntry[] EmptyResults = new AppEntry[0];

      private readonly IRegistry _registry;

      public AppCatalog( IRegistry registry )
      {
         _registry = registry;
      }

      public AppEntry[] Find( string name )
      {
         const string uninstallKeyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

         var appEntries = new List<AppEntry>();
         var subKeys = _registry.GetSubKeysFromLocalMachine( uninstallKeyName );

         foreach ( string subKey in subKeys )
         {
            string displayName = _registry.GetValueFromLocalMachine( subKey, "DisplayName" );

            if ( displayName == name )
            {
               var appEntry = new AppEntry( displayName, $@"{uninstallKeyName}\{subKey}" );

               appEntries.Add( appEntry );
            }
         }
         
         return appEntries.ToArray();
      }
   }
}
