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

      public ILaunchableCommand Resolve( string commandPartialText )
      {
         if ( string.IsNullOrEmpty( commandPartialText ) )
         {
            return null;
         }

         return _commandList.FirstOrDefault( c => c.CommandText.StartsWith( commandPartialText, StringComparison.InvariantCultureIgnoreCase ) );
      }
   }
}
