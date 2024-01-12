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
    }
}
