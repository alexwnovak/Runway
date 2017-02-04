namespace Runway.Input
{
   public interface IInputController
   {
      string InputText
      {
         get;
         set;
      }

      IInputFrame CurrentInputFrame
      {
         get;
      }

      IMatchResult[] MatchResults
      {
         get;
      }
   }
}
