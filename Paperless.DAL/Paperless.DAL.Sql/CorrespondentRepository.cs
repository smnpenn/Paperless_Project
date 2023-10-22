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
        DbSet<Correspondent> correspondents;

        public CorrespondentRepository(IConfiguration configuration, string contextString)
        {
            _config = configuration;
            _contextString = contextString;
        }

        public void Create(Correspondent entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Correspondent entity)
        {
            throw new NotImplementedException();
        }

        public Correspondent GetCorrespondentById(Int64 id)
        {
            var correspondent = new Correspondent();
            correspondent.Name = "Test";
            correspondent.Id = id;
            correspondent.MatchingAlgorithm = 2;
            correspondent.IsInsensitive = true;
            correspondent.DocumentCount = 1;
            correspondent.LastCorrespondence = DateTime.Now;
            return correspondent;
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
    }
}
