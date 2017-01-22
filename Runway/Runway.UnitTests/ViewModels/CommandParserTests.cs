using FluentAssertions;
using Moq;
using Xunit;
using Runway.ViewModels;

namespace Runway.UnitTests.ViewModels
{
   public class CommandParserTests
   {
      [Fact]
      public void GetCommandSuggestion_CommandDoesNotExist_ReturnsNullSuggestion()
      {
         const string command = "doesnotexist";

         // Arrange

         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( command ) ).Returns( CommandCatalog.MissingCommand );

         // Act

         var commandParser = new CommandParser( commandCatalogMock.Object );

         string suggestion = commandParser.GetCommandSuggestion( command );

         // Assert

         suggestion.Should().BeNull();
      }

      [Fact]
      public void GetCommandSuggestion_CommandExists_ReturnsCorrectSuggestion()
      {
         const string partialCommandText = "co";
         const string commandText = "command";
         const string suggestionText = "mmand";

         // Arrange

         var launchableCommandMock = new Mock<ILaunchableCommand>();
         launchableCommandMock.SetupGet( lc => lc.CommandText ).Returns( commandText );

         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( partialCommandText ) ).Returns( launchableCommandMock.Object );

         // Act

         var commandParser = new CommandParser( commandCatalogMock.Object );

         string suggestion = commandParser.GetCommandSuggestion( partialCommandText );

         // Assert

         suggestion.Should().Be( suggestionText );
      }

      [Fact]
      public void ParseArguments_OnlyHasCommandButNoArguments_ReturnsNull()
      {
         var commandParser = new CommandParser( null );

         string arguments = commandParser.ParseArguments( "copy" );

         arguments.Should().BeNull();
      }

      [Fact]
      public void ParseArguments_OnlyHasCommandWithATrailingSpaceButNoArguments_ReturnsNull()
      {
         var commandParser = new CommandParser( null );

         string arguments = commandParser.ParseArguments( "copy " );

         arguments.Should().BeNull();
      }

      [Fact]
      public void ParseArguments_HasCommandAndArgument_ReturnsArgument()
      {
         const string command = "copy";
         const string arguments = "some text here";
         string fullText = $"{command} {arguments}";

         // Act

         var commandParser = new CommandParser( null );

         string justArguments = commandParser.ParseArguments( fullText );

         // Assert

         justArguments.Should().Be( arguments );
      }
   }
}
