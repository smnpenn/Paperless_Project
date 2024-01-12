using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.ServiceAgents.Exceptions
{
    internal class ElasticSearchExceptions : Exception
    {
        public ElasticSearchExceptions(string message) : base(message) { }
        public ElasticSearchExceptions(string message, Exception innerException) : base(message, innerException) { }
    }
}
