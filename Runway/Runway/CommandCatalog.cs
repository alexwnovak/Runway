using System;
using System.Linq;

namespace Runway
{
   public class CommandCatalog : ICommandCatalog
   {
      private static readonly string[] _commandList = CreateCommandList();

      private static string[] CreateCommandList()
      {
         return new[]
         {
            "copy",
            "uninstall"
         };
      }

      public string Resolve( string commandPartialText )
      {
         if ( string.IsNullOrEmpty( commandPartialText ) )
         {
            return null;
         }

         return _commandList.FirstOrDefault( c => c.StartsWith( commandPartialText, StringComparison.InvariantCultureIgnoreCase ) );
      }
   }
}
