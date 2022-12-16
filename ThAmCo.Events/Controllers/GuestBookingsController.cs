using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using ThAmCo.Events.Data;
using ThAmCo.Events.DTOs;
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

        #region Index
        // GET: GuestBookings
        public async Task<IActionResult> Index(int eventId)
        {
            var eventBooking = await _context.Event.FindAsync(eventId);
            var guestBookings = await _context.GuestBooking
                .Include(gb => gb.Guest)
                .Include(gb => gb.Event)
                .Where(e => e.EventId == eventId).ToListAsync();

            var VM = guestBookings.Select(g => new GuestBookingViewModel {
                Event = new EventViewModel(g.Event),
                EventId = g.EventId, 
                GuestId = g.GuestId, 
                Guest = new GuestViewModel(g.Guest),
                Attended = g.Attended == true? "Yes": "No" });
            ViewData["EventBookingName"] = eventBooking.EventName;
            ViewData["EventId"] = eventId;

            return View(VM);
        }
        #endregion

        #region Details
        // GET: GuestBookings/Details/5
        public async Task<IActionResult> Details(int eventId)
        {

            var guestBookings = await _context.GuestBooking
                .Include(g => g.Event)
                .Include(g => g.Guest)
                .Where(e => e.EventId == eventId).ToListAsync();

            if (guestBookings == null)
            {
                return NotFound();
            }
            var VM = guestBookings.Select(gb => new GuestBookingViewModel(gb));
            return View(VM);
        }
        #endregion

        #region Create
        // GET: GuestBookings/Create
        public async Task<IActionResult> Create(int eventId)
        {

            var guestBookingForEvent = await _context.GuestBooking
                .Where(gb => gb.EventId == eventId).ToListAsync();

            if (guestBookingForEvent == null)
            {
                return NotFound();
            }

            var listOfIds = guestBookingForEvent.Select(g => g.GuestId).ToList();

            // create list that doesnt contain those already booked
            var guests = await _context.Guest
                .Where(g => !listOfIds.Contains(g.GuestId))
                .ToListAsync();

            var VM = new GuestBookingViewModel
            {
                EventId = eventId,
                GuestSelectList = new List<SelectListItem>()
            };

            guests.ForEach(g => VM.GuestSelectList.Add(new SelectListItem { Text = g.FullName, Value = g.GuestId.ToString() }));

            return View(VM);
        }



        // POST: GuestBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GuestBookingViewModel VM)
        {

            if (ModelState.IsValid)
            {
                var guestBooking = new GuestBooking
                {
                    GuestId = int.Parse(VM.SelectedGuestId),
                    EventId = VM.EventId
                };
                try
                {
                    // add newly assigned guest
                    _context.Add(guestBooking);

                    // check if the event has enough staff
                    var amountOfGuests = await _context.GuestBooking.Where(gb => gb.EventId == guestBooking.EventId).CountAsync();
                    var amountOfStaff = await _context.Staffing.Where(s => s.EventId == guestBooking.EventId).CountAsync();

                    // if required amount of staff greater and the amount of staff
                    if ((int)Math.Ceiling((float)amountOfGuests / 10.00) > amountOfStaff)
                    {
                        // update if true
                        var eventToUpdate = await _context.Event.FindAsync(guestBooking.EventId);
                        eventToUpdate.HasRequiredStaff = false;
                        _context.Update(eventToUpdate);
                    }
                    //else
                    //{
                    //    var eventToUpdate = await _context.Event.FindAsync(guestBooking.EventId);
                    //    eventToUpdate.HasRequiredStaff = true;
                    //    _context.Update(eventToUpdate);
                    //}

                    // save changes

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { eventId = VM.EventId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return View(VM);
        }
        #endregion

        #region Edit
        // GET: GuestBookings/Edit/5
        public async Task<IActionResult> Edit(int eventId, int GuestId)
        {

            var guestBooking = await _context.GuestBooking.Include(g=> g.Guest).Include(g=> g.Event)
                .Where(gb => gb.GuestId == GuestId && gb.EventId == eventId)
                .FirstOrDefaultAsync();
            if (guestBooking == null)
            {
                return NotFound();
            }
            var VM = new GuestBookingViewModel(guestBooking);
            //ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId", guestBooking.EventId);
            //ViewData["GuestId"] = new SelectList(_context.Guest, "GuestId", "Forename", guestBooking.GuestId);
            return View(VM);
        }

        // POST: GuestBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GuestBookingViewModel VM)
        {
            var attened = VM.Attended == "yes";
            var guestBooking = new GuestBooking { 
                EventId = VM.EventId, 
                GuestId = VM.GuestId, 
                Attended = attened };
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

            return RedirectToAction(nameof(Index), new { eventId = VM.EventId });
            //}
            //return View(guestBooking);
        }
        #endregion

        #region Delete
        // GET: GuestBookings/Delete/5
        public async Task<IActionResult> Delete(int eventId, int guestId)
        {
            //if (id == null || _context.GuestBooking == null)
            //{
            //    return NotFound();
            //}

            var guestBooking = await _context.GuestBooking
                .Include(g => g.Event)
                .Include(g => g.Guest)
                .FirstOrDefaultAsync(m => m.EventId == eventId && m.GuestId == guestId);
            if (guestBooking == null)
            {
                return NotFound();
            }

            return View(guestBooking);
        }

        // POST: GuestBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int eventId, int guestId)
        {
            if (_context.GuestBooking == null)
            {
                return Problem("Entity set 'EventsDBContext.GuestBooking'  is null.");
            }
            var guestBooking = await _context.GuestBooking
                .FirstOrDefaultAsync(m => m.EventId == eventId && m.GuestId == guestId);
            if (guestBooking != null)
            {
                _context.GuestBooking.Remove(guestBooking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { eventId = eventId});
        }
        #endregion

        private bool GuestBookingExists(int id)
        {
            return _context.GuestBooking.Any(e => e.EventId == id);
        }
    }
}
