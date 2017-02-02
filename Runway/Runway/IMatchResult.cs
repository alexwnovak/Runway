namespace Runway
{
   public interface IMatchResult
   {
      MatchType MatchType
      {
         get;
      }

      ILaunchableCommand Command
      {
         get;
      }
   }
}
