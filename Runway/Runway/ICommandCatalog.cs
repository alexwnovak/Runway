namespace Runway
{
   public interface ICommandCatalog
   {
      IMatchResult[] Resolve( string searchText );
   }
}
