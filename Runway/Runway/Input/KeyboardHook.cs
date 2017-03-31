using System;
using System.Diagnostics;

namespace Runway.Input
{
   public sealed class KeyboardHook : IDisposable
   {
      private readonly HookHandlerDelegate _hookHandler;
      private readonly IntPtr _hookId;

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
         return NativeMethods.CallNextHookEx( _hookId, nCode, wParam, ref lParam );
      }

      public void Dispose()
      {
         NativeMethods.UnhookWindowsHookEx( _hookId );
      }
   }
}
