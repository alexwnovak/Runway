namespace Runway
{
   public class CommandCatalog : ICommandCatalog
   {
      public string Resolve( string commandPartialText )
      {
         return "copy";
      }
   }
}
