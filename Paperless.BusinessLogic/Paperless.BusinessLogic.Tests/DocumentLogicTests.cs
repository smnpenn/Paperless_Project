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
using Paperless.BusinessLogic.Exceptions;
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
            _repo.Setup(r => r.GetDocumentById(It.IsAny<Int64>())).Returns((Paperless.DAL.Entities.Document)null);
            _mapper.Setup(m => m.Map<DAL.Entities.Document, Document>(_repo.Object.GetDocumentById(doc.Id))).Returns((Document)null);

            var res = documentLogic.GetDocumentById(doc.Id);

            Assert.IsNull(res);
        }

        [Test]
        public void GetDocuments_Test_ReturnsList()
        {
            ICollection<DAL.Entities.Document> docList = _fixture.CreateMany<DAL.Entities.Document>(4).ToList();
            ICollection<Document> blDocList = new List<Document>();

            foreach (DAL.Entities.Document doc in docList)
            {
                blDocList.Add(new Document
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
                    Tags = doc.Tags
                });
            }
            _repo.Setup(r => r.GetDocuments()).Returns(docList);
            _mapper.Setup(m => m.Map<ICollection<DAL.Entities.Document>, ICollection<Document>>(_repo.Object.GetDocuments())).Returns(blDocList);

            var retrievedList = documentLogic.GetDocuments();

            Assert.That(retrievedList.Count, Is.EqualTo(4));
        }

        [Test]
        public void GetDocumentMetadata_Test_ReturnNull()
        {
            DAL.Entities.Document doc = _fixture.Create<DAL.Entities.Document>();
            _repo.Setup(r => r.GetDocumentById(It.IsAny<Int64>())).Returns((Paperless.DAL.Entities.Document)null);
            _mapper.Setup(m => m.Map<DAL.Entities.Document, Document>(_repo.Object.GetDocumentById(doc.Id))).Returns((Document)null);

            var res = documentLogic.GetDocumentMetadata(doc.Id);

            Assert.IsNull(res);
        }

        [Test]
        public void GetDocumentMetadata_Test_ReturnsString()
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
            string expected = doc.Title + "\n" + "Tags: " + doc.Tags;
            _repo.Setup(r => r.GetDocumentById(doc.Id)).Returns(doc);
            _mapper.Setup(m => m.Map<DAL.Entities.Document, Document>(_repo.Object.GetDocumentById(doc.Id))).Returns(blDoc);

            var res = documentLogic.GetDocumentMetadata(doc.Id);

            Assert.That(res, Is.EqualTo(expected));
        }

        [Test]
        public void SaveDocument_Test_FailValidation()
        {
            DAL.Entities.Document doc = _fixture.Create<DAL.Entities.Document>();
            Document blDoc = new Document
            {
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
            _mapper.Setup(m => m.Map<DAL.Entities.Document, Document>(_repo.Object.Create(doc))).Returns(blDoc);
            Mock<Stream> mockStream = new Mock<Stream>();

            var ex = Assert.Throws<DocumentLogicException>(() => documentLogic.SaveDocument(blDoc, mockStream.Object));
            Assert.That(ex.Message, Is.EqualTo("failed to validate document"));

        }

        [Test]
        public void SaveDocument_Test_Success()
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
            _mapper.Setup(m => m.Map<Document>(_repo.Object.Create(doc))).Returns(blDoc);
            _mapper.Setup(m => m.Map<DAL.Entities.Document>(blDoc)).Returns(doc);
            Mock<Stream> mockStream = new Mock<Stream>();

            Assert.DoesNotThrow(() => documentLogic.SaveDocument(blDoc, mockStream.Object));
        }

        [Test]
        public void UploadDocument_Test_NotADocument()
        {
            _mapper.Setup(m => m.Map<DAL.Entities.Document, Document>(_repo.Object.GetDocumentById(5))).Returns((Document)null);

            var ex = Assert.Throws<DocumentLogicException>(() => documentLogic.UpdateDocument(5, null));
            Assert.That(ex.Message, Is.EqualTo("5 is not a document"));
        }

        [Test]
        public void UploadDocument_Test_FailValidation()
        {
            DAL.Entities.Document doc = _fixture.Create<DAL.Entities.Document>();
            Document blDoc = new Document
            {
                Id = doc.Id,
                Added = doc.Added,
                Created = doc.Created,
                Correspondent = doc.Correspondent,
                Modified = doc.Modified
            };
            _repo.Setup(r => r.GetDocumentById(doc.Id)).Returns(doc);
            _mapper.Setup(m => m.Map<DAL.Entities.Document, Document>(_repo.Object.GetDocumentById(doc.Id))).Returns(blDoc);

            var ex = Assert.Throws<DocumentLogicException>(() => documentLogic.UpdateDocument(doc.Id, blDoc));
            Assert.That(ex.Message, Is.EqualTo("invalid document"));
        }

        [Test]
        public void UploadDocument_Test_Success()
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

            Assert.DoesNotThrow(() => documentLogic.UpdateDocument(doc.Id, blDoc));
        }

        [Test]
        public void DeleteDocument_Test_ElasticFails()
        {
            Int64 id = _fixture.Create<Int64>();

            var res = documentLogic.DeleteDocument(id);

            Assert.That(!res);
        }

        [Test]
        public void DeleteDocument_Test_RepoFails()
        {
            Int64 id = _fixture.Create<Int64>();
            _elasticSearchServiceAgent.Setup(e => e.DeleteDocumentAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            _repo.Setup(r => r.DeleteDocument(It.IsAny<Int64>())).Returns(false);

            var res = documentLogic.DeleteDocument(id);

            Assert.That(!res);
        }

        [Test]
        public void DeleteDocument_Test_Success()
        {
            Int64 id = _fixture.Create<Int64>();
            _elasticSearchServiceAgent.Setup(e => e.DeleteDocumentAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            _repo.Setup(r => r.DeleteDocument(It.IsAny<Int64>())).Returns(true);

            var res = documentLogic.DeleteDocument(id);

            Assert.That(res);
        }

    }
}
