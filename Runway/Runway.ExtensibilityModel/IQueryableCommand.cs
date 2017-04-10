namespace Runway.ExtensibilityModel
{
   public interface IQueryableCommand : ILaunchableCommand
   {
      ISearchCatalog QueryResults();
   }
}
