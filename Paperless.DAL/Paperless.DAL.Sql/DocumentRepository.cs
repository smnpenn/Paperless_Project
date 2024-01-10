using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Paperless.DAL.Entities;
using Paperless.DAL.Interfaces;

namespace Paperless.DAL.Sql
{
    public class DocumentRepository : DbContext, IDocumentRepository
    {
        readonly string _contextString;
        readonly IConfiguration _config;

        public DocumentRepository(IConfiguration configuration, string contextString)
        {
            _config = configuration;
            _contextString = contextString;
            //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Document> Documents { get; set; }

        public void PopulateWithSampleData()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();

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

        public Document Create(Document entity)
        {
            Documents.Add(entity);
            SaveChanges();
            return entity;
        }

        public int DeleteDocument(Int64 id)
        {
            Document? doc = GetDocumentById(id);

            if(doc != null)
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
            Document? doc = GetDocumentById(id);
            if (doc != null)
            {
                entity.Id = id;
                Documents.Remove(doc);
                Documents.Add(entity);
                SaveChanges();
                return entity;
            }
            return null;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_config.GetConnectionString(_contextString));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}
