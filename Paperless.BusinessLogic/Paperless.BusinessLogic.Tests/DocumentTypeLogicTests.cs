using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Moq;
using Moq.Protected;
using Paperless.BusinessLogic.Entities;
using Paperless.DAL.Interfaces;

namespace Paperless.BusinessLogic.Tests
{
    public class DocumentTypeLogicTests
    {
        private Fixture _fixture;
        private Mock<IDocumentTypeRepository> _repo;
        private Mock<IMapper> _mapper;
        private DocumentTypeLogic _typeLogic;

        public DocumentTypeLogicTests()
        {
            _fixture = new Fixture();
            _repo = new Mock<IDocumentTypeRepository>();
            _mapper = new Mock<IMapper>();
            _typeLogic = new DocumentTypeLogic(_repo.Object, _mapper.Object);
        }

        [Test]
        public void CreateType_Test_Valid()
        {
            DocumentType type = new DocumentType 
            { 
                Match = "test",
                MatchingAlgorithm = 200
            };

            int res = _typeLogic.CreateType(type);

            Assert.That(res, Is.EqualTo(-1));
        }

        [Test]
        public void CreateType_Test_Invalid()
        {
            DocumentType type = _fixture.Create<DocumentType>();

            int res = _typeLogic.CreateType(type);

            Assert.That(res, Is.EqualTo(0));
        }

        [Test]
        public void GetTypes_Test_ReturnsList()
        {
            ICollection<DAL.Entities.DocumentType> typeList = _fixture.CreateMany<DAL.Entities.DocumentType>(4).ToList();
            ICollection<DocumentType> blTypeList = new List<DocumentType>();

            foreach (DAL.Entities.DocumentType type in typeList)
            {
                blTypeList.Add(new DocumentType
                {
                    DocumentCount = type.DocumentCount,
                    Id = type.Id,
                    Match = type.Match,
                    MatchingAlgorithm = type.MatchingAlgorithm,
                    Name = type.Name
                });
            }
            _repo.Setup(r => r.GetTypes()).Returns(typeList);
            _mapper.Setup(m => m.Map<ICollection<DAL.Entities.DocumentType>, ICollection<DocumentType>>(_repo.Object.GetTypes())).Returns(blTypeList);

            var retrievedList = _typeLogic.GetTypes();

            Assert.That(retrievedList.Count, Is.EqualTo(4));

        }

        [Test]
        public void UpdateType_Test_IsValid()
        {
            Int64 id = _fixture.Create<Int64>();
            DAL.Entities.DocumentType type = _fixture.Create<DAL.Entities.DocumentType>();
            DocumentType blType = new DocumentType
            {
                DocumentCount = type.DocumentCount,
                Id = type.Id,
                Match = type.Match,
                MatchingAlgorithm = type.MatchingAlgorithm,
                Name = type.Name
            };
            _repo.Setup(r => r.UpdateType(id, type)).Returns(0);

            int res = _typeLogic.UpdateType(id, blType);
            Assert.That(res, Is.EqualTo(0));
        }

        [Test]
        public void UpdateType_Test_IsInvalid()
        {
            Int64 id = _fixture.Create<Int64>();
            DAL.Entities.DocumentType type = _fixture.Create<DAL.Entities.DocumentType>();
            DocumentType blType = new DocumentType
            {
                DocumentCount = type.DocumentCount,
                Id = type.Id,
                Match = type.Match,
                MatchingAlgorithm = type.MatchingAlgorithm
            };
            _repo.Setup(r => r.UpdateType(id, type)).Returns(0);

            int res = _typeLogic.UpdateType(id, blType);
            Assert.That(res, Is.EqualTo(-1));
        }
    }
}
