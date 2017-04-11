using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Runway.Input;
using Runway.Services;

namespace Runway.ViewModels
{
   public class MainViewModel : ViewModelBase
   {
      private readonly IInputController _inputController;
      private bool _isUpdatingInput;

      public BulkObservableCollection<IMatchResult> Suggestions
      {
         get;
      } = new BulkObservableCollection<IMatchResult>();

      public string InputText
      {
         get
         {
            return _inputController.InputText;
         }
         set
         {
            _inputController.InputText = value;
            RaisePropertyChanged();

            Suggestions.Reset( _inputController.MatchResults );

            if ( _inputController.MatchResults.Length == 0 )
            {
               _selectedIndex = -1;
               SelectedSuggestion = null;
            }
            else
            {
               _selectedIndex = 0;
               SelectedSuggestion = Suggestions[0];
            }
         }
      }

      public string PreviewCommandText => SelectedSuggestion?.DisplayText;

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

      public ICommand InputTextChangedCommand
      {
         get;
      }

      public event EventHandler<MoveCaretEventArgs> MoveCaretRequested;
      public event EventHandler<ChangeTextRequestedEventArgs> ChangeTextRequested;
      public event EventHandler DismissRequested;

      public MainViewModel( IAppService appService, IInputController inputController )
      {
         _inputController = inputController;

         SelectNextSuggestionCommand = new RelayCommand( OnSelectNextSuggestionCommand );
         SelectPreviousSuggestionCommand = new RelayCommand( OnSelectPreviousSuggestionCommand );
         CompleteSuggestionCommand = new RelayCommand( OnCompleteSuggestionCommand );
         LaunchCommand = new RelayCommand( OnLaunchCommand );
         ExitCommand = new RelayCommand( appService.Exit );
         DismissCommand = new RelayCommand( OnDismissCommand );
         InputTextChangedCommand = new RelayCommand<string>( OnInputTextChanged, OnInputTextChangedCanExecute );
      }

      protected virtual void OnMoveCaretRequested( object sender, MoveCaretEventArgs e )
         => MoveCaretRequested?.Invoke( sender, e );

      protected virtual void OnDismissRequested( object sender, EventArgs e )
         => DismissRequested?.Invoke( sender, e );

      protected virtual void OnChangeTextRequested( object sender, ChangeTextRequestedEventArgs e )
         => ChangeTextRequested?.Invoke( sender, e );

      private void OnSelectNextSuggestionCommand()
      {
         if ( _selectedIndex + 1 >= Suggestions.Count )
         {
            _selectedIndex = 0;
         }
         else
         {
            _selectedIndex++;
         }

         SelectedSuggestion = Suggestions[_selectedIndex];

         RaisePropertyChanged( () => InputText );
         RaisePropertyChanged( () => PreviewCommandText );

         OnMoveCaretRequested( this, new MoveCaretEventArgs( CaretPosition.End ) );
      }

      private void OnSelectPreviousSuggestionCommand()
      {
         if ( _selectedIndex - 1 < 0 )
         {
            _selectedIndex = Suggestions.Count - 1;
         }
         else
         {
            _selectedIndex--;
         }

         SelectedSuggestion = Suggestions[_selectedIndex];

         RaisePropertyChanged( () => InputText );
         RaisePropertyChanged( () => PreviewCommandText );

         OnMoveCaretRequested( this, new MoveCaretEventArgs( CaretPosition.End ) );
      }

      private void OnCompleteSuggestionCommand()
      {
         if ( SelectedSuggestion == null )
         {
            return;
         }

         InputText = SelectedSuggestion.DisplayText;
         OnMoveCaretRequested( this, new MoveCaretEventArgs( CaretPosition.End ) );
      }

      private void OnLaunchCommand()
      {
         if ( SelectedSuggestion == null )
         {
            return;
         }

         SelectedSuggestion.Activate( null );

         OnDismissRequested( this, EventArgs.Empty );
      }

      private void OnDismissCommand()
      {
         InputText = null;
         OnDismissRequested( this, EventArgs.Empty );
      }

      private void OnInputTextChanged( string text )
      {
      }

      private bool OnInputTextChangedCanExecute( string text ) => !_isUpdatingInput;
   }
}
