using Runway.ExtensibilityModel;

namespace Runway
{
   public class MatchResult : IMatchResult
   {
      public MatchType MatchType
      {
         get;
      }

      public string DisplayText => Command.CommandText;

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
