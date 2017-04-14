using System;

namespace Runway.Commands
{
   public class QuitCommand : LaunchableCommandBase
   {
      public QuitCommand()
         : base( "quit", "Quits the Runway application" )
      {
      }

      public override void Launch() => Environment.Exit( 0 );
   }
}
