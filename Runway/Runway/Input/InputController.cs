using System.Collections.Generic;

namespace Runway.Input
{
   public class InputController : IInputController
   {
      private readonly Stack<IInputFrame> _inputFrames = new Stack<IInputFrame>();

      public string InputText
      {
         get;
         set;
      }

      public IInputFrame CurrentInputFrame => _inputFrames.Peek();

      public InputController( IInputFrame initialInputFrame )
      {
         _inputFrames.Push( initialInputFrame );
      }
   }
}
