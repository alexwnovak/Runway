using System.Threading.Tasks;
using Runway.ExtensibilityModel;

namespace Runway.Input
{
   public interface IInputFrame
   {
      Task<IMatchResult[]> Match( string searchText );
   }
}
