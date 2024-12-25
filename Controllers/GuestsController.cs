using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RozhnovBack.Data;
using RozhnovBack.Models;

namespace RozhnovBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestsController : ControllerBase
    {
        private readonly RozhnovBackContext _context;

        private static List<Guest> Guests = new List<Guest>();

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult GetAllGuests()
        {
            return Ok(Guests);
        }

        [HttpGet("{id}")]
        [Authorize]
        //[Authorize(Roles = "admin")]
        public IActionResult GetGuestById(int id)
        {
            var guest = Guests.FirstOrDefault(g => g.Id == id);

            // Если гость не найден, возвращаем NotFound
            if (guest == null)
            {
                return NotFound("Guest not found");
            }

            // Проверяем, является ли пользователь авторизованным гостем или админом
            if (User.Identity.Name != guest.Name && !User.IsInRole("admin"))
            {
                return Unauthorized("You are not authorized to view this guest's information.");
            }

            return Ok(guest);
        }


        [HttpPost]
        [Authorize]
        //[Authorize(Roles = "admin")]
        public IActionResult CreateGuest([FromBody] Guest guest)
        {
            var existingGuest = Guests.FirstOrDefault(g => g.Name == guest.Name);
            if (existingGuest != null)
            {
                return BadRequest("Guest is already registered.");
            }

            guest.Id = Guests.Any() ? Guests.Max(g => g.Id) + 1 : 1;
            Guests.Add(guest);

            return CreatedAtAction(nameof(GetGuestById), new { id = guest.Id }, guest);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteGuest(int id)
        {
            var guest = Guests.FirstOrDefault(g => g.Id == id);
            if (guest == null) return NotFound("Guest not found");

            Guests.Remove(guest);
            return NoContent();
        }

/*        public GuestsController(RozhnovBackContext context)
        {
            _context = context;
        }

        // GET: api/Guests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Guest>>> GetGuest()

        {
          if (_context.Guest == null)
          {
              return NotFound();
          }
            return await _context.Guest.ToListAsync();
        }

        // GET: api/Guests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Guest>> GetGuest(int id)
        {
          if (_context.Guest == null)
          {
              return NotFound();
          }
            var guest = await _context.Guest.FindAsync(id);

            if (guest == null)
            {
                return NotFound();
            }

            return guest;
        }

        // PUT: api/Guests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGuest(int id, Guest guest)
        {
            if (id != guest.Id)
            {
                return BadRequest();
            }

            _context.Entry(guest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GuestExists(id))
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

        // POST: api/Guests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Guest>> PostGuest(Guest guest)
        {
          if (_context.Guest == null)
          {
              return Problem("Entity set 'RozhnovBackContext.Guest'  is null.");
          }
            _context.Guest.Add(guest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGuest", new { id = guest.Id }, guest);
        }*/

        // DELETE: api/Guests/5
/*        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            if (_context.Guest == null)
            {
                return NotFound();
            }
            var guest = await _context.Guest.FindAsync(id);
            if (guest == null)
            {
                return NotFound();
            }

            _context.Guest.Remove(guest);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool GuestExists(int id)
        {
            return (_context.Guest?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
