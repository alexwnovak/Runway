using System;

namespace Runway.ViewModels
{
   public class CommandParser
   {
      private readonly ICommandCatalog _commandCatalog;

      public CommandParser( ICommandCatalog commandCatalog )
      {
         _commandCatalog = commandCatalog;
      }

      public string GetCommandSuggestion( string partialCommandText )
      {
         var command = _commandCatalog.Resolve( partialCommandText );

         if ( command == null )
         {
            return null;
         }

         int commonIndex = command.CommandText.IndexOf( partialCommandText, StringComparison.InvariantCultureIgnoreCase );
         int postCommonIndex = commonIndex + partialCommandText.Length;

         return command.CommandText.Substring( postCommonIndex );
      }
   }
}
