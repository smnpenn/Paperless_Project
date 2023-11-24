using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.ServiceAgents.Interfaces
{
    public interface IOcrServiceAgent
    {
        string PerformOcrPdf(Stream pdfStream);
    }
}
