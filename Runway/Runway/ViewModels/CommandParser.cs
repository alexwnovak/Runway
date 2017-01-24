using System;

namespace Runway.ViewModels
{
   public static class CommandParser
   {
      public static string GetCommandSuggestion( string partialCommandText, string commandText )
      {
         if ( string.IsNullOrEmpty( commandText ) )
         {
            return null;
         }

         int commonIndex = commandText.IndexOf( partialCommandText, StringComparison.InvariantCultureIgnoreCase );

         int postCommonIndex = commonIndex + partialCommandText.Length;

         return commandText.Substring( postCommonIndex );
      }

      public static string ParseCommand( string fullCommandText )
      {
         if ( string.IsNullOrEmpty( fullCommandText ) )
         {
            return null;
         }

         int firstSpace = fullCommandText.IndexOf( ' ' );

         if ( firstSpace == -1 )
         {
            return fullCommandText;
         }

         return fullCommandText.Substring( 0, firstSpace );
      }

      public static string ParseArguments( string fullCommandText )
      {
         if ( string.IsNullOrEmpty( fullCommandText ) )
         {
            return null;
         }

         int firstSpace = fullCommandText.TrimStart().TrimEnd().IndexOf( ' ' );

         if ( firstSpace == -1 )
         {
            return null;
         }

         return fullCommandText.Substring( firstSpace + 1 );
      }
   }
}
