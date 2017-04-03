namespace Runway
{
   public interface ILaunchableCommand
   {
      string CommandText
      {
         get;
      }

      ISearchCatalog ParameterSource
      {
         get;
      }

      void Launch( object[] parameters );
   }
}
