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
   }
}
