using System;

namespace Runway.Commands.Uninstall
{
   public static class UninstallStringParser
   {
      public static Tuple<string, string> Parse( string wholeLine )
      {
         if ( wholeLine.StartsWith( "\"" ) )
         {
            int nextQuoteIndex = wholeLine.IndexOf( '"', 1 );
            wholeLine = wholeLine.Substring( 1, nextQuoteIndex - 1 );
         }

         return new Tuple<string, string>( wholeLine, string.Empty );
      }
   }
}
