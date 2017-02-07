namespace Runway
{
   public class MatchResult : IMatchResult
   {
      public MatchType MatchType
      {
         get;
      }

      public string DisplayText => Command.CommandText;

      public ISearchCatalog Source
      {
         get;
      }

      public ILaunchableCommand Command
      {
         get;
      }

      public MatchResult( MatchType matchType, ILaunchableCommand command, ISearchCatalog source )
      {
         MatchType = matchType;
         Command = command;
         Source = source;
      }

      public void Activate( object[] parameters ) => Command.Launch( parameters );
   }
}
