using System;

namespace Runway.ViewModels
{
   public static class CommandParser
   {
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
            return string.Empty;
         }

         int firstSpace = fullCommandText.TrimStart().TrimEnd().IndexOf( ' ' );

         if ( firstSpace == -1 )
         {
            return string.Empty;
         }

         return fullCommandText.Substring( firstSpace + 1 );
      }
   }
}
