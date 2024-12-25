namespace RozhnovBack.Models
{
    public static class RoomService
    {
        public static List<Room> Rooms { get; } = new List<Room>
        {
            new Room { Id = 1, Type = "Одноместный", PricePerNight = 150, IsOccupied = false, BookingStartDate = DateTime.Now.AddDays(1), BookingEndDate = DateTime.Now.AddDays(3) },
            new Room { Id = 2, Type = "Двухместный", PricePerNight = 200, IsOccupied = true, BookingStartDate = DateTime.Now.AddDays(2), BookingEndDate = DateTime.Now.AddDays(4) }
        };
    }
}
