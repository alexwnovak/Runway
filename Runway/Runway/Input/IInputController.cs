using System.Threading.Tasks;
using Runway.ExtensibilityModel;

namespace Runway.Input
{
   public interface IInputController
   {
      IInputFrame CurrentInputFrame
      {
         get;
      }

      Task<IMatchResult[]> UpdateInputText( string inputText );
   }
}
