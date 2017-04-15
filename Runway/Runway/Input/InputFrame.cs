using System;
using System.Threading.Tasks;
using Runway.ExtensibilityModel;

namespace Runway.Input
{
   public class InputFrame : IInputFrame
   {
      private readonly ISearchCatalog _searchCatalog;

      public InputFrame( ISearchCatalog searchCatalog )
      {
         _searchCatalog = searchCatalog ?? throw new ArgumentNullException( nameof( searchCatalog ) );
      }

      public Task<IMatchResult[]> Match( string searchText ) => _searchCatalog.Search( searchText );
   }
}
