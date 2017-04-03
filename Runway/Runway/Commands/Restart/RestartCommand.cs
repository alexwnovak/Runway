using System;

namespace Runway.Commands.Restart
{
   public class RestartCommand : LaunchableCommandBase
   {
      public RestartCommand( string commandText )
         : base( commandText )
      {
      }

      public override void Launch( object[] parameters )
      {
         throw new NotImplementedException();
      }
   }
}
