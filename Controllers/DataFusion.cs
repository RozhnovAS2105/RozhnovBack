using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RozhnovBack.Data;
using RozhnovBack.Models;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RozhnovBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataFusion : ControllerBase
    {
        private readonly RozhnovBackContext _context;

        public DataFusion(RozhnovBackContext context)
        {
            _context = context;
        }

        // Получение всех бронирований с деталями номеров (объединение Reservation и Room)
        [HttpGet("reservations-with-rooms")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetReservationsWithRoomDetails()
        {
            var result = from reservation in _context.Reservation
            //объединение двух таблиц, сопостовляя ID комнат.
                         join room in _context.Room on reservation.RoomId equals room.Id //encude
                         select new
                         {
                             ReservationId = reservation.Id,
                             GuestId = reservation.GuestId,
                             RoomType = room.Type,
                             PricePerNight = room.PricePerNight,
                             CheckInDate = reservation.CheckInDate,
                             CheckOutDate = reservation.CheckOutDate
                         };
            var resultList = await result.ToListAsync();
            // Проверяем, если результат пуст, возвращаем NotFound с сообщением
            if (!resultList.Any())
            {
                return NotFound("No reservations found.");
            }

            // Если данные есть, возвращаем их как успешный ответ
            return Ok(resultList);


            //return Ok(await result.ToListAsync());
        }

        // Получение всех доступных номеров
        [HttpGet("available-rooms")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAvailableRooms()
        {
            var availableRooms = from room in _context.Room
                                 where !room.IsOccupied
                                 select new
                                 {
                                     RoomId = room.Id,
                                     RoomType = room.Type,
                                     PricePerNight = room.PricePerNight
                                 };

            return Ok(await availableRooms.ToListAsync());
        }

        // Получение всех бронирований для конкретного гостя с деталями номеров
        [HttpGet("guest/{guestId}/reservations")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetGuestReservations(int guestId)
        {
            var guestReservations = from reservation in _context.Reservation
                                    join room in _context.Room on reservation.RoomId equals room.Id
                                    where reservation.GuestId == guestId
                                    select new
                                    {
                                        ReservationId = reservation.Id,
                                        RoomType = room.Type,
                                        PricePerNight = room.PricePerNight,
                                        CheckInDate = reservation.CheckInDate,
                                        CheckOutDate = reservation.CheckOutDate
                                    };

            return Ok(await guestReservations.ToListAsync());
        }
    }
}
