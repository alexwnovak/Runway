//using System;
//using FluentAssertions;
//using Moq;
//using Runway.ExtensibilityModel;
//using Xunit;
//using Runway.Input;
//using Runway.UnitTests.Helpers;

//namespace Runway.UnitTests.Input
//{
//   public class InputFrameTests
//   {
//      [Fact]
//      public void Match_SearchCatalogIsNull_ThrowsArgumentNullException()
//      {
//         Action constructor = () => new InputFrame( null );

//         constructor.ShouldThrow<ArgumentNullException>();
//      }

//      [Fact]
//      public void Match_SearchesWithText_ReturnsMatchResultsForText()
//      {
//         const string searchText = "search";

//         // Arrange

//         var matchResultMock = new Mock<IMatchResult>();
//         var expectedMatchResults = ArrayHelper.Create( matchResultMock.Object );

//         var searchCatalogMock = new Mock<ISearchCatalog>();
//         searchCatalogMock.Setup( sc => sc.Search( searchText ) ).Returns( expectedMatchResults );

//         // Act

//         var inputFrame = new InputFrame( searchCatalogMock.Object );

//         var matchResults = inputFrame.Match( searchText );

//         // Assert

//         matchResults.Should().HaveCount( 1 );
//         matchResults[0].Should().Be( matchResultMock.Object );
//      }
//   }
//}
