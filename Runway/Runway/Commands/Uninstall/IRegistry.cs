namespace Runway.Commands.Uninstall
{
   public interface IRegistry
   {
      string[] GetSubKeysFromLocalMachine( string path );

      string GetValueFromLocalMachine( string path, string key );
   }
}
