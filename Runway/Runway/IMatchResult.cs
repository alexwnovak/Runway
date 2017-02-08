using Runway.Input;

namespace Runway
{
   public interface IMatchResult
   {
      MatchType MatchType
      {
         get;
      }

      string DisplayText
      {
         get;
      }

      ISearchCatalog Source
      {
         get;
      }

      IInputFrame BeginInputFrame();

      void Activate( object[] parameters );
   }
}
