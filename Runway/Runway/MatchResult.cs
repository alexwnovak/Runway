namespace Runway
{
   public class MatchResult
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
