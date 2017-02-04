using System;
using Xunit;
using FluentAssertions;
using Moq;
using Runway.Input;

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
   }
}
