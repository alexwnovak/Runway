using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

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

      public ICommand CompleteSuggestionCommand
      {
         get;
      }

      public ICommand LaunchCommand
      {
         get;
      }

      public MainViewModel( ICommandCatalog commandCatalog )
      {
         _commandCatalog = commandCatalog;

         CompleteSuggestionCommand = new RelayCommand( OnCompleteSuggestionCommand );
         LaunchCommand = new RelayCommand( OnLaunchCommand );
      }

      private void OnCompleteSuggestionCommand()
      {
         if ( string.IsNullOrEmpty( PreviewCommandText ) )
         {
            return;
         }

         _currentCommandText = CurrentCommandText + PreviewCommandText;
         PreviewCommandText = null;
         RaisePropertyChanged( () => CurrentCommandText );
      }

      private void OnLaunchCommand()
      {
         if ( string.IsNullOrEmpty( CurrentCommandText ) )
         {
            return;
         }

         int firstSpace = CurrentCommandText.IndexOf( ' ' );

         if ( firstSpace == -1 )
         {
            return;
         }

         string commandText = CurrentCommandText.Substring( 0, firstSpace );
         string argumentString = CurrentCommandText.Substring( firstSpace + 1 );

         var launchCommand = _commandCatalog.Resolve( commandText );
         launchCommand.Launch( new object[] { argumentString } );
      }

      private void CommandTextChanged( string newText )
      {
         var commandSuggestion = _commandCatalog.Resolve( newText );

         if ( commandSuggestion == null )
         {
            PreviewCommandText = null;
            return;
         }

         int commonIndex = commandSuggestion.CommandText.IndexOf( newText, StringComparison.InvariantCultureIgnoreCase );
         int postCommonIndex = commonIndex + newText.Length;

         PreviewCommandText = commandSuggestion.CommandText.Substring( postCommonIndex );
      }
   }
}
