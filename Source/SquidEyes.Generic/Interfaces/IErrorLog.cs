using System;
using System.Collections.Generic;

namespace SquidEyes.Generic
{
    public interface IErrorLog
    {
        void AddError(Exception error, 
            IDictionary<string, string> context = null);
    }
}
