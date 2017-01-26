namespace Runway
{
   public interface ICommandCatalog
   {
      MatchResult[] Resolve( string searchText );
   }
}
