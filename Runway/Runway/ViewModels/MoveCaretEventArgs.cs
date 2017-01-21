using System;

namespace Runway.ViewModels
{
   public class MoveCaretEventArgs : EventArgs
   {
      public CaretPosition CaretPosition
      {
         get;
      }

      public MoveCaretEventArgs( CaretPosition caretPosition )
      {
         CaretPosition = caretPosition;
      }
   }
}
