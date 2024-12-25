namespace RozhnovBack.Models
{
    public class ReserationService
    {
        // Статический список для хранения бронирований
        public static List<Reservation> Reservations { get; } = new List<Reservation>
        {
            // Пример данных для бронирования
            new Reservation { Id = 1, GuestId = 1, RoomId = 1, CheckInDate = DateTime.Now.AddDays(1), CheckOutDate = DateTime.Now.AddDays(3), RoomPricePerNight = 150 },
            new Reservation { Id = 2, GuestId = 2, RoomId = 2, CheckInDate = DateTime.Now.AddDays(2), CheckOutDate = DateTime.Now.AddDays(4), RoomPricePerNight = 200 }
        };
    }
}
