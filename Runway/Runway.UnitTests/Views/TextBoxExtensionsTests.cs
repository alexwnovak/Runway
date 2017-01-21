using System.Windows.Controls;
using Xunit;
using FluentAssertions;
using Runway.ViewModels;
using Runway.Views;

namespace Runway.UnitTests.Views
{
   public class TextBoxExtensionsTests
   {
      [StaFact]
      public void MoveCaret_PassesStartPosition_SelectionStartShouldBe0()
      {
         // Arrange

         var textBox = new TextBox
         {
            Text = "Some text",
            SelectionStart = 2
         };

         // Act

         textBox.MoveCaret( CaretPosition.Start );

         // Assert

         textBox.SelectionStart.Should().Be( 0 );
      }

      [StaFact]
      public void MoveCaret_PassesEndPosition_SelectionStartShouldBeAtTheEnd()
      {
         // Arrange

         var textBox = new TextBox
         {
            Text = "Some text",
            SelectionStart = 2
         };

         // Act

         textBox.MoveCaret( CaretPosition.End );

         // Assert

         textBox.SelectionStart.Should().Be( textBox.Text.Length );
      }
   }
}
