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
         const string uninstallString = "uninstall string";
         const string id = "AppId";
         
         // Arrange

         var registryResults = ArrayHelper.Create( id );

         var registryMock = new Mock<IRegistry>();
         registryMock.Setup( r => r.GetSubKeysFromLocalMachine( It.IsAny<string>() ) ).Returns( registryResults );

         var values = new Tuple<string, string>( appName, uninstallString );
         registryMock.Setup( r => r.GetValuesFromLocalMachine( id, "DisplayName", "UninstallString" ) ).Returns( values );

         // Act

         var appCatalog = new AppCatalog( registryMock.Object );

         var results = appCatalog.Find( appName );

         // Assert

         results.Should().HaveCount( 1 );
         results[0].Name.Should().Be( appName );
      }
   }
}
