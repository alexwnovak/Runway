using Xunit;
using FluentAssertions;
using Runway.Commands.Uninstall;

namespace Runway.UnitTests.Commands.Uninstall
{
   public class AppCatalogTests
   {
      [Fact]
      public void Find_CatalogIsEmpty_ReturnsNoResults()
      {
         // Act

         var appCatalog = new AppCatalog();

         var results = appCatalog.Find( "notepad" );

         // Assert

         results.Should().HaveCount( 0 );
      }
   }
}
