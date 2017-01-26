using System;
using System.Collections.Generic;
using System.Linq;

namespace Runway
{
   public class CommandCatalog : ICommandCatalog
   {
      private static readonly List<ILaunchableCommand> _commandList = new List<ILaunchableCommand>();

      public static readonly MatchResult[] EmptySet = new MatchResult[0];

      public void Add( ILaunchableCommand command ) => _commandList.Add( command );

      public MatchResult[] Resolve( string searchText )
      {
         if ( string.IsNullOrEmpty( searchText ) )
         {
            return EmptySet;
         }

         return _commandList.Where( c => c.CommandText.StartsWith( searchText, StringComparison.InvariantCultureIgnoreCase ) )
               .Select( c => new MatchResult( MatchType.Exact, c )  )
               .ToArray();
      }
   }
}
