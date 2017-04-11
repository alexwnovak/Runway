using Xunit;
using Moq;
using FluentAssertions;
using Runway.ExtensibilityModel;
using Runway.Input;
using Runway.Services;
using Runway.UnitTests.Helpers;
using Runway.ViewModels;

namespace Runway.UnitTests.ViewModels
{
   public class MainViewModelTests
   {
      [Fact]
      public void ChangeInputTextCommand_SetsInputText_SuggestionsAreSet()
      {
         const string text = "input text here";

         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         IMatchResult[] matchResults = null;

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupGet( ic => ic.MatchResults ).Returns( matchResults );
         inputControllerMock.SetupSet( ic => ic.InputText = text )
            .Callback( () => inputControllerMock.SetupGet( ic => ic.MatchResults ).Returns( ArrayHelper.Create( matchResultMock.Object ) ) );

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.ChangeInputTextCommand.Execute( text );

         // Assert

         viewModel.Suggestions.Should().HaveCount( 1 ).And.Contain( matchResultMock.Object );
      }

      [Fact]
      public void ChangeInputTextCommand_TextHasNoSuggestions_SelectedSuggestionIsNull()
      {
         // Arrange

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.ChangeInputTextCommand.Execute( "doesnt matter" );

         // Assert

         viewModel.SelectedSuggestion.Should().BeNull();
      }

      [Fact]
      public void ChangeInputTextCommand_TextHasTwoSuggestions_TakesFirstAsSelectedSuggestion()
      {
         const string text = "input text here";

         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         var matchResultMock2 = new Mock<IMatchResult>();
         IMatchResult[] matchResults = ArrayHelper.Create( matchResultMock.Object, matchResultMock2.Object );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupGet( ic => ic.MatchResults ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.ChangeInputTextCommand.Execute( text );

         // Assert

         viewModel.SelectedSuggestion.Should().Be( matchResultMock.Object );
      }

      [Fact]
      public void ChangeInputTextCommand_HasSuggestion_SuggestionTextBecomesPreviewText()
      {
         const string text = "input text here";

         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         matchResultMock.SetupGet( mr => mr.DisplayText ).Returns( text );
         IMatchResult[] matchResults = ArrayHelper.Create( matchResultMock.Object );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupGet( ic => ic.MatchResults ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.MonitorEvents();

         viewModel.ChangeInputTextCommand.Execute( text );

         // Assert

         viewModel.PreviewCommandText.Should().Be( text );
         viewModel.ShouldRaisePropertyChangeFor( vm => vm.PreviewCommandText );
      }

      [Fact]
      public void PreviewCommandText_HasNoSuggestion_PreviewTextIsNull()
      {
         var viewModel = new MainViewModel( null, null );

         viewModel.PreviewCommandText.Should().BeNull();
      }

      [Fact]
      public void CompleteSuggestionCommand_HasNoSuggestion_EventsAreNotRaised()
      {
         var viewModel = new MainViewModel( null, null );
         viewModel.MonitorEvents();

         viewModel.CompleteSuggestionCommand.Execute( null );

         viewModel.ShouldNotRaise( nameof( viewModel.ChangeTextRequested ) );
         viewModel.ShouldNotRaise( nameof( viewModel.MoveCaretRequested ) );
      }

      [Fact]
      public void CompleteSuggestionCommand_HasSuggestion_RaisesChangeTextRequested()
      {
         const string text = "command text";

         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         matchResultMock.SetupGet( mr => mr.DisplayText ).Returns( text );

         // Act

         var viewModel = new MainViewModel( null, null )
         {
            SelectedSuggestion = matchResultMock.Object
         };

         viewModel.MonitorEvents();
         viewModel.CompleteSuggestionCommand.Execute( null );

         // Assert

         viewModel.ShouldRaise( nameof( viewModel.ChangeTextRequested ) )
            .WithArgs<ChangeTextRequestedEventArgs>( e => e.Text == text  );
      }

      [Fact]
      public void CompleteSuggestionCommand_HasSuggestion_MovesCaretToEnd()
      {
         const string text = "command text";

         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         matchResultMock.SetupGet( mr => mr.DisplayText ).Returns( text );

         // Act

         var viewModel = new MainViewModel( null, null )
         {
            SelectedSuggestion = matchResultMock.Object
         };

         viewModel.MonitorEvents();
         viewModel.CompleteSuggestionCommand.Execute( null );

         // Assert

         viewModel.ShouldRaise( nameof( viewModel.MoveCaretRequested ) )
            .WithArgs<MoveCaretEventArgs>( e => e.CaretPosition == CaretPosition.End );
      }

      [Fact]
      public void SelectNextSuggestionCommand_HasTwoSuggestions_SecondSuggestionBecomesSelected()
      {
         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         var matchResultMock2 = new Mock<IMatchResult>();

         // Act

         var viewModel = new MainViewModel( null, null );
         
         viewModel.Suggestions.Add( matchResultMock.Object );
         viewModel.Suggestions.Add( matchResultMock2.Object );

         viewModel.SelectNextSuggestionCommand.Execute( null );

         // Assert

         viewModel.SelectedSuggestion.Should().Be( matchResultMock2.Object );
      }

      [Fact]
      public void SelectNextSuggestionCommand_HasTwoSuggestions_MovesCaretToEnd()
      {
         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         var matchResultMock2 = new Mock<IMatchResult>();

         // Act

         var viewModel = new MainViewModel( null, null );
         viewModel.MonitorEvents();

         viewModel.Suggestions.Add( matchResultMock.Object );
         viewModel.Suggestions.Add( matchResultMock2.Object );

         viewModel.SelectNextSuggestionCommand.Execute( null );

         // Assert

         viewModel.ShouldRaise( nameof( viewModel.MoveCaretRequested ) )
            .WithArgs<MoveCaretEventArgs>( e => e.CaretPosition == CaretPosition.End );
      }

      [Fact]
      public void SelectPreviousSuggestionCommand_HasTwoSuggestions_SecondSuggestionBecomesSelected()
      {
         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         var matchResultMock2 = new Mock<IMatchResult>();

         // Act

         var viewModel = new MainViewModel( null, null );

         viewModel.Suggestions.Add( matchResultMock.Object );
         viewModel.Suggestions.Add( matchResultMock2.Object );

         viewModel.SelectPreviousSuggestionCommand.Execute( null );

         // Assert

         viewModel.SelectedSuggestion.Should().Be( matchResultMock2.Object );
      }

      [Fact]
      public void SelectPreviousSuggestionCommand_HasTwoSuggestions_MovesCaretToEnd()
      {
         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         var matchResultMock2 = new Mock<IMatchResult>();

         // Act

         var viewModel = new MainViewModel( null, null );
         viewModel.MonitorEvents();

         viewModel.Suggestions.Add( matchResultMock.Object );
         viewModel.Suggestions.Add( matchResultMock2.Object );

         viewModel.SelectPreviousSuggestionCommand.Execute( null );

         // Assert

         viewModel.ShouldRaise( nameof( viewModel.MoveCaretRequested ) )
            .WithArgs<MoveCaretEventArgs>( e => e.CaretPosition == CaretPosition.End );
      }

      [Fact]
      public void LaunchCommand_HasSuggestion_LaunchesCommand()
      {
         // Arrange

         var matchResultMock = new Mock<IMatchResult>();

         // Act

         var viewModel = new MainViewModel( null, null )
         {
            SelectedSuggestion = matchResultMock.Object
         };

         viewModel.LaunchCommand.Execute( null );

         // Assert

         matchResultMock.Verify( mr => mr.Activate( null ), Times.Once() );
      }

      [Fact]
      public void LaunchCommand_HasSuggestion_RaisesDismissRequested()
      {
         var viewModel = new MainViewModel( null, null )
         {
            SelectedSuggestion = Mock.Of<IMatchResult>()
         };

         viewModel.MonitorEvents();
         viewModel.LaunchCommand.Execute( null );

         viewModel.ShouldRaise( nameof( viewModel.DismissRequested ) );
      }

      [Fact]
      public void ExitCommand_ExecutesCommand_ExitsApplication()
      {
         // Arrange

         var appService = new Mock<IAppService>();

         // Act

         var viewModel = new MainViewModel( appService.Object, null );

         viewModel.ExitCommand.Execute( null );

         // Assert

         appService.Verify( @as => @as.Exit(), Times.Once() );
      }

      [Fact]
      public void DismissCommand_IsExecuted_RaisesDismissRequested()
      {
         var viewModel = new MainViewModel( null, null );

         viewModel.MonitorEvents();
         viewModel.DismissCommand.Execute( null );

         viewModel.ShouldRaise( nameof( viewModel.DismissRequested ) );
      }

      [Fact]
      public void DismissCommand_IsExecuted_ClearsCurrentState()
      {
         var viewModel = new MainViewModel( null, null );
         viewModel.MonitorEvents();

         viewModel.DismissCommand.Execute( null );

         viewModel.ShouldRaise( nameof( viewModel.ChangeTextRequested ) )
            .WithArgs<ChangeTextRequestedEventArgs>( e => e.Text == string.Empty );
      }
   }
}
