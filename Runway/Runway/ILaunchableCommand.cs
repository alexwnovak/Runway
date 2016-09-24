namespace Runway
{
   public interface ILaunchableCommand
   {
      string CommandText
      {
         get;
      }

      void Launch( object[] parameters );
   }
}
