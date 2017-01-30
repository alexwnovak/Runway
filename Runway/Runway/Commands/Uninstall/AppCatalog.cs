namespace Runway.Commands.Uninstall
{
   public class AppCatalog : IAppCatalog
   {
      public static AppEntry[] EmptyResults = new AppEntry[0];

      public AppEntry[] Find( string name )
      {
         return EmptyResults;
      }
   }
}
