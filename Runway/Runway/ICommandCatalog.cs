namespace Runway
{
   public interface ICommandCatalog
   {
      ILaunchableCommand Resolve( string commandPartialText );
   }
}
