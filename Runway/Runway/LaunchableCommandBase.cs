namespace Runway
{
   public abstract class LaunchableCommandBase : ILaunchableCommand
   {
      public string CommandText
      {
         get;
      }

      protected LaunchableCommandBase( string commandText )
      {
         CommandText = commandText;
      }

      public abstract void Launch( object[] parameters );
   }
}
