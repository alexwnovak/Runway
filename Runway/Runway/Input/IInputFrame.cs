using Runway.ExtensibilityModel;

namespace Runway.Input
{
   public interface IInputFrame
   {
      IMatchResult[] Match( string searchText );
   }
}
