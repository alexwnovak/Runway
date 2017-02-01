using Xunit;
using FluentAssertions;
using Runway.Commands.Uninstall;

namespace Runway.UnitTests.Commands.Uninstall
{
   public class UninstallStringParserTests
   {
      [Fact]
      public void Parse_CommandHasNoArgumentsOrQuotes_GetsTheCommand()
      {
         const string command = @"C:\Program Files (x86)\Audacity\unins000.exe";

         var result = UninstallStringParser.Parse( command );

         result.Item1.Should().Be( command );
         result.Item2.Should().BeEmpty();
      }

      [Fact]
      public void Parse_CommandEnclosedInQuotesButHasNoArgument_GetsTheCommandWithNoQuotes()
      {
         const string command = @"C:\Program Files (x86)\Audacity\unins000.exe";
         string wholeLine = $@"""{command}""";

         var result = UninstallStringParser.Parse( wholeLine );

         result.Item1.Should().Be( command );
         result.Item2.Should().BeEmpty();
      }

      [Fact]
      public void Parse_CommandIsEnclosedInQuotesAndHasAnArgument_GetsTheCommandAndArgumentWithNoQuotes()
      {
         const string command = @"C:\Program Files (x86)\Dropbox\Client\DropboxUninstaller.exe";
         const string argument = "/InstallType:MACHINE";
         string wholeLine = $@"""{command}"" {argument}";

         var result = UninstallStringParser.Parse( wholeLine );

         result.Item1.Should().Be( command );
         result.Item2.Should().Be( argument );
      }
   }
}
