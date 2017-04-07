using System.Linq;
using Runway.ExtensibilityModel;

namespace Runway.UnitTests.Helpers
{
   public static class MatchResultHelper
   {
      public static MatchResult[] Create( MatchType matchType, ILaunchableCommand command )
      {
         return new []
         {
            new MatchResult( matchType, command )
         };
      }

      public static MatchResult[] CreatePartial( params ILaunchableCommand[] commands )
         => commands.Select( c => new MatchResult( MatchType.Partial, c ) ).ToArray();
   }
}
