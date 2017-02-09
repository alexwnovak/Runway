using System;
using System.Collections.Generic;
using System.Linq;

namespace Runway.Commands.Uninstall
{
   public class AppSearchCatalog : IAppSearchCatalog
   {
      private readonly IRegistry _registry;

      public AppSearchCatalog( IRegistry registry )
      {
         _registry = registry;
      }

      public IMatchResult[] Search( string searchText )
      {
         throw new System.NotImplementedException();
      }

      public void Uninstall( string path )
      {
         throw new NotImplementedException();
      }
   }
}
