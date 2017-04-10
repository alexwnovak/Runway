namespace Runway.ExtensibilityModel
{
   public interface IMatchResult
   {
      MatchType MatchType
      {
         get;
      }

      string DisplayText
      {
         get;
      }

      ILaunchableCommand Command
      {
         get;
      }

      void Activate( object[] parameters );
   }
}
