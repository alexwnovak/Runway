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
         return null;
      }
   }
}
