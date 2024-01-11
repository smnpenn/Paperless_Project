using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.BusinessLogic.Entities
{
    public class DocumentType
    {
        public Int64 Id { get; set; }
        public string? Name { get; set; }
        public string? Match { get; set; }
        public Int64 MatchingAlgorithm { get; set; }
        public Int64 DocumentCount { get; set; }
    }
}
