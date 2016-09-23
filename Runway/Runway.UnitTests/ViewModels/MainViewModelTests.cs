using FluentAssertions;
using Moq;
using Runway.ViewModels;

namespace Runway.UnitTests.ViewModels
{
   public class MainViewModelTests
   {
      public void CurrentCommandText_PrefixMatchesCommand_ResolvesPreviewText()
      {
         const string partialCommand = "c";
         const string commandName = "copy";

         // Arrange

         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( partialCommand ) ).Returns( commandName );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object )
         {
            CurrentCommandText = partialCommand
         };

         // Assert

         viewModel.PreviewCommandText.Should().Be( "opy" );
      }

      public void CurrentCommandText_PrefixIsNull_PreviewTextIsNull()
      {
         // Arrange

         var commandCatalogMock = new Mock<ICommandCatalog>();
         commandCatalogMock.Setup( cc => cc.Resolve( null ) ).Returns<string>( null );

         // Act

         var viewModel = new MainViewModel( commandCatalogMock.Object )
         {
            CurrentCommandText = null
         };

         // Assert

         viewModel.PreviewCommandText.Should().BeNull();

      }
   }
}
