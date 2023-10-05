using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.BusinessLogic.Entities
{
    public class Correspondent
    {
        public Int64 Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Match { get; set; }
        public Int64 MatchingAlgorithm { get; set; }
        public bool IsInsensitive { get; set; }
        public Int64 DocumentCount { get; set; }
        public DateTime LastCorrespondence { get; set; }
    }
}
