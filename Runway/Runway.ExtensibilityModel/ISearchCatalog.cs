using System.Threading.Tasks;

namespace Runway.ExtensibilityModel
{
   public interface ISearchCatalog
   {
      Task<IMatchResult[]> Search( string searchText );
   }
}
