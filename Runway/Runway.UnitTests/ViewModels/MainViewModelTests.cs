using FluentAssertions;
using Moq;
using Xunit;
using Runway.Input;
using Runway.Services;
using Runway.UnitTests.Helpers;
using Runway.ViewModels;

namespace Runway.UnitTests.ViewModels
{
   public class MainViewModelTests
   {
      [Fact]
      public void InputText_SetsInputText_PassesInputToInputController()
      {
         const string text = "input text here";

         // Arrange

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = text;

         // Assert

         inputControllerMock.VerifySet( ic => ic.InputText = text );
      }

      [Fact]
      public void InputText_SetsInputText_RaisesNotifyPropertyChangedForInputText()
      {
         // Arrange

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.MonitorEvents();

         viewModel.InputText = "doesntmatter";

         // Assert

         viewModel.ShouldRaisePropertyChangeFor( vm => vm.InputText );
      }

      [Fact]
      public void InputText_ReadsInputText_InputTextComesFromInputController()
      {
         const string expectedInputString = "input text";

         // Arrange

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupGet( ic => ic.InputText ).Returns( expectedInputString );

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         string inputText = viewModel.InputText;

         // Assert

         inputText.Should().Be( expectedInputString );
      }

      [Fact]
      public void InputText_RetrievesMatches_SetsSelectedSuggestionToFirstResult()
      {
         // Arrange

         var appServiceMock = new Mock<IAppService>();

         var matchResultMock = new Mock<IMatchResult>();
         var matchResults = ArrayHelper.Create( matchResultMock.Object );

         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupGet( ic => ic.MatchResults ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = "doesntmatter";

         // Assert

         viewModel.SelectedSuggestion.Should().Be( matchResultMock.Object );
      }

      [Fact]
      public void InputText_DoesNotMatchAnything_NoMatchIsSelected()
      {
         // Arrange

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = "doesnotmatter";

         // Assert

         viewModel.SelectedSuggestion.Should().Be( null );
      }

      [Fact]
      public void InputText_SetsCommandText_RaisesPropertyChangeForPreview()
      {
         // Arrange

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.MonitorEvents();

         viewModel.InputText = "doesntmatter";

         // Assert

         viewModel.ShouldRaisePropertyChangeFor( vm => vm.PreviewCommandText );
      }

      [Fact]
      public void InputText_CommandIsFoundForPrefix_PreviewTextIsSetCorrectly()
      {
         const string inputText = "command";

         // Arrange

         var appServiceMock = new Mock<IAppService>();

         var matchResultMock = new Mock<IMatchResult>();
         matchResultMock.SetupGet( mr => mr.DisplayText ).Returns( inputText );
         var matchResults = ArrayHelper.Create( matchResultMock.Object );

         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupGet( ic => ic.MatchResults ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = inputText;

         // Assert

         viewModel.PreviewCommandText.Should().Be( inputText );
      }

      [Fact]
      public void PreviewCommandText_CommandTextIsNull_PreviewTextIsNull()
      {
         // Arrange

         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( null ) ).Returns<ILaunchableCommand>( null );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object )
         {
            InputText = null
         };

         // Assert

         viewModel.PreviewCommandText.Should().BeNull();
      }

      [Fact]
      public void PreviewCommandText_CommandNotFoundForPrefix_PreviewCommandTextIsNull()
      {
         const string command = "SomeCommand";

         // Arrange

         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( command ) ).Returns( new MatchResult[0] );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = command;

         // Assert

         viewModel.PreviewCommandText.Should().BeNull();
      }

      [Fact]
      public void CompleteSuggestionCommand_CompletingASuggestion_InputTextBecomesTheCompleteText()
      {
         const string partialCommand = "s";
         const string command = "somecommand";

         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         matchResultMock.SetupGet( mr => mr.DisplayText ).Returns( command );
         var results = ArrayHelper.Create( matchResultMock.Object );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupAllProperties();
         inputControllerMock.Setup( ic => ic.MatchResults ).Returns( results );

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );
         viewModel.InputText = partialCommand;

         viewModel.CompleteSuggestionCommand.Execute( null );

         // Assert

         viewModel.InputText.Should().Be( command );
      }

      [Fact]
      public void CompleteSuggestionCommand_CompletesTheSelectedSuggestion_RaisesMoveCaretRequested()
      {
         const string command = "somecommand";

         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         matchResultMock.SetupGet( mr => mr.DisplayText ).Returns( command );
         var results = ArrayHelper.Create( matchResultMock.Object );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupAllProperties();
         inputControllerMock.Setup( ic => ic.MatchResults ).Returns( results );

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );
         viewModel.InputText = "s";

         viewModel.MonitorEvents();

         viewModel.CompleteSuggestionCommand.Execute( null );

         // Assert

         viewModel.ShouldRaise( nameof( viewModel.MoveCaretRequested ) )
                  .WithArgs<MoveCaretEventArgs>( e => e.CaretPosition == CaretPosition.End );
      }

      [Fact]
      public void CompleteSuggestionCommand_TextMatchesNoSuggestion_InputTextDoesNotChange()
      {
         const string currentCommand = "c";

         // Arrange

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupAllProperties();

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object )
         {
            InputText = currentCommand
         };

         viewModel.MonitorEvents();

         viewModel.CompleteSuggestionCommand.Execute( null );

         // Assert

         viewModel.InputText.Should().Be( currentCommand );
      }

      [Fact]
      public void SelectNextSuggestionCommand_FirstCommandIsSelected_SecondCommandBecomesSelected()
      {
         // Arrange

         var matchResultMock1 = new Mock<IMatchResult>();
         var matchResultMock2 = new Mock<IMatchResult>();
         var matchResults = ArrayHelper.Create( matchResultMock1.Object, matchResultMock2.Object );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupGet( ic => ic.MatchResults ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = "doesntmatter";
         viewModel.SelectNextSuggestionCommand.Execute( null );

         // Assert

         viewModel.SelectedSuggestion.Should().Be( matchResultMock2.Object );
      }

      [Fact]
      public void SelectNextSuggestionCommand_CommandIsSelected_CaretIsMovedToTheEnd()
      {
         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         var matchResults = ArrayHelper.Create( matchResultMock.Object );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupGet( ic => ic.MatchResults ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.MonitorEvents();

         viewModel.InputText = "doesnotmatter";
         viewModel.SelectNextSuggestionCommand.Execute( null );

         // Assert

         viewModel.ShouldRaise( nameof( viewModel.MoveCaretRequested ) )
            .WithArgs<MoveCaretEventArgs>( e => e.CaretPosition == CaretPosition.End );
      }

      [Fact]
      public void SelectPreviousSuggestionCommand_FirstCommandIsSelected_SecondCommandBecomesSelected()
      {
         // Arrange

         var matchResultMock1 = new Mock<IMatchResult>();
         var matchResultMock2 = new Mock<IMatchResult>();
         var matchResults = ArrayHelper.Create( matchResultMock1.Object, matchResultMock2.Object );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.Setup( ic => ic.MatchResults ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = "u";
         viewModel.SelectPreviousSuggestionCommand.Execute( null );

         // Assert

         viewModel.SelectedSuggestion.Should().Be( matchResultMock2.Object );
      }

      [Fact]
      public void SelectPreviousSuggestionCommand_CommandIsSelected_CaretIsMovedToTheEnd()
      {
         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         var matchResults = ArrayHelper.Create( matchResultMock.Object );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupGet( ic => ic.MatchResults ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.MonitorEvents();

         viewModel.InputText = "doesnotmatter";
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

         var matchResultMock = new Mock<IMatchResult>();
         matchResultMock.SetupGet( mr => mr.DisplayText ).Returns( commandText );
         var matchResults = ArrayHelper.Create( matchResultMock.Object );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupAllProperties();
         inputControllerMock.SetupGet( ic => ic.MatchResults ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = partialText;

         viewModel.SpacePressedCommand.Execute( null );

         // Assert

         viewModel.InputText.Should().Be( commandText + " " );
      }

      [Fact]
      public void LaunchCommand_LaunchesACommand_DismissesTheUI()
      {
         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         var matchResults = ArrayHelper.Create( matchResultMock.Object );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupGet( ic => ic.MatchResults ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );
         viewModel.InputText = "doesnotmatter";

         viewModel.MonitorEvents();

         viewModel.LaunchCommand.Execute( null );

         // Assert

         viewModel.ShouldRaise( nameof( viewModel.DismissRequested ) );
      }

      [Fact]
      public void LaunchCommand_CommandTextDoesNotMatchAnyCommand_DoesNotDismiss()
      {
         // Arrange

         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( It.IsAny<string>() ) ).Returns( CommandCatalog.EmptySet );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.MonitorEvents();

         viewModel.LaunchCommand.Execute( null );

         // Assert

         viewModel.ShouldNotRaise( nameof( viewModel.DismissRequested ) );
      }

      [Fact]
      public void LaunchCommand_SelectedASuggestion_LaunchesTheSuggestion()
      {
         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         var matchResults = ArrayHelper.Create( matchResultMock.Object );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupGet( ic => ic.MatchResults ).Returns( matchResults );

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = "doesntmatter";

         viewModel.LaunchCommand.Execute( null );

         // Assert

         matchResultMock.Verify( mr => mr.Activate( It.IsAny<object[]>() ), Times.Once() );
      }

      [Fact]
      public void ExitCommand_ExitCommandIsExecuted_ApplicationExits()
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
         // Arrange

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.MonitorEvents();

         viewModel.DismissCommand.Execute( null );

         // Assert

         viewModel.ShouldRaise( nameof( viewModel.DismissRequested ) );
      }

      [Fact]
      public void DismissCommand_IsExecuted_ClearsCurrentState()
      {
         // Arrange

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupAllProperties();

         // Act

         var viewModel = new MainViewModel( appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = "Some search criteria";

         viewModel.DismissCommand.Execute( null );

         // Assert

         viewModel.InputText.Should().BeNull();
      }
   }
}
