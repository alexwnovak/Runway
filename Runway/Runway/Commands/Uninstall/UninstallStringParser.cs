using System;

namespace Runway.Commands.Uninstall
{
   public static class UninstallStringParser
   {
      public static Tuple<string, string> Parse( string wholeLine )
      {
         const string exeSearchPattern = ".exe";
         int exeIndex = wholeLine.IndexOf( exeSearchPattern, StringComparison.InvariantCultureIgnoreCase ) + exeSearchPattern.Length;

         int commandStartIndex = 0;
         bool hasQuotes = false;

         if ( wholeLine[0] == '"' )
         {
            commandStartIndex = 1;
            hasQuotes = true;
         }

         int commandEndIndex = exeIndex;

         while ( commandEndIndex < wholeLine.Length && wholeLine[commandEndIndex] != '"' && wholeLine[commandEndIndex] != ' ' )
         {
            commandEndIndex++;
         }

         string command = wholeLine.Substring( commandStartIndex, commandEndIndex - commandStartIndex );

         if ( hasQuotes )
         {
            exeIndex++;
         }

         if ( exeIndex < wholeLine.Length - 1 )
         {
            exeIndex++;
         }

         string argument = wholeLine.Substring( exeIndex );

         return new Tuple<string, string>( command, argument );
      }
   }
}
