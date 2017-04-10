using System;
using Runway.ExtensibilityModel;

namespace Runway
{
   public class FooCommand : IQueryableCommand
   {
      public string CommandText => "foo";
      public string Description => "Tests queryable commands that provide their own additional options";

      public void Launch( object[] parameters )
      {
      }

      public ISearchCatalog QueryResults() => new FooSearchCatalog( this );
   }

   public class FooSearchCatalog : ISearchCatalog
   {
      private readonly FooCommand _parent;

      public FooSearchCatalog( FooCommand parent )
      {
         _parent = parent;
      }

      public IMatchResult[] Search( string searchText )
      {
         return new[]
         {
            new MatchResult( MatchType.Partial, _parent ),
            new MatchResult( MatchType.Partial, _parent ), 
            new MatchResult( MatchType.Partial, _parent )
         };
      }
   }
}
