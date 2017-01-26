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

         var results = commandCatalog.Resolve( commandText );

         // Assert

         results.Should().HaveCount( 1 );
         results[0].Command.Should().Be( commandMock.Object );
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

         var results = commandCatalog.Resolve( partialCommandText );

         // Assert

         results.Should().HaveCount( 1 );
         results[0].Command.Should().Be( commandMock.Object );
      }

      [Fact]
      public void Resolve_PassesNullText_ReturnsEmptyResultSet()
      {
         // Act

         var commandCatalog = new CommandCatalog();

         var results = commandCatalog.Resolve( null );

         // Assert

         results.Should().BeSameAs( CommandCatalog.EmptySet );
      }

      [Fact]
      public void Resolve_SpecifiedCommandDoesNotExist_ReturnsEmptySet()
      {
         // Act

         var commandCatalog = new CommandCatalog();

         var results = commandCatalog.Resolve( "doesnotexist" );

         // Assert

         results.Should().HaveCount( 0 );
      }
   }
}
