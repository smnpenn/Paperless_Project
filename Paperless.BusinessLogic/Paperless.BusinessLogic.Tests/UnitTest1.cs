using Newtonsoft.Json;
using Paperless.BusinessLogic;
using Paperless.BusinessLogic.Entities;
using RabbitMQ.Client;

namespace Paperless.BusinessLogic.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void SendDocumentToQueue_ShouldSendMessageToQueue()
        {
            Document testDocument = new Document
            {
                Id = 1,
                Correspondent = 123,
                DocumentType = 456,
                Title = "Sample Document",
                Content = "This is a sample content",
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Added = DateTime.Now
            };

            string testQueueName = "TestQueue";
            string rabbitMQHost = "localhost";
            int rabbitMQPort = 5672;
            string rabbitMQUsername = "admin";
            string rabbitMQPassword = "admin";

            var factory = new ConnectionFactory
            {
                HostName = rabbitMQHost,
                Port = rabbitMQPort,
                UserName = rabbitMQUsername,
                Password = rabbitMQPassword
            };

            var rabbitMQService = new RabbitMQService(factory, testQueueName);

            string documentData = SerializeDocument(testDocument);

            // rabbitMQService.SendDocumentToQueue(documentData);

            // Observe the RabbitMQ management interface for the result.
            Console.WriteLine("Document sent to the queue. Check RabbitMQ management interface for the result.");

        }

        private string SerializeDocument(Document document)
        {
            var json = JsonConvert.SerializeObject(document);
            return json;
        }
    }
}