﻿using System.Windows;
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
         else if ( e.Key == Key.Return || e.Key == Key.Enter )
         {
            _viewModel.LaunchCommand.Execute( null );
            e.Handled = true;
         }
         else if ( e.Key == Key.Escape )
         {
            _viewModel.ExitCommand.Execute( null );
            e.Handled = true;
         }
      }
   }
}
