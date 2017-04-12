using Runway.ExtensibilityModel;

namespace Runway.Input
{
   public interface IInputController
   {
      IInputFrame CurrentInputFrame
      {
         get;
      }

      IMatchResult[] UpdateInputText( string inputText );
   }
}
