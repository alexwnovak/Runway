using System;

namespace Runway.Commands
{
   public class QuitCommand : LaunchableCommandBase
   {
      public QuitCommand() : base( "quit" )
      {
      }

      public override void Launch( object[] parameters ) => Environment.Exit( 0 );
   }
}
