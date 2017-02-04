namespace Runway
{
   public interface ISearchCatalog
   {
      IMatchResult[] Resolve( string searchText );
   }
}
