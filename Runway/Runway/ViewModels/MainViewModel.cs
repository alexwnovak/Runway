using GalaSoft.MvvmLight;

namespace Runway.ViewModels
{
   public class MainViewModel : ViewModelBase
   {
      private string _currentCommandText;
      public string CurrentCommandText
      {
         get
         {
            return _currentCommandText;
         }
         set
         {
            Set( () => CurrentCommandText, ref _currentCommandText, value );
         }
      }
   }
}
