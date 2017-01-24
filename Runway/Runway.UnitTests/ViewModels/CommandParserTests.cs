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
      public void ParseCommand_CommandIsNull_ReturnsNull()
      {
         string command = CommandParser.ParseCommand( null );

         command.Should().BeNull();
      }

      [Fact]
      public void ParseCommand_CommandIsEmpty_ReturnsNull()
      {
         string command = CommandParser.ParseCommand( string.Empty );

         command.Should().BeNull();
      }

      [Fact]
      public void ParseCommand_HasAWholeCommandWithNoSpace_ReturnsTheCommand()
      {
         const string fullCommandText = "copy";

         string command = CommandParser.ParseCommand( fullCommandText );

         command.Should().Be( fullCommandText );
      }

      [Fact]
      public void ParseCommand_HasWholeCommandWithTrailingSpace_ReturnsTheCommandWithSpaceRemoved()
      {
         const string fullCommandText = "copy";

         string command = CommandParser.ParseCommand( $"{fullCommandText} " );

         command.Should().Be( fullCommandText );
      }

      [Fact]
      public void ParseCommand_HasCommandAndArguments_ReturnsCommand()
      {
         const string fullCommandText = "copy";

         string command = CommandParser.ParseCommand( $"{fullCommandText} extra stuff after" );

         command.Should().Be( fullCommandText );
      }

      [Fact]
      public void ParseArguments_OnlyHasCommandButNoArguments_ReturnsNull()
      {
         string arguments = CommandParser.ParseArguments( "copy" );

         arguments.Should().BeNull();
      }

      [Fact]
      public void ParseArguments_OnlyHasCommandWithATrailingSpaceButNoArguments_ReturnsNull()
      {
         string arguments = CommandParser.ParseArguments( "copy " );

         arguments.Should().BeNull();
      }

      [Fact]
      public void ParseArguments_HasCommandAndArgument_ReturnsArgument()
      {
         const string command = "copy";
         const string arguments = "some text here";
         string fullText = $"{command} {arguments}";

         string justArguments = CommandParser.ParseArguments( fullText );

         justArguments.Should().Be( arguments );
      }
   }
}
