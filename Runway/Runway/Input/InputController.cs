using System;
using System.Collections.Generic;

namespace Runway.Input
{
   public class InputController : IInputController
   {
      private readonly Stack<IInputFrame> _inputFrames = new Stack<IInputFrame>();

      private string _inputText;
      public string InputText
      {
         get => _inputText;
         set
         {
            _inputText = value;
            MatchResults = CurrentInputFrame.Match( value );
         }
      }

      public IInputFrame CurrentInputFrame => _inputFrames.Peek();

      public IMatchResult[] MatchResults
      {
         get;
         private set;
      }

      public InputController( IInputFrame initialInputFrame )
      {
         if ( initialInputFrame == null )
         {
            throw new ArgumentNullException( nameof( initialInputFrame ) );
         }

         _inputFrames.Push( initialInputFrame );
      }
   }
}
