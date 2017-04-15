using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Runway.ExtensibilityModel;

namespace Runway.Commands.Stop
{
   public class ProcessSearchCatalog : ISearchCatalog
   {
      public async Task<IMatchResult[]> Search( string searchText )
      {
         var results = new List<IMatchResult>();

         var allProcesses = Process.GetProcesses()
                                    //.Where( p => !string.IsNullOrEmpty( p.MainWindowTitle ) )
                                    .OrderBy( p => p.ProcessName );

         foreach ( var process in allProcesses )
         {
            var command = new StopSubCommand( "stop " + process.MainWindowTitle, process );
            results.Add( new MatchResult( MatchType.Exact, command ) );
         }

         await Task.Delay( 2000 );

         return results.ToArray();
      }
   }
}
