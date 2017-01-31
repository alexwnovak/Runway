using System;

namespace Runway.Commands.Uninstall
{
   public interface IRegistry
   {
      string[] GetSubKeysFromLocalMachine( string path );

      Tuple<string, string> GetValuesFromLocalMachine( string path, string key1, string key2 );
   }
}
