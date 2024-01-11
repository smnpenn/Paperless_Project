using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Paperless.DAL.Entities;
using Paperless.DAL.Interfaces;

namespace Paperless.DAL.Sql
{
    public class CorrespondentRepository : DbContext, ICorrespondentRepository, IDocTagRepository, IDocumentRepository
    {
        readonly string _contextString;
        readonly IConfiguration _config;

        public DbSet<Correspondent> Correspondents { get; set; }
        public DbSet<DocTag> DocTags { get; set; }
        public DbSet<Document> Documents { get; set; }

        public CorrespondentRepository(IConfiguration configuration, string contextString)
        {
            _config = configuration;
            _contextString = contextString;
        }

        public void PopulateWithSampleData()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();

            for (int i = 0; i < 5; i++)
                Correspondents.Add(new Correspondent 
                {
                    Name = $"TestCorrespondent{i}",
                    MatchingAlgorithm = 2,
                    IsInsensitive = true,
                    DocumentCount = 1,
                    //LastCorrespondence = DateTime.Now // not working
                });

            for (int i = 0; i < 5; i++)
                Documents.Add(new Document
                {
                    Title = $"TestDocument{i}",
                    //Created = DateTime.Now,
                    Content = $"TestContent{i}",
                    Path = "C:/test/"
                    //LastCorrespondence = DateTime.Now // not working
                });

            SaveChanges();
        }

        public long? Create(Correspondent entity)
        {
            Correspondents.Add(entity);
            SaveChanges();
            return entity.Id;
        }

        public void Delete(Int64 id)
        {
            var temp = Correspondents.Find(id);
            Correspondents.Remove(temp);
            SaveChanges();
        }

        public Correspondent? GetCorrespondentById(Int64 id)
        {
            return Correspondents.Find(id);
        }

        public void Update(Correspondent entity)
        {
            throw new NotImplementedException();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_config.GetConnectionString(_contextString));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }

        public ICollection<Correspondent> GetCorrespondents()
        {
            return Correspondents.ToArray();
        }

        public DocTag GetDocTagById(long id)
        {
            return DocTags.Find(id);
        }

        public Int64 Create(DocTag entity)
        {
            DocTags.Add(entity);
            SaveChanges();
            return entity.Id;
        }

        public void Update(DocTag entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(DocTag entity)
        {
            var temp = DocTags.Find(entity.Id);
            DocTags.Remove(temp);
            SaveChanges();
        }

        public ICollection<DocTag> GetDocTags()
        {
            return DocTags.ToArray();
        }

        public Document Create(Document entity)
        {
            Documents.Add(entity);
            SaveChanges();
            return entity;
        }

        public int DeleteDocument(Int64 id)
        {
            Document? doc = GetDocumentById(id);

            if (doc != null)
            {
                Documents.Remove(doc);
                SaveChanges();
                return 0;
            }
            return -1;
        }

        public Document? GetDocumentById(Int64 id)
        {
            return Documents.Find(id);
        }

        public List<Document> GetDocuments()
        {
            return Documents.ToList();
        }

        public Document? Update(Int64 id, Document entity)
        {
            var existingDocument = GetDocumentById(id);
            if (existingDocument != null)
            {
                existingDocument.Title = entity.Title;
                existingDocument.Content = entity.Content;
                existingDocument.Modified = entity.Modified;
                existingDocument.Added = entity.Added;
                existingDocument.Correspondent = entity.Correspondent;
                existingDocument.DocumentType = entity.DocumentType;
                existingDocument.Tags = entity.Tags;
                existingDocument.Path = entity.Path;

                Documents.Update(existingDocument);
                SaveChanges();
                return existingDocument;
            }
            return null;
        }
    }
}
