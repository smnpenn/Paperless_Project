using System;
using System.Text;
using Paperless.BusinessLogic.Entities;
using Paperless.BusinessLogic.Interfaces;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace Paperless.BusinessLogic
{
	public class RabbitMQService : IRabbitMQService
	{
        private readonly IConnectionFactory _connectionFactory;
        private readonly string _queueName;

		public RabbitMQService(IConnectionFactory connectionFactory, string queueName)
		{
            _connectionFactory = connectionFactory;
            _queueName = queueName;
		}

        public Document RetrieveOCRJob()
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                BasicGetResult result = channel.BasicGet(_queueName, autoAck: true);
                if(result != null)
                {
                    var body = result.Body.ToArray();
                    var documentData = Encoding.UTF8.GetString(body);
                    var document = JsonConvert.DeserializeObject<Document>(documentData);
                    Console.WriteLine("Retrieved document from queue");
                    return document;
                }
                else
                {
                    Console.WriteLine("No document found in queue");
                    return null;
                }
            }
        }

        public void SendDocumentToQueue(Document document)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var documentData = JsonConvert.SerializeObject(document);
                var body = Encoding.UTF8.GetBytes(documentData);
              

                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
                Console.WriteLine("Sent document to queue");
            }
        }

    }
}

