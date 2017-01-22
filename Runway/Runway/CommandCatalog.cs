using System;
using System.Linq;

namespace Runway
{
   public class CommandCatalog : ICommandCatalog
   {
      private static readonly ILaunchableCommand[] _commandList = CreateCommandList();

      private static ILaunchableCommand[] CreateCommandList()
      {
         return new ILaunchableCommand[]
         {
            new CopyLaunchCommand(),
         };
      }

      private class NullCommand : ILaunchableCommand
      {
         public string CommandText => null;

         public void Launch( object[] parameters )
         {
         }
      }

      public static readonly ILaunchableCommand MissingCommand = new NullCommand();

      public ILaunchableCommand Resolve( string commandPartialText )
      {
         if ( string.IsNullOrEmpty( commandPartialText ) )
         {
            return MissingCommand;
         }

         return _commandList.FirstOrDefault( c => c.CommandText.StartsWith( commandPartialText, StringComparison.InvariantCultureIgnoreCase ) );
      }
   }
}
