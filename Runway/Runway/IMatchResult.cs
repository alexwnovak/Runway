﻿namespace Runway
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

      void Activate( object[] parameters );
   }
}