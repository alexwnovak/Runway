using System;
using System.Collections.Generic;
using System.Linq;

namespace Runway
{
   public class CommandCatalog : ICommandCatalog
   {
      private static readonly List<ILaunchableCommand> _commandList = new List<ILaunchableCommand>();

      private class NullCommand : ILaunchableCommand
      {
         public string CommandText => string.Empty;

         public void Launch( object[] parameters )
         {
         }
      }

      public static readonly ILaunchableCommand MissingCommand = new NullCommand();

      public void Add( ILaunchableCommand command ) => _commandList.Add( command );

      public MatchResult[] Resolve( string searchText )
      {
         if ( string.IsNullOrEmpty( searchText ) )
         {
            return new MatchResult[0];
         }

         return _commandList.Where( c => c.CommandText.StartsWith( searchText, StringComparison.InvariantCultureIgnoreCase ) )
               .Select( c => new MatchResult( MatchType.Exact, c )  )
               .ToArray();
      }

      //public ILaunchableCommand Resolve( string commandPartialText )
      //{
      //   if ( string.IsNullOrEmpty( commandPartialText ) )
      //   {
      //      return MissingCommand;
      //   }

      //   var commandMatch = _commandList.FirstOrDefault( c => c.CommandText.StartsWith( commandPartialText, StringComparison.InvariantCultureIgnoreCase ) );

      //   if ( commandMatch == null )
      //   {
      //      return MissingCommand;
      //   }

      //   return commandMatch;
      //}
   }
}
