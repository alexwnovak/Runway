namespace Runway.Commands.Uninstall
{
   public interface IAppSearchCatalog : ISearchCatalog
   {
      void Uninstall( string path );
   }
}
