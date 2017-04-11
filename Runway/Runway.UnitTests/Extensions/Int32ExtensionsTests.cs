using FluentAssertions;
using Xunit;
using Runway.Extensions;

namespace Runway.UnitTests.Extensions
{
   public class Int32ExtensionsTests
   {
      [Fact]
      public void Increment_ValueIsBelowTheMax_ValueIsIncremented()
      {
         int value = 1;

         value = value.Increment( 10 );

         value.Should().Be( 2 );
      }

      [Fact]
      public void Increment_ValueReachesTheMax_ValueIsResetTo0()
      {
         int value = 1;

         value = value.Increment( 2 );

         value.Should().Be( 0 );
      }

      [Fact]
      public void Decrement_ValueIsAbove0_ValueIsDecremented()
      {
         int value = 5;

         value = value.Decrement( 10 );

         value.Should().Be( 4 );
      }

      [Fact]
      public void Decrement_ValueFallsBelow0_ValueIsReset()
      {
         int value = 0;

         value = value.Decrement( 5 );

         value.Should().Be( 5 );
      }
   }
}
