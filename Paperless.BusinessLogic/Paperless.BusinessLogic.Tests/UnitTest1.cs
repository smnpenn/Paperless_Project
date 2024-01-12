using Moq;
using Newtonsoft.Json;
using Paperless.BusinessLogic;
using Paperless.BusinessLogic.Entities;
using RabbitMQ.Client;
using System.Text;

namespace Paperless.BusinessLogic.Tests
{
    public class Tests
    {

        private Mock<IModel> _mockModel;
        private Mock<IConnection> _mockConnection;
        private Mock<IConnectionFactory> _mockConnectionFactory;
        private RabbitMQService _rabbitMQService;

        [SetUp]
        public void Setup()
        {
            _mockModel = new Mock<IModel>();
            _mockConnection = new Mock<IConnection>();
            _mockConnectionFactory = new Mock<IConnectionFactory>();

            _mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(_mockConnection.Object);
            _mockConnection.Setup(c => c.CreateModel()).Returns(_mockModel.Object);

            _rabbitMQService = new RabbitMQService(_mockConnectionFactory.Object, "TestQueue");

        }

        [Test]
        public void SendDocumentToQueue_ShouldSendMessageToQueue()
        {
            var testDocument = new Document { Id = 1, Content = "test", Correspondent = 1, Created = DateTime.Now, Modified = DateTime.Now, Added = DateTime.Now, DocumentType = 2, Path = "C:/test", Title = "Test" };

            _rabbitMQService.SendDocumentToQueue(testDocument);

            _mockModel.Verify(m => m.BasicPublish(
                It.IsAny<string>(),
                "TestQueue",
                false,
                null,
                It.Is<ReadOnlyMemory<byte>>(body => VerifyDocumentBody(body, testDocument))
            ), Times.Once);
        }

        [Test]
        public void RetrieveOCRJob_ShouldRetrieveDocumentFromQueue()
        {
            var expectedDocument = new Document { Id = 1, Content = "test", Correspondent = 1, Created = DateTime.Now, Modified = DateTime.Now, Added = DateTime.Now, DocumentType = 2, Path = "C:/test", Title = "Test" };

            SetupMockBasicGet(expectedDocument);

            var result = _rabbitMQService.RetrieveOCRJob();

            Assert.AreEqual(expectedDocument.Id, result.Id );
        }

        [Test]
        public void RetrieveOCRJob_QueueEmpty_ShouldReturnNull()
        {
            _mockModel.Setup(m => m.BasicGet("TestQueue", true)).Returns((BasicGetResult)null);

            var result = _rabbitMQService.RetrieveOCRJob();
            Assert.IsNull(result);

        }

        [Test]
        public void RetrieveOCRJob_WhenExceptionOccurs_ShouldThrowException()
        {
            // Arrange
            _mockConnectionFactory.Setup(f => f.CreateConnection())
                                  .Throws(new Exception("Test exception"));

            // Act & Assert
            Assert.Throws<Exception>(() => _rabbitMQService.RetrieveOCRJob());
        }

        private void SetupMockBasicGet(Document document)
        {
            var fakeGetResult = new BasicGetResult(
                0, false, "", "TestQueue", 0, null,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(document))
            );
            _mockModel.Setup(m => m.BasicGet("TestQueue", true)).Returns(fakeGetResult);
        }


        private bool VerifyDocumentBody(ReadOnlyMemory<byte> body, Document expectedDocument)
        {
            var jsonString = Encoding.UTF8.GetString(body.ToArray());
            var document = JsonConvert.DeserializeObject<Document>(jsonString);
            return document != null &&
                document.Id == expectedDocument.Id &&
                document.Correspondent == expectedDocument.Correspondent &&
                document.DocumentType == expectedDocument.DocumentType &&
                document.Title == expectedDocument.Title &&
                document.Content == expectedDocument.Content &&
                document.Created == expectedDocument.Created &&
                document.Modified == expectedDocument.Modified &&
                document.Added == expectedDocument.Added &&
           true;
        }



    }
}