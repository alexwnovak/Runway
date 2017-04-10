using System;
using System.Collections.Generic;
using System.Linq;
using Runway.ExtensibilityModel;

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

      public void UpdateInputText( string newText )
      {
         if ( string.IsNullOrEmpty( newText ) )
         {
            MatchResults = new IMatchResult[0];
            return;
         }

         int oldLength = string.IsNullOrEmpty( InputText ) ? 0 : InputText.Length;
         int newLength = newText.Length;

         if ( oldLength < newLength )
         {
            AddText( newText );
         }
         else if ( oldLength > newLength )
         {
            char removedChar = InputText.Last();

            if ( removedChar == ' ' )
            {
               _inputFrames.Pop();
            }
         }

         InputText = newText;
      }

      private void AddText( string newText )
      {
         char newestChar = newText.Last();

         if ( newestChar == ' ' )
         {
            var topMatch = MatchResults[0];

            if ( topMatch.Command is IQueryableCommand )
            {
               var queryable = (IQueryableCommand) topMatch.Command;

               var nextSearchCatalog = queryable.QueryResults();
               var inputFrame = new InputFrame( nextSearchCatalog );

               _inputFrames.Push( inputFrame );

               MatchResults = CurrentInputFrame.Match( "" );
            }
         }
         else
         {
            //MatchResults = CurrentInputFrame.Match( newText );
         }
      }
   }
}
