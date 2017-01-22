using FluentAssertions;
using Moq;
using Xunit;

namespace Runway.UnitTests
{
   public class CommandCatalogTests
   {
      [Fact]
      public void Resolve_CommandExists_CommandIsFound()
      {
         const string commandText = "foo";

         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( c => c.CommandText ).Returns( commandText );

         // Act

         var commandCatalog = new CommandCatalog();

         commandCatalog.Add( commandMock.Object );

         var resolvedCommand = commandCatalog.Resolve( commandText );

         // Assert

         resolvedCommand.Should().Be( commandMock.Object );
      }

      [Fact]
      public void Resolve_CommandExists_CommandIsFoundByPartialNameMatch()
      {
         const string partialCommandText = "c";
         const string commandText = "copy";

         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( c => c.CommandText ).Returns( commandText );

         // Act

         var commandCatalog = new CommandCatalog();

         commandCatalog.Add( commandMock.Object );

         var resolvedCommand = commandCatalog.Resolve( partialCommandText );

         // Assert

         resolvedCommand.Should().Be( commandMock.Object );
      }

      [Fact]
      public void Resolve_PassesNullText_ReturnsMissingCommand()
      {
         // Act

         var commandCatalog = new CommandCatalog();

         var command = commandCatalog.Resolve( null );

         // Assert

         command.Should().Be( CommandCatalog.MissingCommand );
      }

      [Fact]
      public void Resolve_SpecifiedCommandDoesNotExist_ReturnsMissingCommand()
      {
         // Act

         var commandCatalog = new CommandCatalog();

         var command = commandCatalog.Resolve( "doesnotexist" );

         // Assert

         command.Should().Be( CommandCatalog.MissingCommand );
      }
   }
}
