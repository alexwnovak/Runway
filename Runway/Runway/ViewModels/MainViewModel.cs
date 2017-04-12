using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Runway.ExtensibilityModel;
using Runway.Extensions;
using Runway.Input;
using Runway.Services;

namespace Runway.ViewModels
{
   public class MainViewModel : ViewModelBase
   {
      private readonly IAppService _appService;
      private readonly IInputController _inputController;
      private bool _isUpdatingInput;

      public BulkObservableCollection<IMatchResult> Suggestions
      {
         get;
      } = new BulkObservableCollection<IMatchResult>();

      public string PreviewCommandText => SelectedSuggestion?.DisplayText;

      private int _selectedIndex;
      private IMatchResult _selectedSuggestion;
      public IMatchResult SelectedSuggestion
      {
         get => _selectedSuggestion;
         set
         {
            Set( () => SelectedSuggestion, ref _selectedSuggestion, value );
            RaisePropertyChanged( () => PreviewCommandText );
         }
      }

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

      public ICommand LaunchCommand
      {
         get;
      }

      public ICommand ExitCommand
      {
         get;
      }

      public ICommand DismissCommand
      {
         get;
      }

      public ICommand ChangeInputTextCommand
      {
         get;
      }

      public event EventHandler<MoveCaretEventArgs> MoveCaretRequested;
      public event EventHandler<ChangeTextRequestedEventArgs> ChangeTextRequested;
      public event EventHandler DismissRequested;

      public MainViewModel( IAppService appService, IInputController inputController )
      {
         _appService = appService;
         _inputController = inputController;

         SelectNextSuggestionCommand = new RelayCommand( OnSelectNextSuggestionCommand );
         SelectPreviousSuggestionCommand = new RelayCommand( OnSelectPreviousSuggestionCommand );
         CompleteSuggestionCommand = new RelayCommand( OnCompleteSuggestionCommand, HasSelectedSuggestion );
         LaunchCommand = new RelayCommand( OnLaunchCommand, HasSelectedSuggestion );
         ExitCommand = new RelayCommand( OnExitCommand );
         DismissCommand = new RelayCommand( OnDismissCommand );
         ChangeInputTextCommand = new RelayCommand<string>( OnChangeInputText, ChangeInputTextCanExecute );
      }

      protected virtual void OnMoveCaretRequested( object sender, MoveCaretEventArgs e )
         => MoveCaretRequested?.Invoke( sender, e );

      protected virtual void OnDismissRequested( object sender, EventArgs e )
         => DismissRequested?.Invoke( sender, e );

      protected virtual void OnChangeTextRequested( object sender, ChangeTextRequestedEventArgs e )
         => ChangeTextRequested?.Invoke( sender, e );

      private void OnSelectNextSuggestionCommand()
      {
         _selectedIndex = _selectedIndex.Increment( Suggestions.Count );
         SelectedSuggestion = Suggestions[_selectedIndex];
         OnMoveCaretRequested( this, new MoveCaretEventArgs( CaretPosition.End ) );
      }

      private void OnSelectPreviousSuggestionCommand()
      {
         _selectedIndex = _selectedIndex.Decrement( Suggestions.Count - 1 );
         SelectedSuggestion = Suggestions[_selectedIndex];
         OnMoveCaretRequested( this, new MoveCaretEventArgs( CaretPosition.End ) );
      }

      private void OnCompleteSuggestionCommand()
      {
         OnChangeTextRequested( this, new ChangeTextRequestedEventArgs( SelectedSuggestion.DisplayText ) );
         OnMoveCaretRequested( this, new MoveCaretEventArgs( CaretPosition.End ) );
      }

      private void OnLaunchCommand()
      {
         SelectedSuggestion.Activate( null );
         OnDismissRequested( this, EventArgs.Empty );
      }

      private bool HasSelectedSuggestion() => SelectedSuggestion != null;

      private void OnExitCommand() => _appService.Exit();

      private void OnDismissCommand()
      {
         OnChangeTextRequested( this, new ChangeTextRequestedEventArgs( string.Empty ) );
         OnDismissRequested( this, EventArgs.Empty );
      }

      private void OnChangeInputText( string text )
      {
         _isUpdatingInput = true;
         var matchResults = _inputController.UpdateInputText( text );

         Suggestions.Reset( matchResults );

         if ( matchResults.Length == 0 )
         {
            _selectedIndex = -1;
            SelectedSuggestion = null;
         }
         else
         {
            _selectedIndex = 0;
            SelectedSuggestion = Suggestions[0];
         }

         _isUpdatingInput = false;
      }

      private bool ChangeInputTextCanExecute( string text ) => !_isUpdatingInput;
   }
}
