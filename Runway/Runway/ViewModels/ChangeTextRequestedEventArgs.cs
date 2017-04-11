using System;

namespace Runway.ViewModels
{
   public class ChangeTextRequestedEventArgs : EventArgs
   {
      public string Text
      {
         get;
      }

      public ChangeTextRequestedEventArgs( string text )
      {
         Text = text;
      }
   }
}
