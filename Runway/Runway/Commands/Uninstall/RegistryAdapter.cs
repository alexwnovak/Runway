using Microsoft.Win32;

namespace Runway.Commands.Uninstall
{
   public class RegistryAdapter : IRegistry
   {
      private const string _uninstallKeyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

      public string[] GetSubKeysFromLocalMachine( string path )
      {
         using ( var registryKey = Registry.LocalMachine.OpenSubKey( _uninstallKeyName ) )
         {
            return registryKey.GetSubKeyNames();
         }
      }

      public string GetValueFromLocalMachine( string path, string key )
      {
         string finalPath = $@"{_uninstallKeyName}\{path}";

         using ( var registryKey = Registry.LocalMachine.OpenSubKey( finalPath ) )
         {
            return registryKey.GetValue( key )?.ToString();
         }
      }
   }
}
