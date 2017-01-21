using System.Windows;
using System.Windows.Input;
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
      }

      private void MainWindow_OnPreviewKeyDown( object sender, KeyEventArgs e )
      {
         if ( e.Key == Key.Tab )
         {
            _viewModel.CompleteSuggestionCommand.Execute( null );
            e.Handled = true;
            InputTextBox.SelectionStart = InputTextBox.Text.Length;
         }
      }
   }
}
