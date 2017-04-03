using System;
using System.Collections.Generic;

namespace Runway.Input
{
   public class InputController : IInputController
   {
      private readonly Stack<IInputFrame> _inputFrames = new Stack<IInputFrame>();

      private int _startingReadIndex = 0;

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
         char newChar = newText[newText.Length - 1];

         if ( newChar == ' ' )
         {
            var newInputFrame = MatchResults[0].BeginInputFrame();
            _inputFrames.Push( newInputFrame );

            _startingReadIndex = newText.Length;
            return;
         }

         string searchText = newText.Substring( _startingReadIndex );

         var currentInputFrame = _inputFrames.Peek();
         MatchResults = currentInputFrame.Match( searchText );
      }
   }
}
