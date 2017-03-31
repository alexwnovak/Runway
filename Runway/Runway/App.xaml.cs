using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using Runway.Commands.Uninstall;
using Runway.Extensions;
using Runway.Input;
using Runway.Services;

namespace Runway
{
   public partial class App : Application
   {
      private const int _leftWinKey = 91;
      private const int _rightWinKey = 92;

      protected override void OnStartup( StartupEventArgs e )
      {
         base.OnStartup( e );

         WireDependencies();

         var keyboardHook = new KeyboardHook();
         keyboardHook.KeyIntercepted += ( _, keyArgs ) =>
         {
            if ( keyArgs.KeyCode == _leftWinKey || keyArgs.KeyCode == _rightWinKey )
            {
               keyArgs.Handled = true;
               Launch();
            }
         };
      }

      private void WireDependencies()
      {
         var commandCatalog = new CommandCatalog();
         commandCatalog.Add( new CopyLaunchCommand() );

         SimpleIoc.Default.Register<ISearchCatalog>( () => commandCatalog );
         SimpleIoc.Default.Register<IAppService>( () => new AppService() );

         var inputController = new InputController( new InputFrame( commandCatalog ) );
         SimpleIoc.Default.Register<IInputController>( () => inputController );

         SimpleIoc.Default.Register<IAppCatalog, AppCatalog>();
         SimpleIoc.Default.Register<IRegistry, RegistryAdapter>();
         SimpleIoc.Default.Register<IProcess, ProcessAdapter>();
         SimpleIoc.Default.Register<UninstallCommand>();

         commandCatalog.Add( SimpleIoc.Default.GetInstance<UninstallCommand>() );
      }

      private void Launch()
      {
         if ( !MainWindow.IsActive )
         {
            MainWindow.Visibility = Visibility.Visible;
            MainWindow.FadeIn();
         }
      }
   }
}
