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

      public BulkObservableCollection<IMatchResult> Suggestions
      {
         get;
      } = new BulkObservableCollection<IMatchResult>();

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
               UpdateCommandText( value );
            }
         }
      }

      public string PreviewCommandText
      {
         get
         {
            if ( CurrentMatchResults.Length > 0 )
            {
               return CurrentMatchResults[_selectedIndex].DisplayText;
            }

            return null;
         }
      }

      private int _selectedIndex;
      private IMatchResult _selectedSuggestion;
      public IMatchResult SelectedSuggestion
      {
         get
         {
            return _selectedSuggestion;
         }
         set
         {
            Set( () => SelectedSuggestion, ref _selectedSuggestion, value );
         }
      }

      public IMatchResult[] CurrentMatchResults
      {
         get;
         private set;
      } = CommandCatalog.EmptySet;

      public ICommand SelectNextSuggestionCommand
      {
         get;
      }

      public ICommand SelectPreviousSuggestionCommand
      {
         get;
      }

      public ICommand CompleteSuggestionCommand
      {
         get;
      }

      public ICommand SpacePressedCommand
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

         SelectNextSuggestionCommand = new RelayCommand( OnSelectNextSuggestionCommand );
         SelectPreviousSuggestionCommand = new RelayCommand( OnSelectPreviousSuggestionCommand );
         CompleteSuggestionCommand = new RelayCommand( OnCompleteSuggestionCommand );
         SpacePressedCommand = new RelayCommand( OnSpacePressedCommand );
         LaunchCommand = new RelayCommand( OnLaunchCommand );
         ExitCommand = new RelayCommand( () => _appService.Exit() );
      }

      private void UpdateCommandText( string newText )
      {
         CurrentMatchResults = _commandCatalog.Resolve( newText );

         Suggestions.Reset( CurrentMatchResults );

         if ( CurrentMatchResults.Length > 0 )
         {
            _selectedIndex = 0;
            SelectedSuggestion = CurrentMatchResults[_selectedIndex];
         }

         RaisePropertyChanged( () => PreviewCommandText );
      }

      protected virtual void OnMoveCaretRequested( object sender, MoveCaretEventArgs e )
         => MoveCaretRequested?.Invoke( sender, e );

      protected virtual void OnDismissRequested( object sender, EventArgs e )
         => DismissRequested?.Invoke( sender, e );

      private void OnSelectNextSuggestionCommand()
      {
         if ( _selectedIndex + 1 >= CurrentMatchResults.Length )
         {
            _selectedIndex = 0;
         }
         else
         {
            _selectedIndex++;
         }

         SelectedSuggestion = CurrentMatchResults[_selectedIndex];

         _currentCommandText = SelectedSuggestion.DisplayText;

         RaisePropertyChanged( () => CurrentCommandText );
         RaisePropertyChanged( () => PreviewCommandText );

         OnMoveCaretRequested( this, new MoveCaretEventArgs( CaretPosition.End ) );
      }

      private void OnSelectPreviousSuggestionCommand()
      {
         if ( _selectedIndex - 1 < 0 )
         {
            _selectedIndex = CurrentMatchResults.Length - 1;
         }
         else
         {
            _selectedIndex--;
         }

         SelectedSuggestion = CurrentMatchResults[_selectedIndex];

         _currentCommandText = SelectedSuggestion.DisplayText;

         RaisePropertyChanged( () => CurrentCommandText );
         RaisePropertyChanged( () => PreviewCommandText );

         OnMoveCaretRequested( this, new MoveCaretEventArgs( CaretPosition.End ) );
      }

      private void OnCompleteSuggestionCommand()
      {
         if ( string.IsNullOrEmpty( PreviewCommandText ) )
         {
            return;
         }

         _currentCommandText = PreviewCommandText;

         RaisePropertyChanged( () => CurrentCommandText );
         RaisePropertyChanged( () => PreviewCommandText );

         OnMoveCaretRequested( this, new MoveCaretEventArgs( CaretPosition.End ) );
      }

      private void OnSpacePressedCommand()
      {
         CompleteSuggestionCommand.Execute( null );

         CurrentCommandText += " ";
      }

      private void OnLaunchCommand()
      {
         if ( SelectedSuggestion == null )
         {
            return;
         }

         string commandText = CommandParser.ParseCommand( CurrentCommandText );
         string argumentString = CommandParser.ParseArguments( CurrentCommandText );

         var results = _commandCatalog.Resolve( commandText );

         if ( results.Length > 0 )
         {
            SelectedSuggestion.Activate( new object[] { argumentString } );
            OnDismissRequested( this, EventArgs.Empty );
         }
      }
   }
}
