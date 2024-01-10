using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Paperless.DAL.Entities;
using Paperless.DAL.Interfaces;

namespace Paperless.DAL.Sql
{
    public class CorrespondentRepository : DbContext, ICorrespondentRepository
    {
        readonly string _contextString;
        readonly IConfiguration _config;

        public DbSet<Correspondent> Correspondents { get; set; }

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

        public void Create(Correspondent entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Correspondent entity)
        {
            throw new NotImplementedException();
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
    }
}
