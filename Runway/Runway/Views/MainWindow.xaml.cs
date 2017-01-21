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
         => InputTextBox.MoveCaret( e.CaretPosition );
   }
}
