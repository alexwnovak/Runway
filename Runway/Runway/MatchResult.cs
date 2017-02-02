namespace Runway
{
   public class MatchResult : IMatchResult
   {
      public MatchType MatchType
      {
         get;
      }

      public ILaunchableCommand Command
      {
         get;
      }

      public MatchResult( MatchType matchType, ILaunchableCommand command )
      {
         MatchType = matchType;
         Command = command;
      }
   }
}
