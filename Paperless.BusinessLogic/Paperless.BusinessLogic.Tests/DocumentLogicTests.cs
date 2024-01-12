using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Paperless.BusinessLogic.Interfaces;
using Paperless.BusinessLogic.Entities;
using AutoFixture;
using System.Runtime.CompilerServices;
using Paperless.DAL.Interfaces;
using AutoMapper;
using Paperless.ServiceAgents.Interfaces;
using Nest;

namespace Paperless.BusinessLogic.Tests
{
    public class DocumentLogicTests
    {
        private Fixture _fixture;
        private Mock<IDocumentRepository> _repo;
        private Mock<IMapper> _mapper;
        private Mock<IRabbitMQService> _rabbitmqService;
        private Mock<IMinIOServiceAgent> _minioService;
        private Mock<IElasticSearchServiceAgent> _elasticSearchServiceAgent;
        private DocumentLogic documentLogic;

        public DocumentLogicTests() 
        {
            _fixture = new Fixture();
            _repo = new Mock<IDocumentRepository>();
            _mapper = new Mock<IMapper>();
            _rabbitmqService = new Mock<IRabbitMQService>();
            _minioService = new Mock<IMinIOServiceAgent>();
            _elasticSearchServiceAgent = new Mock<IElasticSearchServiceAgent>();
            documentLogic = new DocumentLogic(_repo.Object, _mapper.Object, _rabbitmqService.Object, _minioService.Object, _elasticSearchServiceAgent.Object);
        } 

        [Test]
        public void GetDocumentById_Test_Valid()
        {
            DAL.Entities.Document doc = _fixture.Create<DAL.Entities.Document>();
            Document blDoc = new Document
            {
                Title = doc.Title,
                Id = doc.Id,
                Added = doc.Added,
                Created = doc.Created,
                Correspondent = doc.Correspondent,
                Modified = doc.Modified,
                Content = doc.Content,
                Path = doc.Path,
                DocumentType = doc.DocumentType,
                Tags = doc.Tags,
            };
            _repo.Setup(r => r.GetDocumentById(doc.Id)).Returns(doc);
            _mapper.Setup(m => m.Map<DAL.Entities.Document, Document>(_repo.Object.GetDocumentById(doc.Id))).Returns(blDoc);

            var res = documentLogic.GetDocumentById(doc.Id);

            Assert.That(res.Title, Is.EqualTo(doc.Title));    
        }

        [Test]
        public void GetDocumentById_Test_Invalid()
        {
            DAL.Entities.Document doc = _fixture.Create<DAL.Entities.Document>();
            _repo.Setup(r => r.GetDocumentById(doc.Id)).Returns((Paperless.DAL.Entities.Document)null);
            _mapper.Setup(m => m.Map<DAL.Entities.Document, Document>(_repo.Object.GetDocumentById(doc.Id))).Returns((Document)null);

            var res = documentLogic.GetDocumentById(doc.Id);

            Assert.IsNull(res);
        }

    }
}
