namespace RozhnovBack.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalCost => CalculateTotalCost();

        // Метод для подсчета общей стоимости проживания
        private decimal CalculateTotalCost()
        {
            int numberOfNights = (CheckOutDate - CheckInDate).Days;
            return numberOfNights * RoomPricePerNight;
        }

        public decimal RoomPricePerNight { get; set; } // Цена номера за ночь
    }
}
