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
        /// <summary>
        /// This method is used to display all guests currently assigned to an event this is achieved by taking in an event ID 
        /// and searching the database to find any if there are any guests currently assigned to the event. After this the guest
        /// booking are then transformed into a collection of guest booking view models and returned to the view
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns> a collection of guestbooking view models to a view</returns>
        // GET: GuestBookings
        public async Task<IActionResult> Index(int eventId)
        {
            // find the event in the  database
            var eventBooking = await _context.Event.FindAsync(eventId);

            // find all guest bookings for the event
            var guestBookings = await _context.GuestBooking
                .Include(gb => gb.Guest)
                .Include(gb => gb.Event)
                .Where(e => e.EventId == eventId).ToListAsync();

            // transform the guest booking into a view model
            var VM = guestBookings.Select(g => new GuestBookingViewModel {
                Event = new EventViewModel(g.Event),
                EventId = g.EventId, 
                GuestId = g.GuestId, 
                Guest = new GuestViewModel(g.Guest),
                Attended = g.Attended == true? "Yes": "No" });
            // pass event details to view for a better user experience
            ViewData["EventBookingName"] = eventBooking.EventName;
            ViewData["EventId"] = eventId;

            return View(VM);
        }
        #endregion

        //#region Details
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="eventId"></param>
        ///// <returns></returns>
        //// GET: GuestBookings/Details/5
        //public async Task<IActionResult> Details(int eventId)
        //{

        //    var guestBookings = await _context.GuestBooking
        //        .Include(g => g.Event)
        //        .Include(g => g.Guest)
        //        .Where(e => e.EventId == eventId).ToListAsync();

        //    if (guestBookings == null)
        //    {
        //        return NotFound();
        //    }
        //    var VM = guestBookings.Select(gb => new GuestBookingViewModel(gb));
        //    return View(VM);
        //}
        //#endregion

        #region Create
        /// <summary>
        /// This method is used to create a new guest booking, this is achieved by searching the database to 
        /// find all current guests, after this a list of guests that are currently signed to the event is created
        /// then all of the guests that are already assigned are removed and this list is added to the view model to be displayed in the view
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns> guest booking view model to the view</returns>
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


        /// <summary>
        /// This method is designed to create a new guest booking based on the guest that the user has selected 
        /// once the guest booking has been created it is then saved to the database
        /// </summary>
        /// <param name="VM"></param>
        /// <returns>the create view if unsuccessful and redirects to the index view if the creation was successful</returns>
        // POST: GuestBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GuestBookingViewModel VM)
        {
            // check if view has been correctly filled
            if (ModelState.IsValid)
            {
                // create  a new guest booking based on the selected guest
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
        /// <summary>
        /// This method is used find a specific guest booking that  is stored i the database and transform it in to a 
        /// guest booking view model that is then displayed to the view ready to be edited
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="GuestId"></param>
        /// <returns> a guest booking view model to the EDIT view</returns>
        // GET: GuestBookings/Edit/5
        public async Task<IActionResult> Edit(int eventId, int GuestId)
        {
            // find the corresponding guestbooking to the event id and guest id specified in the parameters
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

        /// <summary>
        /// This method is designed to update whether a guest has attended attended an event.
        /// </summary>
        /// <param name="VM"></param>
        /// <returns>once the guest booking is updated the user is then redirected to the INDEX view</returns>
        // POST: GuestBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GuestBookingViewModel VM)
        {
            // if vm.attended equals yes attended is true otherwise attened is false
            var attened = VM.Attended == "yes";
            // find the guest booking and apply the update
            var guestBooking = await _context.GuestBooking
                .Where(gb => gb.GuestId == VM.GuestId && gb.EventId == VM.EventId)
                .FirstOrDefaultAsync();
            guestBooking.Attended = attened;

            try
            {
                // try to apply the update
                _context.Update(guestBooking);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GuestBookingExists(guestBooking.EventId, guestBooking.GuestId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index), new { eventId = VM.EventId });
        }
        #endregion

        #region Delete
        /// <summary>
        /// This method is used to find the the guest booking that the user intends to delete this
        /// is achieved by searching the database for the guest booking that corresponds to the 
        /// event ID and Guest ID that is supplied to the method via the parameters. 
        /// Once the guest booking has been found the guest booking is then transformed into a guest booking
        /// view model which is then dispayed to the DELETE view
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="guestId"></param>
        /// <returns>a guest booking view model to the view</returns>
        // GET: GuestBookings/Delete/5
        public async Task<IActionResult> Delete(int eventId, int guestId)
        {
            //if (id == null || _context.GuestBooking == null)
            //{
            //    return NotFound();
            //}
            // find the guest booking
            var guestBooking = await _context.GuestBooking
                .Include(g => g.Event)
                .Include(g => g.Guest)
                .FirstOrDefaultAsync(m => m.EventId == eventId && m.GuestId == guestId);

            // if the guest booking is null return not found view
            if (guestBooking == null)
            {
                return NotFound();
            }

            return View(guestBooking);
        }

        /// <summary>
        /// This method is used to delete a guest booking from the database this is achieved by 
        /// finding the guest booking that the user has comfirmed they wish to delete and once 
        /// the user has confirmed the guest booking is then removed from the database
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="guestId"></param>
        /// <returns>returns a redirection to the INDEX page if successful and not found if unsuccessful</returns>
        // POST: GuestBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int eventId, int guestId)
        {
            if (_context.GuestBooking == null)
            {
                return Problem("Entity set 'EventsDBContext.GuestBooking'  is null.");
            }
            // find the booking
            var guestBooking = await _context.GuestBooking
                .Where(m => m.EventId == eventId && m.GuestId == guestId)
                .FirstOrDefaultAsync();
            // check if booking exists
            if (guestBooking != null)
            {
                // save changes and redirect user to the index page
                _context.GuestBooking.Remove(guestBooking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { eventId = eventId });
            }
            // if booking doesnt exists return not found
            return NotFound();

        }
        #endregion

        #region private methods
        /// <summary>
        /// This method is used to determin wether a guest booking exists or not
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if exists otherwise returns false</returns>
        private bool GuestBookingExists(int eventId, int guestId)
        {
            return _context.GuestBooking.Any(e => e.EventId == eventId && e.GuestId == guestId);
        }

        #endregion
    }
}
