namespace Infrastructure.RabbitMQ
{
    public record NewBookingEmail
    {
        public string ReceiverEmail { get; init; } = string.Empty;
        public string HotelName { get; init; } = string.Empty;
        public string HotelAddress { get; init; } = string.Empty;
        public DateTime StartDate { get; init; } = DateTime.Now;
        public DateTime EndDate { get; init; } = DateTime.Now;
    }
}
