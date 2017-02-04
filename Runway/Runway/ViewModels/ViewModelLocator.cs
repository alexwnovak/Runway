using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;

namespace Runway.ViewModels
{
   public class ViewModelLocator
   {
      public ViewModelLocator()
      {
         ServiceLocator.SetLocatorProvider( () => SimpleIoc.Default );
         SimpleIoc.Default.Register<MainViewModel>();
      }

      public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();
   }
}