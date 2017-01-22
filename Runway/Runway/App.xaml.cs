﻿using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Ioc;
using NHotkey;
using NHotkey.Wpf;
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

         SimpleIoc.Default.Register<ICommandCatalog>( () => commandCatalog );
         SimpleIoc.Default.Register<IAppService>( () => new AppService() );
      }

      private void OnLaunch( object sender, HotkeyEventArgs e ) => MainWindow.Activate();
   }
}
