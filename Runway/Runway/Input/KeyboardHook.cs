using System;
using System.Diagnostics;

namespace Runway.Input
{
   public sealed class KeyboardHook : IDisposable
   {
      private readonly HookHandlerDelegate _hookHandler;
      private readonly IntPtr _hookId;

      public event EventHandler<KeyboardHookEventArgs> KeyIntercepted;

      public KeyboardHook()
      {
         _hookHandler = HookCallback;

         using ( var process = Process.GetCurrentProcess() )
         {
            using ( ProcessModule module = process.MainModule )
            {
               _hookId = NativeMethods.SetWindowsHookEx( NativeMethods.WH_KEYBOARD_LL, _hookHandler, NativeMethods.GetModuleHandle( module.ModuleName ), 0 );
            }
         }
      }

      private IntPtr HookCallback( int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam )
      {
         var e = new KeyboardHookEventArgs( lParam.vkCode );
         OnKeyIntercepted( this, e );

         if ( e.Handled )
         {
            return (IntPtr) 1;
         }

         return NativeMethods.CallNextHookEx( _hookId, nCode, wParam, ref lParam );
      }

      private void OnKeyIntercepted( object sender, KeyboardHookEventArgs e )
         => KeyIntercepted?.Invoke( sender, e );

      public void Dispose()
      {
         NativeMethods.UnhookWindowsHookEx( _hookId );
      }
   }
}
