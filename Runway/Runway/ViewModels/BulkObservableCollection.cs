using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Runway.ViewModels
{
   public class BulkObservableCollection<T> : ObservableCollection<T>
   {
      private bool _notificationsEnabled;

      protected override void OnCollectionChanged( NotifyCollectionChangedEventArgs e )
      {
         if ( _notificationsEnabled )
         {
            base.OnCollectionChanged( e );
         }
      }

      public void Reset( IEnumerable<T> items )
      {
         _notificationsEnabled = false;

         Clear();

         foreach ( var item in items )
         {
            Add( item );
         }

         _notificationsEnabled = true;
         OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
      }
   }
}
