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
         throw new NotImplementedException();
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
