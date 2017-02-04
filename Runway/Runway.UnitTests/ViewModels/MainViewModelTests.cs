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

         var commandCatalogMock = new Mock<ISearchCatalog>();
         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = text;

         // Assert

         inputControllerMock.VerifySet( ic => ic.InputText = text );
      }

      [Fact]
      public void InputText_SetsInputText_RaisesNotifyPropertyChangedForInputText()
      {
         // Arrange

         var commandCatalogMock = new Mock<ISearchCatalog>();
         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

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

         var commandCatalogMock = new Mock<ISearchCatalog>();
         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();
         inputControllerMock.SetupGet( ic => ic.InputText ).Returns( expectedInputString );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

         string inputText = viewModel.InputText;

         // Assert

         inputText.Should().Be( expectedInputString );
      }

      [Fact]
      public void CurrentMatchResults_DefaultState_HasNoMatchResults()
      {
         var appServiceMock = new Mock<IAppService>();

         var viewModel = new MainViewModel( null, appServiceMock.Object, null );

         viewModel.CurrentMatchResults.Should().BeSameAs( CommandCatalog.EmptySet );
      }

      [Fact]
      public void CurrentMatchResults_MatchIsFound_MatchIsCached()
      {
         const string command = "x";

         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         var results = ArrayHelper.Create( matchResultMock.Object );

         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( command ) ).Returns( results );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = command;

         // Assert

         viewModel.CurrentMatchResults.Should().HaveCount( 1 );
         viewModel.CurrentMatchResults[0].Should().Be( matchResultMock.Object );
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

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object )
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

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = command;

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
         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( command ) ).Returns( results );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );
         viewModel.InputText = command;

         viewModel.MonitorEvents();

         viewModel.CompleteSuggestionCommand.Execute( null );

         // Assert

         viewModel.ShouldRaisePropertyChangeFor( vm => vm.InputText );
      }

      [Fact]
      public void CompleteSuggestionCommand_HappyPath_RaisesMoveCaretRequested()
      {
         const string command = "SomeCommand";

         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( c => c.CommandText ).Returns( command );

         var results = MatchResultHelper.Create( MatchType.Exact, commandMock.Object );
         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( command ) ).Returns( results );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );
         viewModel.InputText = command;

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
         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( currentCommand ) ).Returns( matchResults );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object )
         {
            InputText = currentCommand
         };

         viewModel.CompleteSuggestionCommand.Execute( null );

         // Assert

         viewModel.InputText.Should().Be( commandName );
      }

      [Fact]
      public void CompleteSuggestionCommand_TextMatchesNoSuggestion_DoesNotTryToComplete()
      {
         const string currentCommand = "c";

         // Arrange

         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( It.IsAny<string>() ) ).Returns( CommandCatalog.EmptySet );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object )
         {
            InputText = currentCommand
         };

         viewModel.MonitorEvents();

         viewModel.CompleteSuggestionCommand.Execute( null );

         // Assert

         viewModel.InputText.Should().Be( currentCommand );
         viewModel.ShouldNotRaise( nameof( viewModel.MoveCaretRequested ) );
      }

      [Fact]
      public void InputText_SetsCommandText_RaisesPropertyChangeForPreview()
      {
         // Arrange

         var commandCatalogMock = new Mock<ISearchCatalog>();
         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

         viewModel.MonitorEvents();

         viewModel.InputText = "doesntmatter";

         // Assert

         viewModel.ShouldRaisePropertyChangeFor( vm => vm.PreviewCommandText );
      }

      [Fact]
      public void InputText_CommandIsFoundForPrefix_PreviewTextIsSetCorrectly()
      {
         const string commandPartialText = "co";
         const string inputText = "command";

         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( lc => lc.CommandText ).Returns( inputText );

         var matchResults = MatchResultHelper.Create( MatchType.Exact, commandMock.Object );
         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( commandPartialText ) ).Returns( matchResults );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = commandPartialText;

         // Assert

         viewModel.PreviewCommandText.Should().Be( inputText );
      }

      [Fact]
      public void InputText_MatchesACommand_MarksTheFirstResultAsSelected()
      {
         const string inputText = "copy";

         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( lc => lc.CommandText ).Returns( inputText );

         var matchResults = MatchResultHelper.Create( MatchType.Exact, commandMock.Object );
         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( inputText ) ).Returns( matchResults );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = inputText;

         // Assert

         viewModel.SelectedSuggestion.Should().Be( matchResults[0] );
      }

      [Fact]
      public void InputText_DoesNotMatchAnything_NoMatchIsSelected()
      {
         // Arrange

         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( It.IsAny<string>() ) ).Returns( CommandCatalog.EmptySet );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = "doesnotmatter";

         // Assert

         viewModel.SelectedSuggestion.Should().Be( null );
      }

      [Fact]
      public void SelectNextSuggestionCommand_FirstCommandIsSelected_SecondCommandBecomesSelected()
      {
         // Arrange

         var matchResultMock1 = new Mock<IMatchResult>();
         var matchResultMock2 = new Mock<IMatchResult>();
         var matchResults = ArrayHelper.Create( matchResultMock1.Object, matchResultMock2.Object );

         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( It.IsAny<string>() ) ).Returns( matchResults );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = "u";
         viewModel.SelectNextSuggestionCommand.Execute( null );

         // Assert

         viewModel.SelectedSuggestion.Should().Be( matchResultMock2.Object );
      }

      [Fact]
      public void SelectNextSuggestionCommand_CommandIsSelected_CaretIsMovedToTheEnd()
      {
         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();

         var matchResults = MatchResultHelper.CreatePartial( commandMock.Object );
         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( It.IsAny<string>() ) ).Returns( matchResults );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

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
         const string commandText1 = "uninstall";
         const string commandText2 = "undo";

         // Arrange

         var matchResultMock1 = new Mock<IMatchResult>();
         matchResultMock1.SetupGet( mr => mr.DisplayText ).Returns( commandText1 );

         var matchResultMock2 = new Mock<IMatchResult>();
         matchResultMock2.SetupGet( mr => mr.DisplayText ).Returns( commandText2 );

         var matchResults = ArrayHelper.Create( matchResultMock1.Object, matchResultMock2.Object );
         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( It.IsAny<string>() ) ).Returns( matchResults );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = "u";
         viewModel.SelectPreviousSuggestionCommand.Execute( null );

         // Assert

         viewModel.SelectedSuggestion.Should().Be( matchResultMock2.Object );
      }

      [Fact]
      public void SelectPreviousSuggestionCommand_CommandIsSelected_CaretIsMovedToTheEnd()
      {
         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();

         var matchResults = MatchResultHelper.CreatePartial( commandMock.Object );
         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( It.IsAny<string>() ) ).Returns( matchResults );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

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

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( c => c.CommandText ).Returns( commandText );

         var matchResults = MatchResultHelper.CreatePartial( commandMock.Object );
         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( partialText ) ).Returns( matchResults );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

         viewModel.MonitorEvents();

         viewModel.InputText = partialText;

         viewModel.SpacePressedCommand.Execute( null );

         // Assert

         viewModel.InputText.Should().Be( commandText + " " );
      }

      [Fact]
      public void LaunchCommand_LaunchesACommand_DismissesTheUI()
      {
         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();

         var matchResults = MatchResultHelper.Create( MatchType.Exact, commandMock.Object );
         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( It.IsAny<string>() ) ).Returns( matchResults );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );
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

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

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
         var commandCatalogMock = new Mock<ISearchCatalog>();
         commandCatalogMock.Setup( cc => cc.Search( It.IsAny<string>() ) ).Returns( matchResults );

         var appServiceMock = new Mock<IAppService>();
         var inputControllerMock = new Mock<IInputController>();

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object, appServiceMock.Object, inputControllerMock.Object );

         viewModel.InputText = "u";
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

         var viewModel = new MainViewModel( null, appService.Object, null );

         viewModel.ExitCommand.Execute( null );

         // Assert

         appService.Verify( @as => @as.Exit(), Times.Once() );
      }
   }
}
