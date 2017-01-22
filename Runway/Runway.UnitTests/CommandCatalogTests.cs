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
   }
}
