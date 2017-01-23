using FluentAssertions;
using Xunit;
using Runway.ViewModels;

namespace Runway.UnitTests.ViewModels
{
   public class CommandParserTests
   {
      [Fact]
      public void GetCommandSuggestion_TextMatchesCommand_ReturnsNullSuggestion()
      {
         string suggestion = CommandParser.GetCommandSuggestion( "FullCommand", "FullCommand" );

         suggestion.Should().BeEmpty();
      }

      [Fact]
      public void GetCommandSuggestion_TextPartiallyMatchesCommand_ReturnsCorrectSuggestion()
      {
         const string partialCommandText = "co";
         const string commandText = "command";
         const string suggestionText = "mmand";

         string suggestion = CommandParser.GetCommandSuggestion( partialCommandText, commandText );

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
