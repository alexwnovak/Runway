namespace Runway
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

      void Launch( object[] parameters );
   }
}
