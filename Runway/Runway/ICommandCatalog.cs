namespace Runway
{
   public interface ICommandCatalog
   {
      string Resolve( string commandPartialText );
   }
}
