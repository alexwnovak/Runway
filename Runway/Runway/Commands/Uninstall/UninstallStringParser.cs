using System;

namespace Runway.Commands.Uninstall
{
   public static class UninstallStringParser
   {
      public static Tuple<string, string> Parse( string wholeLine )
      {
         return new Tuple<string, string>( wholeLine, string.Empty );
      }
   }
}
