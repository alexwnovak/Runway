using System.Linq;
using Xunit;
using FluentAssertions;
using Runway.ViewModels;

namespace Runway.UnitTests.ViewModels
{
   public class BulkObservableCollectionTests
   {
      [Fact]
      public void Reset_ResettingToOneItem_OnlyReceivesOneNotification()
      {
         // Act

         var collection = new BulkObservableCollection<int>();

         int eventsRaised = 0;
         collection.CollectionChanged += ( sender, e ) => eventsRaised++;

         collection.Reset( Enumerable.Range( 1, 1 ) );

         // Assert

         eventsRaised.Should().Be( 1 );
      }
   }
}
