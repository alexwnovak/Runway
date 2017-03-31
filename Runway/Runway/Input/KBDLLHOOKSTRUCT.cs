namespace Runway.Input
{
   internal struct KBDLLHOOKSTRUCT
   {
      public int vkCode;
      int scanCode;
      public int flags;
      int time;
      int dwExtraInfo;
   }
}
