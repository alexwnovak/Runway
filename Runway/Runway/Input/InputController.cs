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
         get
         {
            return _inputText;
         }
         set
         {
            _inputText = value;
            UpdateMatches( value );
         }
      }

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

      private void UpdateMatches( string newText )
      {
         var currentInputFrame = _inputFrames.Peek();
         MatchResults = currentInputFrame.Match( newText );
      }
   }
}
