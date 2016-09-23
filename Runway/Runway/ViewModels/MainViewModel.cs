using System;
using GalaSoft.MvvmLight;

namespace Runway.ViewModels
{
   public class MainViewModel : ViewModelBase
   {
      private readonly ICommandCatalog _commandCatalog;

      private string _currentCommandText;
      public string CurrentCommandText
      {
         get
         {
            return _currentCommandText;
         }
         set
         {
            bool changed = Set( () => CurrentCommandText, ref _currentCommandText, value );

            if ( changed )
            {
               CommandTextChanged( value );
            }
         }
      }

      private string _previewCommandText;
      public string PreviewCommandText
      {
         get
         {
            return _previewCommandText;
         }
         set
         {
            Set( () => PreviewCommandText, ref _previewCommandText, value );
         }
      }

      public MainViewModel( ICommandCatalog commandCatalog )
      {
         _commandCatalog = commandCatalog;
      }

      private void CommandTextChanged( string newText )
      {
         string commandSuggestion = _commandCatalog.Resolve( newText );

         if ( string.IsNullOrEmpty( commandSuggestion ) )
         {
            PreviewCommandText = null;
            return;
         }

         int commonIndex = commandSuggestion.IndexOf( newText, StringComparison.InvariantCultureIgnoreCase );
         int postCommonIndex = commonIndex + newText.Length;

         PreviewCommandText = commandSuggestion.Substring( postCommonIndex );
      }
   }
}
