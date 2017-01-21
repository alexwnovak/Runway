using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Ioc;
using NHotkey;
using NHotkey.Wpf;

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
         SimpleIoc.Default.Register<ICommandCatalog>( () => new CommandCatalog() );
      }

      private void OnLaunch( object sender, HotkeyEventArgs e ) => MainWindow.Activate();
   }
}
