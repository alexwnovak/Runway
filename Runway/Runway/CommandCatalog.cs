using System;
using System.Collections.Generic;
using System.Linq;
using Runway.ExtensibilityModel;

namespace Runway
{
   public class CommandCatalog : ISearchCatalog
   {
      private readonly List<ILaunchableCommand> _commandList = new List<ILaunchableCommand>();

      public static readonly IMatchResult[] EmptySet = new IMatchResult[0];

      public void Add( ILaunchableCommand command ) => _commandList.Add( command );

      public IMatchResult[] Search( string searchText )
      {
         if ( string.IsNullOrEmpty( searchText ) )
         {
            return EmptySet;
         }

         var results = from c in _commandList
                       let exactMatch = c.CommandText == searchText
                       let partialMatch = c.CommandText.StartsWith( searchText, StringComparison.InvariantCultureIgnoreCase )
                       let matchType = exactMatch ? MatchType.Exact : MatchType.Partial
                       where exactMatch || partialMatch
                       orderby c.CommandText
                       select new MatchResult( matchType, c );

         return results.ToArray();
      }
   }
}
