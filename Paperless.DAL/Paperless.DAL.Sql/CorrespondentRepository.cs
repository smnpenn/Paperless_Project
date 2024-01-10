using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Paperless.DAL.Entities;
using Paperless.DAL.Interfaces;

namespace Paperless.DAL.Sql
{
    public class CorrespondentRepository : DbContext, ICorrespondentRepository, IDocTagRepository
    {
        readonly string _contextString;
        readonly IConfiguration _config;

        public DbSet<Correspondent> Correspondents { get; set; }
        public DbSet<DocTag> DocTags { get; set; }

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

        public Correspondent GetCorrespondentById(Int64 id)
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
    }
}
