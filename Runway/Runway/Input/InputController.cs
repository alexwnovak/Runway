using System;
using System.Collections.Generic;

namespace Runway.Input
{
   public class InputController : IInputController
   {
      private readonly Stack<IInputFrame> _inputFrames = new Stack<IInputFrame>();
      public IInputFrame CurrentInputFrame => _inputFrames.Peek();

      public InputController( IInputFrame initialInputFrame )
      {
         if ( initialInputFrame == null )
         {
            throw new ArgumentNullException( nameof( initialInputFrame ) );
         }

         _inputFrames.Push( initialInputFrame );
      }

      public IMatchResult[] UpdateInputText( string inputText ) => CurrentInputFrame.Match( inputText );
   }
}
