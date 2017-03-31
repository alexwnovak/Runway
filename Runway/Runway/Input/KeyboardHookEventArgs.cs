using System;

namespace Runway.Input
{
   public class KeyboardHookEventArgs : EventArgs
   {
      public int KeyCode
      {
         get;
      }

      public bool Handled
      {
         get;
         set;
      }

      public KeyboardHookEventArgs( int keyCode )
      {
         KeyCode = keyCode;
      }
   }
}
