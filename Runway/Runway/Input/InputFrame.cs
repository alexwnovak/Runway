using System;

namespace Runway.Input
{
   public class InputFrame : IInputFrame
   {
      private readonly ISearchCatalog _searchCatalog;

      public InputFrame( ISearchCatalog searchCatalog )
      {
         if ( searchCatalog == null )
         {
            throw new ArgumentNullException( nameof( searchCatalog ) );
         }

         _searchCatalog = searchCatalog;
      }

      public IMatchResult[] Match( string searchText ) => _searchCatalog.Search( searchText );
   }
}
