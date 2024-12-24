namespace RozhnovBack.Models
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        // Метод для проверки текущих бронирований
        public List<Reservation> GetActiveReservations()
        {
            return Reservations.Where(r => r.CheckOutDate > DateTime.Now).ToList();
        }
    }
}
