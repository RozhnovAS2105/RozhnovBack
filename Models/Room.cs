namespace RozhnovBack.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty; // Инициализация по умолчанию
        public decimal PricePerNight { get; set; }
        public bool IsOccupied { get; set; }
        public DateTime? BookingStartDate { get; set; }
        public DateTime? BookingEndDate { get; set; }

        public void ChangeOccupancyStatus(bool status)
        {
            IsOccupied = status;
        }
    }

}
