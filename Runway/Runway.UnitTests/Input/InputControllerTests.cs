using System;
using Xunit;
using FluentAssertions;
using Moq;
using Runway.Input;
using Runway.UnitTests.Helpers;

namespace Runway.UnitTests.Input
{
   public class InputControllerTests
   {
      [Fact]
      public void Constructor_InitialFrameIsNull_ThrowsArgumentNullException()
      {
         Action constructor = () => new InputController( null );

         constructor.ShouldThrow<ArgumentNullException>();
      }

      [Fact]
      public void InputText_SearchesForSomething_GetsResultsFromInputFrame()
      {
         const string searchText = "c";

         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         var matchResults = ArrayHelper.Create( matchResultMock.Object );

         var inputFrameMock = new Mock<IInputFrame>();
         inputFrameMock.Setup( @if => @if.Match( searchText ) ).Returns( matchResults );

         // Act

         var inputController = new InputController( inputFrameMock.Object );

         inputController.InputText = searchText;

         // Assert

         inputController.MatchResults.Should().HaveCount( 1 );
         inputController.MatchResults[0].Should().Be( matchResultMock.Object );
      }
   }
}
