using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RozhnovBack.Data;
using RozhnovBack.Models;

namespace RozhnovBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReservationsController : ControllerBase
    {
        private readonly RozhnovBackContext _context;

        //private static List<Reservation> Reservations = new List<Reservation>();

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult GetAllReservations()
        {
            return Ok(ReserationService.Reservations);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetReservationById(int id)
        {
            var reservation = ReserationService.Reservations.FirstOrDefault(r => r.Id == id);
            return reservation != null ? Ok(reservation) : NotFound("Reservation not found");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult CreateReservation([FromBody] Reservation reservation)
        {
            // Прежде чем добавить, проверяем, что указан номер, который существует
            var room = RoomService.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
            if (room == null)
            {
                return BadRequest("Invalid room ID.");
            }

            reservation.Id = ReserationService.Reservations.Any() ? ReserationService.Reservations.Max(r => r.Id) + 1 : 1;
            ReserationService.Reservations.Add(reservation);
            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        }

        [HttpGet("guest/{guestId}")]
        [Authorize(Roles = "admin")]
        public IActionResult GetReservationsByGuest(int guestId)
        {
            var guestReservations = ReserationService.Reservations.Where(r => r.GuestId == guestId).ToList();
            return Ok(guestReservations);
        }

        /*        [HttpGet("available-rooms")]
                public IActionResult GetAvailableRooms()
                {
                    // Свободные номера (LINQ)
                    var availableRooms = RoomController.Rooms.Where(r => !r.IsOccupied).ToList();
                    return Ok(availableRooms);
                }*/

        [HttpGet("available-rooms")]
        [Authorize]
        public IActionResult GetAvailableRooms()
        {
            var availableRooms = RoomService.Rooms.Where(r => !r.IsOccupied).ToList();
            return Ok(availableRooms);
        }

        [HttpGet("search")]
        [Authorize]
        public IActionResult SearchReservations(
            [FromQuery] int? guestId,
            [FromQuery] int? roomId,
            [FromQuery] decimal? minCost,
            [FromQuery] decimal? maxCost,
            [FromQuery] DateTime? CheckInDate,
            [FromQuery] DateTime? CheckOutDate)
        {
            // Фильтрация по параметрам
            var reservationsQuery = ReserationService.Reservations.AsQueryable();

            // Фильтрация по RoomId, если передан параметр
            if (roomId.HasValue)
            {
                reservationsQuery = reservationsQuery.Where(r => r.RoomId == roomId.Value);
            }

            // Фильтрация по GuestId, если передан параметр
            if (guestId.HasValue)
            {
                reservationsQuery = reservationsQuery.Where(r => r.GuestId == guestId.Value);
            }

            // Фильтрация по дате заезда, если передан параметр
            if (CheckInDate.HasValue)
            {
                reservationsQuery = reservationsQuery.Where(r => r.CheckInDate >= CheckInDate.Value);
            }

            // Фильтрация по дате выезда, если передан параметр
            if (CheckOutDate.HasValue)
            {
                reservationsQuery = reservationsQuery.Where(r => r.CheckOutDate <= CheckOutDate.Value);
            }

            // Фильтрация по стоимости проживания, если передан параметр
            if (minCost.HasValue)
            {
                reservationsQuery = reservationsQuery.Where(r => r.TotalCost >= minCost.Value);
            }

            if (maxCost.HasValue)
            {
                reservationsQuery = reservationsQuery.Where(r => r.TotalCost <= maxCost.Value);
            }

            var reservations = reservationsQuery.ToList();

            return reservations.Any() ? Ok(reservations) : NotFound("No reservations found with the specified criteria.");
        }

        /*        public ReservationsController(RozhnovBackContext context)
                {
                    _context = context;
                }

                // GET: api/Reservations
                [HttpGet]
                public async Task<ActionResult<IEnumerable<Reservation>>> GetReservation()
                {
                  if (_context.Reservation == null)
                  {
                      return NotFound();
                  }
                    return await _context.Reservation.ToListAsync();
                }

                // GET: api/Reservations/5
                [HttpGet("{id}")]
                public async Task<ActionResult<Reservation>> GetReservation(int id)
                {
                  if (_context.Reservation == null)
                  {
                      return NotFound();
                  }
                    var reservation = await _context.Reservation.FindAsync(id);

                    if (reservation == null)
                    {
                        return NotFound();
                    }

                    return reservation;
                }

                // PUT: api/Reservations/5
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPut("{id}")]
                public async Task<IActionResult> PutReservation(int id, Reservation reservation)
                {
                    if (id != reservation.Id)
                    {
                        return BadRequest();
                    }

                    _context.Entry(reservation).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ReservationExists(id))
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

                // POST: api/Reservations
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPost]
                public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
                {
                  if (_context.Reservation == null)
                  {
                      return Problem("Entity set 'RozhnovBackContext.Reservation'  is null.");
                  }
                    _context.Reservation.Add(reservation);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetReservation", new { id = reservation.Id }, reservation);
                }

                // DELETE: api/Reservations/5
                [HttpDelete("{id}")]
                public async Task<IActionResult> DeleteReservation(int id)
                {
                    if (_context.Reservation == null)
                    {
                        return NotFound();
                    }
                    var reservation = await _context.Reservation.FindAsync(id);
                    if (reservation == null)
                    {
                        return NotFound();
                    }

                    _context.Reservation.Remove(reservation);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }

                private bool ReservationExists(int id)
                {
                    return (_context.Reservation?.Any(e => e.Id == id)).GetValueOrDefault();
                }*/
    }
}
