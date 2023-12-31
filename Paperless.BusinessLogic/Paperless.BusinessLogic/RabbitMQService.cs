﻿using System;
using System.Text;
using Paperless.BusinessLogic.Entities;
using Paperless.BusinessLogic.Interfaces;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace Paperless.BusinessLogic
{
	public class RabbitMQService : IRabbitMQService
	{
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _queueName;

		public RabbitMQService(ConnectionFactory connectionFactory, string queueName)
		{
            _connectionFactory = connectionFactory;
            _queueName = queueName;
		}

        public void SendDocumentToQueue(Document document)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var documentData = JsonConvert.SerializeObject(document);
                var body = Encoding.UTF8.GetBytes(documentData);
              

                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
                Console.WriteLine("Sent document to queue");
            }
        }

        public void SendDocumentToQueue(DAL.Entities.Document document)
        {
            throw new NotImplementedException();
        }
    }
}

