namespace Runway
{
   public abstract class LaunchableCommandBase : ILaunchableCommand
   {
      public string CommandText
      {
         get;
      }

      public string Description
      {
         get;
      }

      protected LaunchableCommandBase( string commandText, string description )
      {
         CommandText = commandText;
         Description = description;
      }

      public abstract void Launch( object[] parameters );
   }
}
