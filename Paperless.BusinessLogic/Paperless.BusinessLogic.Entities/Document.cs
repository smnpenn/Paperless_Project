using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.BusinessLogic.Entities
{
    public class Document
    {
        public Int64 Id { get; set; }
        public Int64? Correspondent { get; set; }
        public Int64? DocumentType { get; set; }
        public Int64? StoragePath { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public List<Int64>? Tags { get; set; }
        public DateTime Created { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Added { get; set; }
        public string? ArchiveSerialNumber { get; set; }
        public string? OriginalFileName { get; set; }
        public string? ArchivedFileName { get; set; }
    }
}
