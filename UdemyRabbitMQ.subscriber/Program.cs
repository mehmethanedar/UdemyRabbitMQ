﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace UdemyRabbitMQ.subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            //channel.QueueDeclare("hello-queue", true, false, false);

            //channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout); //yazmasak da olur 

            var randomQueueName = channel.QueueDeclare().QueueName;


            //channel.QueueDeclare(randomQueueName, true, false, false);

            channel.QueueBind(randomQueueName, "logs-fanout", "", null);



            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(randomQueueName, false, consumer);

            Console.WriteLine("Loglar dinleniyor..");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Console.WriteLine("Gelen Mesaj:" + message);

                channel.BasicAck(e.DeliveryTag, false);
            };



            Console.ReadLine();
        }
    }
}
