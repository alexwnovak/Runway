namespace Runway.Input
{
   public interface IInputController
   {
      string InputText
      {
         get;
         set;
      }

      IMatchResult[] MatchResults
      {
         get;
      }
   }
}
