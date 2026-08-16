using MassTransit;

using Microsoft.Extensions.Logging;

namespace Infrastructure.RabbitMQ
{
    // O Consumer deveria ficar em uma API separada, mas pelo escopo do projeto, deixei aqui com o intuito de aprendizado
    public class SendBookingEmailConsumer : IConsumer<NewBookingEmail>
    {
        private readonly ILogger<SendBookingEmailConsumer> _logger;

        public SendBookingEmailConsumer(ILogger<SendBookingEmailConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<NewBookingEmail> context)
        {
            var message = context.Message;

            _logger.LogInformation($"Processando email para: {message.ReceiverEmail}");

            _logger.LogInformation($"Email de lembrete de reserva para {message.ReceiverEmail} enviado com sucesso!");
        }
    }
}
