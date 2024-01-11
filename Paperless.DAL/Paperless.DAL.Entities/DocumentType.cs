namespace Paperless.DAL.Entities
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
