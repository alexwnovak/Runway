namespace Runway.ExtensibilityModel
{
   public interface ISearchCatalog
   {
      IMatchResult[] Search( string searchText );
   }
}
