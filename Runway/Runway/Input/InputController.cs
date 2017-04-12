using System;
using System.Collections.Generic;
using System.Linq;
using Runway.ExtensibilityModel;

namespace Runway.Input
{
   public class InputController : IInputController
   {
      private readonly Stack<IInputFrame> _inputFrames = new Stack<IInputFrame>();
      public IInputFrame CurrentInputFrame => _inputFrames.Peek();

      private IMatchResult[] _matchResults;
      private string _inputText;
      private int _readIndex;

      public InputController( IInputFrame initialInputFrame )
      {
         if ( initialInputFrame == null )
         {
            throw new ArgumentNullException( nameof( initialInputFrame ) );
         }

         _inputFrames.Push( initialInputFrame );
      }

      public IMatchResult[] UpdateInputText( string inputText )
      {
         if ( inputText.Last() == ' ' )
         {
            if ( _matchResults[0].Command is IQueryableCommand queryable )
            {
               var searchCatalog = queryable.Query();
               var nextInputFrame = new InputFrame( searchCatalog );
               _inputFrames.Push( nextInputFrame );

               _readIndex = _inputText.Length + inputText.Length;
            }
         }

         _inputText += inputText;

         string frameText = GetCurrentText();
         _matchResults = CurrentInputFrame.Match( frameText );

         return _matchResults;
      }

      private string GetCurrentText()
      {
         return _inputText.Substring( _readIndex );
      }
   }
}
