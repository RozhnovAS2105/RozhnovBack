using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RozhnovBack.Data;
using RozhnovBack.Models;

namespace RozhnovBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly RozhnovBackContext _context;

        /*        private static List<Room> Rooms = new List<Room>
                {
                    new Room{ Id=1, Type = "Одноместный", PricePerNight = 150, IsOccupied = false },
                    new Room{ Id=2, Type = "Двухместный", PricePerNight = 150, IsOccupied = true },
                };*/

        [HttpGet]
        [Authorize]
        public IActionResult GetAllRooms()
        {
            return Ok(RoomService.Rooms);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetRoomById(int id)
        {
            var room = RoomService.Rooms.FirstOrDefault(r => r.Id == id);
            return room != null ? Ok(room) : NotFound("Room not found");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult CreateRoom([FromBody] Room room)
        {
            room.Id = RoomService.Rooms.Max(r => r.Id) + 1;
            RoomService.Rooms.Add(room);
            return CreatedAtAction(nameof(GetRoomById), new { id = room.Id }, room);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateRoom(int id, [FromBody] Room updatedRoom)
        {
            var room = RoomService.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound("Room not found");

            room.Type = updatedRoom.Type;
            room.PricePerNight = updatedRoom.PricePerNight;
            room.ChangeOccupancyStatus(updatedRoom.IsOccupied);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteRoom(int id)
        {
            var room = RoomService.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound("Room not found");

            RoomService.Rooms.Remove(room);
            return NoContent();
        }

        [HttpGet("search")]
        [Authorize]
        public IActionResult SearchRooms([FromQuery] string? type,
            [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice,
            [FromQuery] bool? isOccupied,
            [FromQuery] DateTime? bookingStartDate,
            [FromQuery] DateTime? bookingEndDate
            )
        {
            // Фильтрация по типу комнаты
            var roomsQuery = RoomService.Rooms.AsQueryable();

            // Применение фильтрации по типу, если передан параметр
            if (!string.IsNullOrEmpty(type))
            {
                roomsQuery = roomsQuery.Where(r => r.Type.Contains(type, StringComparison.OrdinalIgnoreCase));
            }

            if (minPrice.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.PricePerNight >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.PricePerNight <= maxPrice.Value);
            }

            if (isOccupied.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.IsOccupied == isOccupied.Value);
            }

            if (bookingStartDate.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.BookingStartDate >= bookingStartDate.Value);
            }

            if (bookingEndDate.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.BookingEndDate <= bookingEndDate.Value);
            }

            var rooms = roomsQuery.ToList();

            return rooms.Any() ? Ok(rooms) : NotFound("No rooms found with the specified criteria.");
        }

        /*        public RoomsController(RozhnovBackContext context)
                {
                    _context = context;
                }

                // GET: api/Rooms
                [HttpGet]
                public async Task<ActionResult<IEnumerable<Room>>> GetRoom()
                {
                  if (_context.Room == null)
                  {
                      return NotFound();
                  }
                    return await _context.Room.ToListAsync();
                }

                // GET: api/Rooms/5
                [HttpGet("{id}")]
                public async Task<ActionResult<Room>> GetRoom(int id)
                {
                  if (_context.Room == null)
                  {
                      return NotFound();
                  }
                    var room = await _context.Room.FindAsync(id);

                    if (room == null)
                    {
                        return NotFound();
                    }

                    return room;
                }

                // PUT: api/Rooms/5
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPut("{id}")]
                public async Task<IActionResult> PutRoom(int id, Room room)
                {
                    if (id != room.Id)
                    {
                        return BadRequest();
                    }

                    _context.Entry(room).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RoomExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return NoContent();
                }

                // POST: api/Rooms
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPost]
                public async Task<ActionResult<Room>> PostRoom(Room room)
                {
                  if (_context.Room == null)
                  {
                      return Problem("Entity set 'RozhnovBackContext.Room'  is null.");
                  }
                    _context.Room.Add(room);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetRoom", new { id = room.Id }, room);
                }*/

        // DELETE: api/Rooms/5
        /*        [HttpDelete("{id}")]
                public async Task<IActionResult> DeleteRoom(int id)
                {
                    if (_context.Room == null)
                    {
                        return NotFound();
                    }
                    var room = await _context.Room.FindAsync(id);
                    if (room == null)
                    {
                        return NotFound();
                    }

                    _context.Room.Remove(room);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }*/
        /*
                private bool RoomExists(int id)
                {
                    return (_context.Room?.Any(e => e.Id == id)).GetValueOrDefault();
                }*/
    }
}
