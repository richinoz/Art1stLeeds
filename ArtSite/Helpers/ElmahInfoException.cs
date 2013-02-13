using System;

namespace ArtSite.Helpers
{
    public class ElmahInfoException:Exception
    {
         public ElmahInfoException(string message) : base(message)
        {
           
        }

         public ElmahInfoException(string message, Exception innerException)
             : base(message, innerException)
        {
            
        }
    }
}