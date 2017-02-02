namespace Runway.Commands.Uninstall
{
   public interface IAppCatalog
   {
      AppEntry[] Find( string name );

      void Uninstall( string path );
   }
}
