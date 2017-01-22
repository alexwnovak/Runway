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
         // Arrange

         var commandCatalogMock = new Mock<ICommandCatalog>();

         // Act

         var commandParser = new CommandParser( commandCatalogMock.Object );

         string suggestion = commandParser.GetCommandSuggestion( "doesnotexist" );

         // Assert

         suggestion.Should().BeNull();
      }
   }
}
