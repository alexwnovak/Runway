using System;

namespace Runway.Commands.Uninstall
{
   public static class UninstallStringParser
   {
      public static Tuple<string, string> Parse( string wholeLine )
      {
         string command;
         int spaceIndent = 0;

         if ( wholeLine.StartsWith( "\"" ) )
         {
            int nextQuoteIndex = wholeLine.IndexOf( '"', 1 );
            command = wholeLine.Substring( 1, nextQuoteIndex - 1 );
            spaceIndent = 2;
         }
         else
         {
            command = wholeLine;
         }

         string argument = wholeLine.Substring( command.Length + spaceIndent ).TrimStart();

         return new Tuple<string, string>( command, argument );
      }
   }
}
