using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Runway.Services;

namespace Runway.ViewModels
{
   public class MainViewModel : ViewModelBase
   {
      private readonly ICommandCatalog _commandCatalog;
      private readonly IAppService _appService;

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
               var matchResults = _commandCatalog.Resolve( value );

               if ( matchResults.Length > 0 )
               {
                  PreviewCommandText = CommandParser.GetCommandSuggestion( value, matchResults[0].Command.CommandText );
               }
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

      public ILaunchableCommand CurrentCommand
      {
         get;
         private set;
      }

      public ICommand CompleteSuggestionCommand
      {
         get;
      }

      public ICommand LaunchCommand
      {
         get;
      }

      public ICommand ExitCommand
      {
         get;
      }

      public event EventHandler<MoveCaretEventArgs> MoveCaretRequested;

      public MainViewModel( ICommandCatalog commandCatalog, IAppService appService )
      {
         _commandCatalog = commandCatalog;
         _appService = appService;

         CompleteSuggestionCommand = new RelayCommand( OnCompleteSuggestionCommand );
         LaunchCommand = new RelayCommand( OnLaunchCommand );
         ExitCommand = new RelayCommand( () => _appService.Exit() );
      }

      protected virtual void OnMoveCaretRequested( object sender, MoveCaretEventArgs e )
         => MoveCaretRequested?.Invoke( sender, e );

      private void OnCompleteSuggestionCommand()
      {
         _currentCommandText = CurrentCommandText + PreviewCommandText;
         PreviewCommandText = null;

         RaisePropertyChanged( () => CurrentCommandText );
         OnMoveCaretRequested( this, new MoveCaretEventArgs( CaretPosition.End ) );
      }

      private void OnLaunchCommand()
      {
         string commandText = CommandParser.ParseCommand( CurrentCommandText );
         string argumentString = CommandParser.ParseArguments( CurrentCommandText );

         var results = _commandCatalog.Resolve( commandText );
         results[0].Command.Launch( new object[] { argumentString } );
      }
   }
}
