using System.Windows.Media;

namespace Runway
{
   public interface ILaunchableCommand
   {
      string CommandText
      {
         get;
      }

      string Description
      {
         get;
      }

      ImageSource Icon
      {
         get;
      }

      void Launch( object[] parameters );
   }
}
