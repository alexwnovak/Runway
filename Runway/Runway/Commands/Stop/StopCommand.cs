using System;
using Runway.ExtensibilityModel;

namespace Runway.Commands.Stop
{
   public class StopCommand : IQueryableCommand
   {
      public string CommandText
      {
         get;
      }

      public string Description
      {
         get;
      }

      public void Launch()
      {
         throw new NotImplementedException();
      }

      public ISearchCatalog Query()
      {
         throw new NotImplementedException();
      }
   }
}
