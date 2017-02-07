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

      void Activate( object[] parameters );
   }
}
