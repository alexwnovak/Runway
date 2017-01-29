﻿using FluentAssertions;
using Moq;
using Xunit;
using Runway.Services;
using Runway.UnitTests.Helpers;
using Runway.ViewModels;

namespace Runway.UnitTests.ViewModels
{
   public class MainViewModelTests
   {
      [Fact]
      public void CurrentMatchResults_DefaultState_HasNoMatchResults()
      {
         var viewModel = new MainViewModel( null, null );

         viewModel.CurrentMatchResults.Should().BeSameAs( CommandCatalog.EmptySet );
      }

      [Fact]
      public void CurrentMatchResults_MatchIsFound_MatchIsCached()
      {
         const string command = "x";

         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();

         var results = MatchResultHelper.Create( MatchType.Exact, commandMock.Object );
         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( command ) ).Returns( results );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.CurrentCommandText = command;

         // Assert

         viewModel.CurrentMatchResults.Should().HaveCount( 1 );
         viewModel.CurrentMatchResults.Should().Contain( m => m.Command == commandMock.Object );
      }

      [Fact]
      public void PreviewCommandText_CommandTextIsNull_PreviewTextIsNull()
      {
         // Arrange

         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( null ) ).Returns<ILaunchableCommand>( null );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null )
         {
            CurrentCommandText = null
         };

         // Assert

         viewModel.PreviewCommandText.Should().BeNull();
      }

      [Fact]
      public void PreviewCommandText_CommandNotFoundForPrefix_PreviewCommandTextIsNull()
      {
         const string command = "SomeCommand";

         // Arrange

         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( command ) ).Returns( new MatchResult[0] );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.CurrentCommandText = command;

         // Assert

         viewModel.PreviewCommandText.Should().BeNull();
      }

      [Fact]
      public void CompleteSuggestionCommand_HappyPath_RaisesPropertyChangeForCurrentCommandText()
      {
         const string command = "SomeCommand";

         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( c => c.CommandText ).Returns( command );

         var results = MatchResultHelper.Create( MatchType.Exact, commandMock.Object );
         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( command ) ).Returns( results );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );
         viewModel.CurrentCommandText = command;

         viewModel.MonitorEvents();

         viewModel.CompleteSuggestionCommand.Execute( null );

         // Assert

         viewModel.ShouldRaisePropertyChangeFor( vm => vm.CurrentCommandText );
      }

      [Fact]
      public void CompleteSuggestionCommand_HappyPath_RaisesMoveCaretRequested()
      {
         const string command = "SomeCommand";

         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( c => c.CommandText ).Returns( command );

         var results = MatchResultHelper.Create( MatchType.Exact, commandMock.Object );
         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( command ) ).Returns( results );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );
         viewModel.CurrentCommandText = command;

         viewModel.MonitorEvents();

         viewModel.CompleteSuggestionCommand.Execute( null );

         // Assert

         viewModel.ShouldRaise( nameof( viewModel.MoveCaretRequested ) )
                  .WithArgs<MoveCaretEventArgs>( e => e.CaretPosition == CaretPosition.End );
      }

      [Fact]
      public void CompleteSuggestionCommand_HasPartialTextAndSuggestion_SuggestionBecomesTheCommandText()
      {
         const string currentCommand = "c";
         const string commandName = "copy";

         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( c => c.CommandText ).Returns( commandName );

         var matchResults = MatchResultHelper.Create( MatchType.Exact, commandMock.Object );
         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( currentCommand ) ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null )
         {
            CurrentCommandText = currentCommand
         };

         viewModel.CompleteSuggestionCommand.Execute( null );

         // Assert

         viewModel.CurrentCommandText.Should().Be( commandName );
      }

      [Fact]
      public void CompleteSuggestionCommand_TextMatchesNoSuggestion_DoesNotTryToComplete()
      {
         const string currentCommand = "c";

         // Arrange

         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( It.IsAny<string>() ) ).Returns( CommandCatalog.EmptySet );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null )
         {
            CurrentCommandText = currentCommand
         };

         viewModel.MonitorEvents();

         viewModel.CompleteSuggestionCommand.Execute( null );

         // Assert

         viewModel.CurrentCommandText.Should().Be( currentCommand );
         viewModel.ShouldNotRaise( nameof( viewModel.MoveCaretRequested ) );
      }

      [Fact]
      public void CommandText_SetsCommandText_RaisesPropertyChangeForPreview()
      {
         // Arrange

         var commandCatalogMock = new Mock<ICommandCatalog>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.MonitorEvents();

         viewModel.CurrentCommandText = "doesntmatter";

         // Assert

         viewModel.ShouldRaisePropertyChangeFor( vm => vm.PreviewCommandText );
      }

      [Fact]
      public void CommandText_CommandIsFoundForPrefix_PreviewTextIsSetCorrectly()
      {
         const string commandPartialText = "co";
         const string commandText = "command";

         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( lc => lc.CommandText ).Returns( commandText );

         var matchResults = MatchResultHelper.Create( MatchType.Exact, commandMock.Object );
         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( commandPartialText ) ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.CurrentCommandText = commandPartialText;

         // Assert

         viewModel.PreviewCommandText.Should().Be( commandText );
      }

      [Fact]
      public void CommandText_MatchesACommand_MarksTheFirstResultAsSelected()
      {
         const string commandText = "copy";

         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( lc => lc.CommandText ).Returns( commandText );

         var matchResults = MatchResultHelper.Create( MatchType.Exact, commandMock.Object );
         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( commandText ) ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.CurrentCommandText = commandText;

         // Assert

         viewModel.SelectedSuggestion.Should().Be( matchResults[0] );
      }

      [Fact]
      public void CommandText_DoesNotMatchAnything_NoMatchIsSelected()
      {
         // Arrange

         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( It.IsAny<string>() ) ).Returns( CommandCatalog.EmptySet );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.CurrentCommandText = "doesnotmatter";

         // Assert

         viewModel.SelectedSuggestion.Should().Be( null );
      }

      [Fact]
      public void SelectNextSuggestionCommand_FirstCommandIsSelected_SecondCommandBecomesSelected()
      {
         const string commandText1 = "uninstall";
         const string commandText2 = "undo";

         // Arrange

         var commandMock1 = new Mock<ILaunchableCommand>();
         commandMock1.SetupGet( c => c.CommandText ).Returns( commandText1 );

         var commandMock2 = new Mock<ILaunchableCommand>();
         commandMock2.Setup( c => c.CommandText ).Returns( commandText2 );

         var matchResults = MatchResultHelper.CreatePartial( commandMock1.Object, commandMock2.Object );
         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( It.IsAny<string>() ) ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.CurrentCommandText = "u";
         viewModel.SelectNextSuggestionCommand.Execute( null );

         // Assert

         viewModel.SelectedSuggestion.Command.Should().Be( commandMock2.Object );
      }

      [Fact]
      public void SelectNextSuggestionCommand_CommandIsSelected_CaretIsMovedToTheEnd()
      {
         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();

         var matchResults = MatchResultHelper.CreatePartial( commandMock.Object );
         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( It.IsAny<string>() ) ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.MonitorEvents();

         viewModel.CurrentCommandText = "doesnotmatter";
         viewModel.SelectNextSuggestionCommand.Execute( null );

         // Assert

         viewModel.ShouldRaise( nameof( viewModel.MoveCaretRequested ) )
            .WithArgs<MoveCaretEventArgs>( e => e.CaretPosition == CaretPosition.End );
      }

      [Fact]
      public void SelectPreviousSuggestionCommand_FirstCommandIsSelected_SecondCommandBecomesSelected()
      {
         const string commandText1 = "uninstall";
         const string commandText2 = "undo";

         // Arrange

         var commandMock1 = new Mock<ILaunchableCommand>();
         commandMock1.SetupGet( c => c.CommandText ).Returns( commandText1 );

         var commandMock2 = new Mock<ILaunchableCommand>();
         commandMock2.Setup( c => c.CommandText ).Returns( commandText2 );

         var matchResults = MatchResultHelper.CreatePartial( commandMock1.Object, commandMock2.Object );
         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( It.IsAny<string>() ) ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.CurrentCommandText = "u";
         viewModel.SelectPreviousSuggestionCommand.Execute( null );

         // Assert

         viewModel.SelectedSuggestion.Command.Should().Be( commandMock2.Object );
      }

      [Fact]
      public void SelectPreviousSuggestionCommand_CommandIsSelected_CaretIsMovedToTheEnd()
      {
         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();

         var matchResults = MatchResultHelper.CreatePartial( commandMock.Object );
         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( It.IsAny<string>() ) ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.MonitorEvents();

         viewModel.CurrentCommandText = "doesnotmatter";
         viewModel.SelectPreviousSuggestionCommand.Execute( null );

         // Assert

         viewModel.ShouldRaise( nameof( viewModel.MoveCaretRequested ) )
            .WithArgs<MoveCaretEventArgs>( e => e.CaretPosition == CaretPosition.End );
      }

      [Fact]
      public void SpacePressedCommand_HasPartialCommandTextAndSuggestion_AutoCompletesSuggestion()
      {
         const string commandText = "copy";
         const string partialText = "c";
         
         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( c => c.CommandText ).Returns( commandText );

         var matchResults = MatchResultHelper.CreatePartial( commandMock.Object );
         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( partialText ) ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.MonitorEvents();

         viewModel.CurrentCommandText = partialText;

         viewModel.SpacePressedCommand.Execute( null );

         // Assert

         viewModel.CurrentCommandText.Should().Be( commandText + " " );
      }

      [Fact]
      public void LaunchCommand_LaunchesACommand_DismissesTheUI()
      {
         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();

         var matchResults = MatchResultHelper.Create( MatchType.Exact, commandMock.Object );
         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( It.IsAny<string>() ) ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );
         viewModel.CurrentCommandText = "doesnotmatter";

         viewModel.MonitorEvents();

         viewModel.LaunchCommand.Execute( null );

         // Assert

         viewModel.ShouldRaise( nameof( viewModel.DismissRequested ) );
      }

      [Fact]
      public void LaunchCommand_CommandTextDoesNotMatchAnyCommand_DoesNotDismiss()
      {
         // Arrange

         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( It.IsAny<string>() ) ).Returns( CommandCatalog.EmptySet );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.MonitorEvents();

         viewModel.LaunchCommand.Execute( null );

         // Assert

         viewModel.ShouldNotRaise( nameof( viewModel.DismissRequested ) );
      }

      [Fact]
      public void LaunchCommand_SelectedASuggestion_LaunchesTheSuggestion()
      {
         const string commandText1 = "uninstall";
         const string commandText2 = "undo";

         // Arrange

         var commandMock1 = new Mock<ILaunchableCommand>();
         commandMock1.SetupGet( c => c.CommandText ).Returns( commandText1 );

         var commandMock2 = new Mock<ILaunchableCommand>();
         commandMock2.Setup( c => c.CommandText ).Returns( commandText2 );

         var matchResults = MatchResultHelper.CreatePartial( commandMock1.Object, commandMock2.Object );
         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( It.IsAny<string>() ) ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.CurrentCommandText = "u";
         viewModel.SelectPreviousSuggestionCommand.Execute( null );
         viewModel.LaunchCommand.Execute( null );

         // Assert

         commandMock2.Verify( c => c.Launch( It.IsAny<object[]>() ), Times.Once() );
      }

      [Fact]
      public void ExitCommand_ExitCommandIsExecuted_ApplicationExits()
      {
         // Arrange

         var appService = new Mock<IAppService>();

         // Act

         var viewModel = new MainViewModel( null, appService.Object );

         viewModel.ExitCommand.Execute( null );

         // Assert

         appService.Verify( @as => @as.Exit(), Times.Once() );
      }
   }
}
