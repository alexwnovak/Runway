namespace Runway.ExtensibilityModel
{
   public interface ILaunchableCommand
   {
      string CommandText
      {
         get;
      }

      string Description
      {
         get;
      }

      void Launch();
   }
}
