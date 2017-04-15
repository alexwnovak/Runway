using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Runway.ExtensibilityModel;

namespace Runway.Input
{
   public class InputController : IInputController
   {
      private readonly Stack<IInputFrame> _inputFrames = new Stack<IInputFrame>();
      public IInputFrame CurrentInputFrame => _inputFrames.Peek();

      private IMatchResult[] _matchResults;
      private string _inputText = string.Empty;
      private int _readIndex;

      public InputController( IInputFrame initialInputFrame )
      {
         if ( initialInputFrame == null )
         {
            throw new ArgumentNullException( nameof( initialInputFrame ) );
         }

         _inputFrames.Push( initialInputFrame );
      }

      public async Task<IMatchResult[]> UpdateInputText( string inputText )
      {
         if ( inputText.Length > _inputText.Length )
         {
            if ( inputText.Last() == ' ' )
            {
               if ( _matchResults[0].Command is IQueryableCommand queryable )
               {
                  var searchCatalog = queryable.Query();
                  var nextInputFrame = new InputFrame( searchCatalog );
                  _inputFrames.Push( nextInputFrame );

                  _readIndex = inputText.Length;
               }
            }
         }
         else if ( inputText.Length < _inputText.Length )
         {
            if ( _inputText.Last() == ' ' )
            {
               _inputFrames.Pop();
               _readIndex = 0;
            }
         }

         _inputText = inputText;

         string frameText = GetCurrentText();
         _matchResults = await CurrentInputFrame.Match( frameText );

         return _matchResults;
      }

      private string GetCurrentText()
      {
         return _inputText.Substring( _readIndex );
      }
   }
}
