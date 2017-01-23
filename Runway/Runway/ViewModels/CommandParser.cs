using System;

namespace Runway.ViewModels
{
   public static class CommandParser
   {
      public static string GetCommandSuggestion( string partialCommandText, string commandText )
      {
         int commonIndex = commandText.IndexOf( partialCommandText, StringComparison.InvariantCultureIgnoreCase );

         int postCommonIndex = commonIndex + partialCommandText.Length;

         return commandText.Substring( postCommonIndex );
      }

      public static string ParseArguments( string fullCommandText )
      {
         int firstSpace = fullCommandText.TrimStart().TrimEnd().IndexOf( ' ' );

         if ( firstSpace == -1 )
         {
            return null;
         }

         return fullCommandText.Substring( firstSpace + 1 );
      }
   }
}
