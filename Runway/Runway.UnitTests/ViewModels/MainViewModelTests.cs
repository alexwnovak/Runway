using FluentAssertions;
using Moq;
using Runway.Services;
using Runway.ViewModels;
using Xunit;

namespace Runway.UnitTests.ViewModels
{
   public class MainViewModelTests
   {
      [Fact]
      public void CurrentCommand_NoCommandTextSet_CurrentCommandIsNull()
      {
         var viewModel = new MainViewModel( null, null );

         viewModel.CurrentCommand.Should().BeNull();
      }

      [Fact]
      public void CurrentCommandText_PrefixMatchesCommand_ResolvesPreviewText()
      {
         const string partialCommand = "c";
         const string commandName = "copy";

         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( c => c.CommandText ).Returns( commandName );

         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( partialCommand ) ).Returns( commandMock.Object );

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
      public void CompleteSuggestionCommand_HasPreviewCommandText_RaisesPropertyChangeForCurrentCommandText()
      {
         // Act

         var viewModel = new MainViewModel( null, null )
         {
            PreviewCommandText = "Doesn't matter"
         };

         viewModel.MonitorEvents();

         viewModel.CompleteSuggestionCommand.Execute( null );

         // Assert

         viewModel.ShouldRaisePropertyChangeFor( vm => vm.CurrentCommandText );
      }

      [Fact]
      public void CompleteSuggestionCommand_HasPreviewCommandText_RaisesMoveCaretRequested()
      {
         // Act

         var viewModel = new MainViewModel( null, null )
         {
            PreviewCommandText = "Doesn't matter"
         };

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

         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( currentCommand ) ).Returns( commandMock.Object );

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

         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( currentCommand ) ).Returns( commandMock.Object );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null )
         {
            CurrentCommandText = currentCommand
         };

         viewModel.CompleteSuggestionCommand.Execute( null );

         // Assert

         viewModel.PreviewCommandText.Should().BeNull();
      }

      [Fact]
      public void CommandText_CommandNotFoundForPrefix_PreviewCommandTextIsNull()
      {
         const string command = "SomeCommand";

         // Arrange

         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( command ) ).Returns( CommandCatalog.MissingCommand );

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

         var launchableCommand = new Mock<ILaunchableCommand>();
         launchableCommand.SetupGet( lc => lc.CommandText ).Returns( commandText );

         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( commandPartialText ) ).Returns( launchableCommand.Object );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.CurrentCommandText = commandPartialText;

         // Assert

         viewModel.PreviewCommandText.Should().Be( previewCommandText );
      }

      [Fact]
      public void LaunchCommand_CommandTextIsNull_DoesNotLaunchAnything()
      {
         // Arrange

         var commandCatalogMock = new Mock<ICommandCatalog>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, null );

         viewModel.LaunchCommand.Execute( null );

         // Assert

         commandCatalogMock.Verify( cc => cc.Resolve( It.IsAny<string>() ), Times.Never() );
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
