using System.Text;

using RabbitMQ.Client;

namespace IntegrationTests
{
    public class RabbitMQIntegrationTests
    {
        [Fact]
        public async Task RabbitMQ_SendingMessage()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/"
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            string queue = "queueTest";

            await channel.QueueDeclareAsync(
                queue: queue,
                durable: false,
                exclusive: true,
                autoDelete: true
                );

            var message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync("", queue, body);

            BasicGetResult? result = await channel.BasicGetAsync(queue, true);
            Assert.NotNull(result);
            var queueMessage = Encoding.UTF8.GetString(result!.Body.ToArray());

            Assert.Equal(message, queueMessage);
        }
    }
}
