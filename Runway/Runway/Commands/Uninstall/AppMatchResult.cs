using System;
using Runway.Input;

namespace Runway.Commands.Uninstall
{
   public class AppMatchResult : IMatchResult
   {
      public MatchType MatchType
      {
         get;
      }

      public string DisplayText
      {
         get;
      }

      public ISearchCatalog Source
      {
         get;
      }

      public AppMatchResult( string displayText )
      {
         DisplayText = displayText;
      }

      public IInputFrame BeginInputFrame()
      {
         throw new NotImplementedException();
      }

      public void Activate( object[] parameters )
      {
         throw new NotImplementedException();
      }
   }
}
