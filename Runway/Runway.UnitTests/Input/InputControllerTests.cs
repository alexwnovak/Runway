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
      public void Constructor_PassesInitialInputFrame_BecomesTheCurrentInputFrame()
      {
         // Arrange

         var inputFrameMock = new Mock<IInputFrame>();

         // Act

         var inputController = new InputController( inputFrameMock.Object );

         inputController.CurrentInputFrame.Should().Be( inputFrameMock.Object );
      }

      [Fact]
      public void Constructor_InitialFrameIsNull_ThrowsArgumentNullException()
      {
         Action constructor = () => new InputController( null );

         constructor.ShouldThrow<ArgumentNullException>();
      }

      [Fact]
      public void UpdateInputText_SearchesForSomething_GetsResultsFromInputFrame()
      {
         const string searchText = "c";

         // Arrange

         var matchResultMock = new Mock<IMatchResult>();
         var matchResults = ArrayHelper.Create( matchResultMock.Object );

         var inputFrameMock = new Mock<IInputFrame>();
         inputFrameMock.Setup( @if => @if.Match( searchText ) ).Returns( matchResults );

         // Act

         var inputController = new InputController( inputFrameMock.Object );

         var actualMatchResults = inputController.UpdateInputText( searchText );

         // Assert

         actualMatchResults.Should().HaveCount( 1 ).And.Contain( matchResultMock.Object );
      }
   }
}
