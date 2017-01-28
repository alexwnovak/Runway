using System;
using System.Collections.ObjectModel;
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

      public ObservableCollection<SuggestionViewModel> Suggestions
      {
         get;
      } = new ObservableCollection<SuggestionViewModel>();

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
               CurrentMatchResults = _commandCatalog.Resolve( value );
               RaisePropertyChanged( () => PreviewCommandText );
            }
         }
      }

      public string PreviewCommandText
      {
         get
         {
            if ( CurrentMatchResults.Length > 0 )
            {
               return CommandParser.GetCommandSuggestion( CurrentCommandText, CurrentMatchResults[0].Command.CommandText );
            }

            return null;
         }
      }

      public MatchResult[] CurrentMatchResults
      {
         get;
         private set;
      } = CommandCatalog.EmptySet;

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
      public event EventHandler DismissRequested;

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

      protected virtual void OnDismissRequested( object sender, EventArgs e )
         => DismissRequested?.Invoke( sender, e );

      private void OnCompleteSuggestionCommand()
      {
         _currentCommandText = CurrentCommandText + PreviewCommandText;

         RaisePropertyChanged( () => CurrentCommandText );
         RaisePropertyChanged( () => PreviewCommandText );

         OnMoveCaretRequested( this, new MoveCaretEventArgs( CaretPosition.End ) );
      }

      private void OnLaunchCommand()
      {
         string commandText = CommandParser.ParseCommand( CurrentCommandText );
         string argumentString = CommandParser.ParseArguments( CurrentCommandText );

         var results = _commandCatalog.Resolve( commandText );

         if ( results.Length > 0 )
         {
            results[0].Command.Launch( new object[] { argumentString } );
            OnDismissRequested( this, EventArgs.Empty );
         }
      }
   }
}
