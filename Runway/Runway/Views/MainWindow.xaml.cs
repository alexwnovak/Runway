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
      }

      private void OnMoveCaretRequested( object sender, MoveCaretEventArgs e )
      {
         if ( e.CaretPosition == CaretPosition.Start )
         {
            InputTextBox.SelectionStart = 0;
         }
         else
         {
            InputTextBox.SelectionStart = InputTextBox.Text.Length;
         }
      }
   }
}
