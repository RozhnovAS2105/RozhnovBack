namespace RozhnovBack.Models
{
    public static class RoomService
    {
        public static List<Room> Rooms { get; } = new List<Room>
        {
            new Room { Id = 1, Type = "Одноместный", PricePerNight = 150, IsOccupied = false },
            new Room { Id = 2, Type = "Двухместный", PricePerNight = 200, IsOccupied = true }
        };
    }
}
