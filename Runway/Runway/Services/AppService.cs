using System.Windows;

namespace Runway.Services
{
   public class AppService : IAppService
   {
      public void Exit() => Application.Current.Shutdown();
   }
}
