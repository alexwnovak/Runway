namespace Runway.Commands.Uninstall
{
   public class AppSearchCatalog : ISearchCatalog
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
   }
}
