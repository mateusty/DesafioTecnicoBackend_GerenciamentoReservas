using MassTransit;

using Microsoft.Extensions.Logging;

namespace Infrastructure.RabbitMQ
{
    public class SendBookingEmailConsumer : IConsumer<SendBookingEmail>
    {
        private readonly ILogger<SendBookingEmailConsumer> _logger;

        public SendBookingEmailConsumer(ILogger<SendBookingEmailConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SendBookingEmail> context)
        {
            var message = context.Message;

            _logger.LogInformation($"Processando email para: {message.ReceiverEmail}");

            _logger.LogInformation($"Email de lembrete de reserva para {message.ReceiverEmail} enviado com sucesso!");
        }
    }
}
