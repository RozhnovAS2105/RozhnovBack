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

        private static List<Reservation> Reservations = new List<Reservation>();

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult GetAllReservations()
        {
            return Ok(Reservations);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetReservationById(int id)
        {
            var reservation = Reservations.FirstOrDefault(r => r.Id == id);
            return reservation != null ? Ok(reservation) : NotFound("Reservation not found");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult CreateReservation([FromBody] Reservation reservation)
        {
            reservation.Id = Reservations.Any() ? Reservations.Max(r => r.Id) + 1 : 1;
            Reservations.Add(reservation);
            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        }

        [HttpGet("guest/{guestId}")]
        [Authorize(Roles = "admin")]
        public IActionResult GetReservationsByGuest(int guestId)
        {
            var guestReservations = Reservations.Where(r => r.GuestId == guestId).ToList();
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
