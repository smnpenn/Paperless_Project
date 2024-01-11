namespace Paperless.DAL.Entities
{
    public class Document
    {
        public Int64 Id { get; set; }
        public Int64? Correspondent { get; set; }
        public Int64? DocumentType { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public List<Int64>? Tags { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Added { get; set; }
        public string? Path { get; set; }
    }
}
