using Runway.ExtensibilityModel;

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

      void UpdateInputText( string newText );
   }
}
