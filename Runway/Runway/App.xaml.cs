using System.Windows;
using GalaSoft.MvvmLight.Ioc;

namespace Runway
{
   public partial class App : Application
   {
      protected override void OnStartup( StartupEventArgs e )
      {
         base.OnStartup( e );

         WireDependencies();
      }

      private void WireDependencies()
      {
         SimpleIoc.Default.Register<ICommandCatalog>( () => new CommandCatalog() );
      }
   }
}
