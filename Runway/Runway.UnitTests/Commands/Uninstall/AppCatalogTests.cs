//using Xunit;
//using FluentAssertions;
//using Moq;
//using Runway.Commands.Uninstall;
//using Runway.UnitTests.Helpers;

//namespace Runway.UnitTests.Commands.Uninstall
//{
//   public class AppCatalogTests
//   {
//      [Fact]
//      public void Find_CatalogIsEmpty_ReturnsNoResults()
//      {
//         // Arrange

//         var registryMock = new Mock<IRegistry>();

//         // Act

//         var appCatalog = new AppCatalog( registryMock.Object, null );

//         var results = appCatalog.Find( "notepad" );

//         // Assert

//         results.Should().HaveCount( 0 );
//      }

//      [Fact]
//      public void Find_SearchesForAppByName_FindsApp()
//      {
//         const string appName = "notepad";
//         const string path = "subKeyPath";
         
//         // Arrange

//         var registryResults = ArrayHelper.Create( path );

//         var registryMock = new Mock<IRegistry>();
//         registryMock.Setup( r => r.GetSubKeysFromLocalMachine( It.IsAny<string>() ) ).Returns( registryResults );

//         registryMock.Setup( r => r.GetValueFromLocalMachine( path, "DisplayName" ) ).Returns( appName );

//         // Act

//         var appCatalog = new AppCatalog( registryMock.Object, null );

//         var results = appCatalog.Find( appName );

//         // Assert

//         results.Should().HaveCount( 1 );
//         results[0].Name.Should().Be( appName );
//         results[0].Path.Should().EndWith( path );
//      }

//      [Fact]
//      public void Find_HasOneAppAndSearchesForSomethingElse_ReturnsAnEmptySet()
//      {
//         const string appName = "notepad";
//         const string path = "subKeyPath";

//         // Arrange

//         var registryResults = ArrayHelper.Create( path );

//         var registryMock = new Mock<IRegistry>();
//         registryMock.Setup( r => r.GetSubKeysFromLocalMachine( It.IsAny<string>() ) ).Returns( registryResults );

//         registryMock.Setup( r => r.GetValueFromLocalMachine( path, "DisplayName" ) ).Returns( appName );

//         // Act

//         var appCatalog = new AppCatalog( registryMock.Object, null );

//         var results = appCatalog.Find( "doesnotexist" );

//         // Assert

//         results.Should().HaveCount( 0 );
//      }

//      [Fact]
//      public void Find_HasTwoAppsAndSearchesWithPartialText_ReturnsBoth()
//      {
//         const string appName1 = "notepad";
//         const string path1 = "subKeyPath";

//         const string appName2 = "notepad++";
//         const string path2 = "subKeyPath2";

//         // Arrange

//         var registryResults = ArrayHelper.Create( path1, path2 );

//         var registryMock = new Mock<IRegistry>();
//         registryMock.Setup( r => r.GetSubKeysFromLocalMachine( It.IsAny<string>() ) ).Returns( registryResults );

//         registryMock.Setup( r => r.GetValueFromLocalMachine( path1, "DisplayName" ) ).Returns( appName1 );
//         registryMock.Setup( r => r.GetValueFromLocalMachine( path2, "DisplayName" ) ).Returns( appName2 );

//         // Act

//         var appCatalog = new AppCatalog( registryMock.Object, null );

//         var results = appCatalog.Find( "n" );

//         // Assert

//         results.Should().HaveCount( 2 );
//      }

//      [Fact]
//      public void Uninstall_PassesAppPath_StartsCorrespondingUninstallString()
//      {
//         const string path = "path";
//         const string uninstallCommand = @"C:\Program Files\Notepad++\Notepad++.exe";
//         const string uninstallArgument = "/uninstall";
//         string uninstallString = $"\"{uninstallCommand}\" {uninstallArgument}";

//         // Arrange

//         var processMock = new Mock<IProcess>();
         
//         var registryMock = new Mock<IRegistry>();
//         registryMock.Setup( r => r.GetValueFromLocalMachine( path, "UninstallString" ) ).Returns( uninstallString );

//         // Act

//         var appCatalog = new AppCatalog( registryMock.Object, processMock.Object );

//         appCatalog.Uninstall( path );

//         // Assert

//         processMock.Verify( p => p.Start( uninstallCommand, uninstallArgument ), Times.Once() );
//      }
//   }
//}
