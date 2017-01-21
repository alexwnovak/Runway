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
   }
}
