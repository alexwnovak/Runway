using Runway.ExtensibilityModel;

namespace Runway.Commands.Stop
{
   public class StopCommand : IQueryableCommand
   {
      public string CommandText => "stop";
      public string Description => "Stops Window processes";

      public void Launch()
      {
      }

      public ISearchCatalog Query() => new ProcessSearchCatalog();
   }
}
