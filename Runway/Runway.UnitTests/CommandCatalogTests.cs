using FluentAssertions;
using Moq;
using Xunit;

namespace Runway.UnitTests
{
   public class CommandCatalogTests
   {
      [Fact]
      public void Resolve_CommandExists_CommandIsFoundWithExactMatch()
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
         results[0].MatchType.Should().Be( MatchType.Exact );
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
         results[0].MatchType.Should().Be( MatchType.Partial );
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

      [Fact]
      public void Resolve_CatalogHasTwoCommands_FindsAnExactMatchAndAPartialMatch()
      {
         const string commandText1 = "co";
         const string commandText2 = "copy";

         // Arrange

         var commandMock1 = new Mock<ILaunchableCommand>();
         commandMock1.SetupGet( c => c.CommandText ).Returns( commandText1 );

         var commandMock2 = new Mock<ILaunchableCommand>();
         commandMock2.Setup( c => c.CommandText ).Returns( commandText2 );

         // Act

         var commandCatalog = new CommandCatalog();

         commandCatalog.Add( commandMock1.Object );
         commandCatalog.Add( commandMock2.Object );

         var results = commandCatalog.Resolve( commandText1 );

         // Assert

         results.Should().HaveCount( 2 );
         results.Should().Contain( c => c.Command == commandMock1.Object && c.MatchType == MatchType.Exact );
         results.Should().Contain( c => c.Command == commandMock2.Object && c.MatchType == MatchType.Partial );
      }

      [Fact]
      public void Resolve_CatalogHasTwoCommands_SearchCorrectlyOnlyFindsOneMatch()
      {
         const string commandText1 = "copy";
         const string commandText2 = "uninstall";

         // Arrange

         var commandMock1 = new Mock<ILaunchableCommand>();
         commandMock1.SetupGet( c => c.CommandText ).Returns( commandText1 );

         var commandMock2 = new Mock<ILaunchableCommand>();
         commandMock2.Setup( c => c.CommandText ).Returns( commandText2 );

         // Act

         var commandCatalog = new CommandCatalog();

         commandCatalog.Add( commandMock1.Object );
         commandCatalog.Add( commandMock2.Object );

         var results = commandCatalog.Resolve( commandText1 );

         // Assert

         results.Should().HaveCount( 1 );
         results.Should().Contain( c => c.Command == commandMock1.Object && c.MatchType == MatchType.Exact );
      }

      [Fact]
      public void Resolve_CatalogHasTwoCommands_BothFoundByPartialMatch()
      {
         const string commandText1 = "copy";
         const string commandText2 = "comment";

         // Arrange

         var commandMock1 = new Mock<ILaunchableCommand>();
         commandMock1.SetupGet( c => c.CommandText ).Returns( commandText1 );

         var commandMock2 = new Mock<ILaunchableCommand>();
         commandMock2.Setup( c => c.CommandText ).Returns( commandText2 );

         // Act

         var commandCatalog = new CommandCatalog();

         commandCatalog.Add( commandMock1.Object );
         commandCatalog.Add( commandMock2.Object );

         var results = commandCatalog.Resolve( "co" );

         // Assert

         results.Should().HaveCount( 2 );
         results.Should().Contain( c => c.Command == commandMock1.Object && c.MatchType == MatchType.Partial );
         results.Should().Contain( c => c.Command == commandMock2.Object && c.MatchType == MatchType.Partial );
      }
   }
}
