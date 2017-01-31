namespace Runway.Commands.Uninstall
{
   public class AppEntry
   {
      public string Name
      {
         get;
      }

      public string Path
      {
         get;
      }

      public AppEntry( string name, string path )
      {
         Name = name;
         Path = path;
      }
   }
}
