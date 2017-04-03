namespace Runway
{
   public abstract class LaunchableCommandBase : ILaunchableCommand
   {
      public string CommandText
      {
         get;
      }

      public ISearchCatalog ParameterSource
      {
         get;
      }

      protected LaunchableCommandBase( string commandText, ISearchCatalog parameterSource )
      {
         CommandText = commandText;
         ParameterSource = parameterSource;
      }

      public abstract void Launch( object[] parameters );
   }
}
