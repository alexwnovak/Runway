namespace Runway.Extensions
{
   public static class Int32Extensions
   {
      public static int Increment( this int value, int max ) => ++value >= max ? 0 : value;
      public static int Decrement( this int value, int reset ) => --value < 0 ? reset : value;
   }
}
