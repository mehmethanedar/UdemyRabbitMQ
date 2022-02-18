using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace UdemyRabbitMQ.publisher
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

            //channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);//durable => uygulama restart olduğunda exchange'lerimiz kaybolmaması için
            channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);//durable => uygulama restart olduğunda exchange'lerimiz kaybolmaması için


            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                string message = $"log {x}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("logs-fanout", "", null, messageBody);

                Console.WriteLine($"Mesaj Gönderilmiştir : {message}");
            });

            Console.ReadLine();
        }
    }
}
