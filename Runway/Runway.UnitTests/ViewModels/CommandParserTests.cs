﻿using FluentAssertions;
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
         // Arrange

         var commandCatalogMock = new Mock<ICommandCatalog>();

         // Act

         var commandParser = new CommandParser( commandCatalogMock.Object );

         string suggestion = commandParser.GetCommandSuggestion( "doesnotexist" );

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
   }
}
