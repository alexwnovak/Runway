using System.Diagnostics;

namespace Runway.Commands.Uninstall
{
   public class ProcessAdapter : IProcess
   {
      public void Start( string path ) => Process.Start( path );
   }
}
