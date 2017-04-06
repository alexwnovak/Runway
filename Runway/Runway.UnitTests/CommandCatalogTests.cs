using FluentAssertions;
using Moq;
using Runway.ExtensibilityModel;
using Xunit;

namespace Runway.UnitTests
{
   public class CommandCatalogTests
   {
      [Fact]
      public void Search_CommandExists_CommandIsFoundWithExactMatch()
      {
         const string commandText = "foo";

         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( c => c.CommandText ).Returns( commandText );

         // Act

         var commandCatalog = new CommandCatalog();

         commandCatalog.Add( commandMock.Object );

         var results = commandCatalog.Search( commandText );

         // Assert

         results.Should().HaveCount( 1 );
         results[0].MatchType.Should().Be( MatchType.Exact );
         results[0].DisplayText.Should().Be( commandText );
      }

      [Fact]
      public void Search_CommandExists_CommandIsFoundByPartialNameMatch()
      {
         const string partialCommandText = "c";
         const string commandText = "copy";

         // Arrange

         var commandMock = new Mock<ILaunchableCommand>();
         commandMock.SetupGet( c => c.CommandText ).Returns( commandText );

         // Act

         var commandCatalog = new CommandCatalog();

         commandCatalog.Add( commandMock.Object );

         var results = commandCatalog.Search( partialCommandText );

         // Assert

         results.Should().HaveCount( 1 );
         results[0].MatchType.Should().Be( MatchType.Partial );
         results[0].DisplayText.Should().Be( commandText );
      }

      [Fact]
      public void Search_PassesNullText_ReturnsEmptyResultSet()
      {
         // Act

         var commandCatalog = new CommandCatalog();

         var results = commandCatalog.Search( null );

         // Assert

         results.Should().BeSameAs( CommandCatalog.EmptySet );
      }

      [Fact]
      public void Search_SpecifiedCommandDoesNotExist_ReturnsEmptySet()
      {
         // Act

         var commandCatalog = new CommandCatalog();

         var results = commandCatalog.Search( "doesnotexist" );

         // Assert

         results.Should().HaveCount( 0 );
      }

      [Fact]
      public void Search_CatalogHasTwoCommands_FindsAnExactMatchAndAPartialMatch()
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

         var results = commandCatalog.Search( commandText1 );

         // Assert

         results.Should().HaveCount( 2 );
         results.Should().Contain( c => c.DisplayText == commandText1 && c.MatchType == MatchType.Exact );
         results.Should().Contain( c => c.DisplayText == commandText2 && c.MatchType == MatchType.Partial );
      }

      [Fact]
      public void Search_CatalogHasTwoCommands_SearchCorrectlyOnlyFindsOneMatch()
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

         var results = commandCatalog.Search( commandText1 );

         // Assert

         results.Should().HaveCount( 1 );
         results.Should().Contain( c => c.DisplayText == commandText1 && c.MatchType == MatchType.Exact );
      }

      [Fact]
      public void Search_CatalogHasTwoCommands_BothFoundByPartialMatch()
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

         var results = commandCatalog.Search( "co" );

         // Assert

         results.Should().HaveCount( 2 );
         results.Should().Contain( c => c.DisplayText == commandText1 && c.MatchType == MatchType.Partial );
         results.Should().Contain( c => c.DisplayText == commandText2 && c.MatchType == MatchType.Partial );
      }

      [Fact]
      public void Search_CatalogHasTwoCommands_ReturnedInAlphabeticalOrder()
      {
         const string commandText1 = "uninstall";
         const string commandText2 = "undo";

         // Arrange

         var commandMock1 = new Mock<ILaunchableCommand>();
         commandMock1.SetupGet( c => c.CommandText ).Returns( commandText1 );

         var commandMock2 = new Mock<ILaunchableCommand>();
         commandMock2.Setup( c => c.CommandText ).Returns( commandText2 );

         // Act

         var commandCatalog = new CommandCatalog();

         commandCatalog.Add( commandMock1.Object );
         commandCatalog.Add( commandMock2.Object );

         var results = commandCatalog.Search( "u" );

         // Assert

         results.Should().HaveCount( 2 );
         results[0].DisplayText.Should().Be( commandText2 );
         results[1].DisplayText.Should().Be( commandText1 );
      }
   }
}
