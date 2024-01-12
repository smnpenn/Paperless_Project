using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.ServiceAgents.Exceptions
{
    internal class MinIOServiceAgentException : Exception
    {
        public MinIOServiceAgentException(string message) : base(message) { }
        public MinIOServiceAgentException(string message, Exception innerException) : base(message, innerException) { }
    }
}
