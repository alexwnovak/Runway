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

      public BulkObservableCollection<IMatchResult> Suggestions
      {
         get;
      } = new BulkObservableCollection<IMatchResult>();

      private string _inputText;
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

      public MainViewModel( IAppService appService, IInputController inputController )
      {
         _inputController = inputController;

         SelectNextSuggestionCommand = new RelayCommand( OnSelectNextSuggestionCommand );
         SelectPreviousSuggestionCommand = new RelayCommand( OnSelectPreviousSuggestionCommand );
         CompleteSuggestionCommand = new RelayCommand( OnCompleteSuggestionCommand );
         SpacePressedCommand = new RelayCommand( OnSpacePressedCommand );
         LaunchCommand = new RelayCommand( OnLaunchCommand );
         ExitCommand = new RelayCommand( appService.Exit );
      }

      private void UpdateCommandText( string newText )
      {
         //CurrentMatchResults = _commandCatalog.Search( newText );

         //Suggestions.Reset( CurrentMatchResults );

         //if ( CurrentMatchResults.Length > 0 )
         //{
         //   _selectedIndex = 0;
         //   SelectedSuggestion = CurrentMatchResults[_selectedIndex];
         //}

         RaisePropertyChanged( () => PreviewCommandText );
      }

      protected virtual void OnMoveCaretRequested( object sender, MoveCaretEventArgs e )
         => MoveCaretRequested?.Invoke( sender, e );

      protected virtual void OnDismissRequested( object sender, EventArgs e )
         => DismissRequested?.Invoke( sender, e );

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

         _inputText = SelectedSuggestion.DisplayText;

         RaisePropertyChanged( () => InputText );
         RaisePropertyChanged( () => PreviewCommandText );

         OnMoveCaretRequested( this, new MoveCaretEventArgs( CaretPosition.End ) );
      }

      private void OnSelectPreviousSuggestionCommand()
      {
         if ( _selectedIndex - 1 < 0 )
         {
            //_selectedIndex = CurrentMatchResults.Length - 1;
         }
         else
         {
            _selectedIndex--;
         }

         //SelectedSuggestion = CurrentMatchResults[_selectedIndex];

         _inputText = SelectedSuggestion.DisplayText;

         RaisePropertyChanged( () => InputText );
         RaisePropertyChanged( () => PreviewCommandText );

         OnMoveCaretRequested( this, new MoveCaretEventArgs( CaretPosition.End ) );
      }

      private void OnCompleteSuggestionCommand()
      {
         if ( string.IsNullOrEmpty( PreviewCommandText ) )
         {
            return;
         }

         _inputText = PreviewCommandText;

         RaisePropertyChanged( () => InputText );
         RaisePropertyChanged( () => PreviewCommandText );

         OnMoveCaretRequested( this, new MoveCaretEventArgs( CaretPosition.End ) );
      }

      private void OnSpacePressedCommand()
      {
         CompleteSuggestionCommand.Execute( null );

         InputText += " ";
      }

      private void OnLaunchCommand()
      {
         if ( SelectedSuggestion == null )
         {
            return;
         }

         string commandText = CommandParser.ParseCommand( InputText );
         string argumentString = CommandParser.ParseArguments( InputText );

         //var results = _commandCatalog.Search( commandText );

         //if ( results.Length > 0 )
         //{
         //   SelectedSuggestion.Activate( new object[] { argumentString } );
         //   OnDismissRequested( this, EventArgs.Empty );
         //}
      }
   }
}
