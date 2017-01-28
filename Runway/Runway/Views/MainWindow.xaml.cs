using System;
using System.Collections.Specialized;
using System.Windows;
using Runway.ViewModels;

namespace Runway.Views
{
   public partial class MainWindow : Window
   {
      private readonly MainViewModel _viewModel;

      public MainWindow()
      {
         InitializeComponent();

         _viewModel = (MainViewModel) DataContext;
         _viewModel.MoveCaretRequested += OnMoveCaretRequested;
         _viewModel.DismissRequested += OnDismissRequested;
         _viewModel.Suggestions.CollectionChanged += OnSuggestionsChanged;
      }

      private void OnMoveCaretRequested( object sender, MoveCaretEventArgs e )
         => InputTextBox.MoveCaret( e.CaretPosition );

      private void OnDismissRequested( object sender, EventArgs e )
         => Visibility = Visibility.Hidden;

      private void OnSuggestionsChanged( object sender, NotifyCollectionChangedEventArgs e )
         => Height = _viewModel.Suggestions.Count == 0 ? 80 : 500;
   }
}
