namespace Runway.Input
{
   public class InputFrame : IInputFrame
   {
      private readonly ISearchCatalog _searchCatalog;

      public InputFrame( ISearchCatalog searchCatalog )
      {
         _searchCatalog = searchCatalog;
      }

      public IMatchResult[] Match( string searchText )
      {
         throw new System.NotImplementedException();
      }
   }
}
