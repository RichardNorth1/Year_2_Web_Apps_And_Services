using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Controllers
{
    public class GuestBookingsController : Controller
    {
        private readonly EventsDBContext _context;

        public GuestBookingsController(EventsDBContext context)
        {
            _context = context;
        }

        // GET: GuestBookings
        public async Task<IActionResult> Index()
        {
            var allGuestBookings = await _context.GuestBooking
                .Include(g => g.Event).Include(g => g.Guest).ToListAsync();
            var VM = allGuestBookings.Select(g => new GuestBookingViewModel(g));
            return View(VM);
        }

        // GET: GuestBookings/Details/5
        public async Task<IActionResult> Details(int? id, int id2)
        {
            if (id == null || _context.GuestBooking == null)
            {
                return NotFound();
            }

            var guestBooking = await _context.GuestBooking
                .Include(g => g.Event)
                .Include(g => g.Guest)
                .FirstOrDefaultAsync(m => m.EventId == id && m.GuestId ==id2);
            if (guestBooking == null)
            {
                return NotFound();
            }

            var VM = new GuestBookingViewModel(guestBooking);
            return View(VM);
        }

        // GET: GuestBookings/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId");
            ViewData["GuestId"] = new SelectList(_context.Guest, "GuestId", "Forename");
            return View();
        }

        // POST: GuestBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GuestId,EventId")] GuestBooking guestBooking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(guestBooking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId", guestBooking.EventId);
            ViewData["GuestId"] = new SelectList(_context.Guest, "GuestId", "Forename", guestBooking.GuestId);
            return View(guestBooking);
        }

        // GET: GuestBookings/Edit/5
        public async Task<IActionResult> Edit(int? id, int id2)
        {
            if (id == null || _context.GuestBooking == null)
            {
                return NotFound();
            }

            var guestBooking = await _context.GuestBooking.FindAsync(id, id2);
            if (guestBooking == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId", guestBooking.EventId);
            ViewData["GuestId"] = new SelectList(_context.Guest, "GuestId", "Forename", guestBooking.GuestId);
            return View(guestBooking);
        }

        // POST: GuestBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GuestId,EventId")] GuestBooking guestBooking)
        {
            if (id != guestBooking.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guestBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuestBookingExists(guestBooking.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId", guestBooking.EventId);
            ViewData["GuestId"] = new SelectList(_context.Guest, "GuestId", "Forename", guestBooking.GuestId);
            return View(guestBooking);
        }

        // GET: GuestBookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GuestBooking == null)
            {
                return NotFound();
            }

            var guestBooking = await _context.GuestBooking
                .Include(g => g.Event)
                .Include(g => g.Guest)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (guestBooking == null)
            {
                return NotFound();
            }

            return View(guestBooking);
        }

        // POST: GuestBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GuestBooking == null)
            {
                return Problem("Entity set 'EventsDBContext.GuestBooking'  is null.");
            }
            var guestBooking = await _context.GuestBooking.FindAsync(id);
            if (guestBooking != null)
            {
                _context.GuestBooking.Remove(guestBooking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuestBookingExists(int id)
        {
          return _context.GuestBooking.Any(e => e.EventId == id);
        }
    }
}
