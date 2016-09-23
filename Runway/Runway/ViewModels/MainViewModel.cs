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
            bool changed = Set( () => CurrentCommandText, ref _currentCommandText, value );

            if ( changed )
            {
               CommandTextChanged( value );
            }
         }
      }

      private void CommandTextChanged( string newText )
      {
         
      }
   }
}
