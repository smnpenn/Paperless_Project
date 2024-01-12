using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Paperless.DAL.Entities;
using Paperless.DAL.Interfaces;
using Paperless.DAL.Sql.Exceptions;

namespace Paperless.DAL.Sql
{

    public class Repository : DbContext, ICorrespondentRepository, IDocTagRepository, IDocumentRepository, IDocumentTypeRepository
    {
        readonly string _contextString;
        readonly IConfiguration _config;

        public DbSet<Correspondent> Correspondents { get; set; }
        public DbSet<DocTag> DocTags { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }

        public Repository(IConfiguration configuration, string contextString)
        {
            _config = configuration;
            _contextString = contextString;
        }

        public void PopulateWithSampleData()
        {
            try
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
            catch (Exception ex) 
            {
                // log error
                throw new DALException("cant ensure the database is created. is the database running? is the connectionstring valid?", ex);
            }

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

            if (temp == null)
                throw new DALException("correspondent to be deleted was not found");

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

        public bool DeleteDocument(Int64 id)
        {
            Document? doc = GetDocumentById(id);

            if (doc == null) return false;

            if (doc.DocumentType != null)
                DecrementDocumentCount(doc.DocumentType);

            Documents.Remove(doc);
            SaveChanges();

            return true;
        }

        public Document? GetDocumentById(Int64 id)
        {
            return Documents.Find(id);
        }

        public ICollection<Document> GetDocuments()
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

        public DocumentType? GetDocumentTypeById(Int64? id)
        {
            return DocumentTypes.Find(id);
        }

        public ICollection<DocumentType> GetTypes()
        {
            return DocumentTypes.ToList();
        }

        public void CreateType(DocumentType entity)
        {
            DocumentTypes.Add(entity);
            SaveChanges();
        }

        public int UpdateType(Int64 id, DocumentType entity)
        {
            DocumentType? type = GetDocumentTypeById(id);
            if (type != null)
            {
                entity.Id = id;
                DocumentTypes.Remove(type);
                DocumentTypes.Add(entity);
                SaveChanges();
                return 0;
            }
            return -1;
        }

        public int DeleteType(Int64 id)
        {
            DocumentType? type = GetDocumentTypeById(id);

            if (type != null)
            {
                DocumentTypes.Remove(type);
                SaveChanges();
                return 0;
            }
            return -1;
        }

        public void IncrementDocumentCount(Int64? id)
        {
            DocumentType? type = GetDocumentTypeById(id);

            if(type != null)
            {
                type.DocumentCount++;
                SaveChanges();
            }
        }

        public void DecrementDocumentCount(Int64? id)
        {
            DocumentType? type = GetDocumentTypeById(id);

            if (type != null)
            {
                type.DocumentCount--;
                SaveChanges();
            }
        }
    }
}
