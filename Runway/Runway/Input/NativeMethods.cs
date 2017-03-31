using System;
using System.Runtime.InteropServices;

namespace Runway.Input
{
   internal delegate IntPtr HookHandlerDelegate( int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam );

   internal static class NativeMethods
   {
      public const int WH_KEYBOARD_LL = 13;

      [DllImport( "kernel32.dll", CharSet = CharSet.Auto, SetLastError = true )]
      public static extern IntPtr GetModuleHandle( string lpModuleName );

      [DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
      public static extern IntPtr SetWindowsHookEx( int idHook, HookHandlerDelegate lpfn, IntPtr hMod, uint dwThreadId );

      [DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
      [return: MarshalAs( UnmanagedType.Bool )]
      public static extern bool UnhookWindowsHookEx( IntPtr hhk );

      [DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
      public static extern IntPtr CallNextHookEx( IntPtr hhk, int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam );

      [DllImport( "user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi )]
      public static extern short GetKeyState( int keyCode );
   }
}
