using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using Runway.Extensions;
using Runway.ViewModels;

namespace Runway.Views
{
   public partial class MainWindow : Window
   {
      private readonly MainViewModel _viewModel;
      private bool _firstLaunch = true;

      

      public MainWindow()
      {
         InitializeComponent();

         _viewModel = (MainViewModel) DataContext;
         _viewModel.MoveCaretRequested += OnMoveCaretRequested;
         _viewModel.DismissRequested += OnDismissRequested;
         _viewModel.ChangeTextRequested += OnChangeTextRequested;
         _viewModel.Suggestions.CollectionChanged += OnSuggestionsChanged;
      }

      private void OnMoveCaretRequested( object sender, MoveCaretEventArgs e )
         => InputTextBox.MoveCaret( e.CaretPosition );

      private void MainWindow_OnActivated( object sender, EventArgs e )
      {
         //if ( _firstLaunch )
         //{
         //   _firstLaunch = false;
         //   return;
         //}

         //Top = -ActualHeight;
         //Opacity = 0;
      }

      private void MainWindow_OnDeactivated( object sender, EventArgs e )
         => _viewModel.DismissCommand.Execute( null );

      private void OnDismissRequested( object sender, EventArgs e )
      {
         this.SlideOut();
         //Hide();
         //InputTextBox.Text = "";
      }
         //this.SlideOut();
         //=> this.FadeOut();

      private void OnChangeTextRequested( object sender, ChangeTextRequestedEventArgs e )
         => InputTextBox.Text = e.Text;

      private void OnSuggestionsChanged( object sender, NotifyCollectionChangedEventArgs e )
      {
         
      }
         //=> Height = _viewModel.Suggestions.Count == 0 ? 80 : 500;

      private void MainWindow_OnPreviewKeyDown( object sender, KeyEventArgs e )
      {
         if ( e.Key == Key.Down )
         {
            _viewModel.SelectNextSuggestionCommand.Execute( null );
            e.Handled = true;
         }
         else if ( e.Key == Key.Up )
         {
            _viewModel.SelectPreviousSuggestionCommand.Execute( null );
            e.Handled = true;
         }
      }
   }
}
