using System;
using Xunit;
using FluentAssertions;
using Moq;
using Runway.Commands.Uninstall;
using Runway.UnitTests.Helpers;

namespace Runway.UnitTests.Commands.Uninstall
{
   public class AppCatalogTests
   {
      [Fact]
      public void Find_CatalogIsEmpty_ReturnsNoResults()
      {
         // Arrange

         var registryMock = new Mock<IRegistry>();

         // Act

         var appCatalog = new AppCatalog( registryMock.Object );

         var results = appCatalog.Find( "notepad" );

         // Assert

         results.Should().HaveCount( 0 );
      }

      [Fact]
      public void Find_SearchesForAppByName_FindsApp()
      {
         const string appName = "notepad";
         const string path = "subKeyPath";
         
         // Arrange

         var registryResults = ArrayHelper.Create( path );

         var registryMock = new Mock<IRegistry>();
         registryMock.Setup( r => r.GetSubKeysFromLocalMachine( It.IsAny<string>() ) ).Returns( registryResults );

         registryMock.Setup( r => r.GetValueFromLocalMachine( path, "DisplayName" ) ).Returns( appName );

         // Act

         var appCatalog = new AppCatalog( registryMock.Object );

         var results = appCatalog.Find( appName );

         // Assert

         results.Should().HaveCount( 1 );
         results[0].Name.Should().Be( appName );
         results[0].Path.Should().EndWith( path );
      }
   }
}
