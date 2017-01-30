namespace Runway.Commands.Uninstall
{
   public class AppEntry
   {
      public string Name
      {
         get;
      }

      public string Id
      {
         get;
      }

      public AppEntry( string name, string id )
      {
         Name = name;
         Id = id;
      }
   }
}
