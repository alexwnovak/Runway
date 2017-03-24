using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Ioc;
using NHotkey;
using NHotkey.Wpf;
using Runway.Commands.Uninstall;
using Runway.Extensions;
using Runway.Input;
using Runway.Services;

namespace Runway
{
   public partial class App : Application
   {
      protected override void OnStartup( StartupEventArgs e )
      {
         base.OnStartup( e );

         WireDependencies();

         HotkeyManager.Current.AddOrReplace( "Launch", Key.R, ModifierKeys.Control | ModifierKeys.Shift, OnLaunch );
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

      private void OnLaunch( object sender, HotkeyEventArgs e )
      {
         if ( !MainWindow.IsActive )
         {
            MainWindow.Visibility = Visibility.Visible;
            MainWindow.FadeIn();
         }
      }
   }
}
