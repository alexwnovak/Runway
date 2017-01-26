using FluentAssertions;
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
      public void CurrentCommandText_PrefixMatchesCommand_ResolvesPreviewText()
      {
         const string partialCommand = "c";
         const string commandName = "copy";

         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( c => c.CommandText ).Returns( commandName );

         var matchResults = MatchResultHelper.Create( MatchType.Exact, commandMock.Object );
         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( partialCommand ) ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null )
         {
            CurrentCommandText = partialCommand
         };

         // Assert

         viewModel.PreviewCommandText.Should().Be( "opy" );
      }

      [Fact]
      public void CurrentCommandText_PrefixIsNull_PreviewTextIsNull()
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
      public void CompleteSuggestionCommand_HappyPath_RaisesPropertyChangeForCurrentCommandText()
      {
         // Act

         var viewModel = new MainViewModel( null, null );

         viewModel.MonitorEvents();

         viewModel.CompleteSuggestionCommand.Execute( null );

         // Assert

         viewModel.ShouldRaisePropertyChangeFor( vm => vm.CurrentCommandText );
      }

      [Fact]
      public void CompleteSuggestionCommand_HappyPath_RaisesMoveCaretRequested()
      {
         // Act

         var viewModel = new MainViewModel( null, null );

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
      public void CompleteSuggestionCommand_HasPartialTextAndSuggestion_PreviewTextBecomesNull()
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

         viewModel.PreviewCommandText.Should().BeEmpty();
      }

      [Fact]
      public void CommandText_CommandNotFoundForPrefix_PreviewCommandTextIsNull()
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
      public void CommandText_CommandIsFoundForPrefix_PreviewTextIsSetCorrectly()
      {
         const string commandPartialText = "co";
         const string commandText = "command";
         const string previewCommandText = "mmand";

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

         viewModel.PreviewCommandText.Should().Be( previewCommandText );
      }

      [Fact]
      public void LaunchCommand_CommandTextIsNull_LaunchesMissingCommandToDoAnything()
      {
         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();

         var matchResults = MatchResultHelper.Create( MatchType.Exact, commandMock.Object );
         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( It.IsAny<string>() ) ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.LaunchCommand.Execute( null );

         // Assert

         commandMock.Verify( c => c.Launch( It.IsAny<object[]>() ), Times.Once() );
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
