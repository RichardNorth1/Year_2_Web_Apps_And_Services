using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using ThAmCo.Events.Data;
using ThAmCo.Events.DTOs;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Controllers
{
    public class EventsController : Controller
    {
        private readonly HttpClient venueClient;
        private readonly HttpClient cateringClient;
        private readonly EventsDBContext _context;

        public EventsController(EventsDBContext context)
        {
            _context = context;
            venueClient = new HttpClient();
            cateringClient = new HttpClient();
            venueClient.BaseAddress = new System.Uri("https://localhost:7088/");
            cateringClient.BaseAddress = new System.Uri("https://localhost:7173/");
            venueClient.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            cateringClient.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }

        #region Index
        // GET: Events
        public async Task<IActionResult> Index()
        {
            var allEvents = await _context.Event.Include(s => s.GuestBookings).Include(s => s.Staffs).Where(e => e.IsDeleted != true).ToListAsync();
            var VM = allEvents.Select(e => new EventViewModel(e) { GuestCount = e.GuestBookings.Count , StaffCount = e.Staffs.Count});


            return View(VM);

        }
        #endregion

        #region Details
        // GET: Events/Details/5
        public async Task<IActionResult> Details(int eventId)
        {
            if (_context.Event == null)
            {
                return NotFound();
            }

            var eventToDisplay = await _context.Event.Include(e => e.GuestBookings).Include(e => e.Staffs)
                .FirstOrDefaultAsync(m => m.EventId == eventId);
            if (eventToDisplay == null)
            {
                return NotFound();
            }
            var VM = new EventViewModel(eventToDisplay);

            return View(VM);
        }
        #endregion

        #region Create
        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [Bind("EventName, EventTypeId, EventDate, MenuId")] EventViewModel VM
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventViewModel VM)
        {
            
            if (ModelState.IsValid)
            {
                // split the date and venue code from vm.selected venue and date
                var eventDateandvenue = VM.SelectedEventDate.Split(',');
                var newEvent = new Event
                {
                    EventName = VM.EventName,
                    //EventTypeId = VM.SelectedEventTypeId,
                    EventDate = DateTime.Parse(eventDateandvenue[0]),
                    VenueName = eventDateandvenue[2],
                    ClientReferenceId = int.Parse(DateTime.Now.ToString("MMddHHmmss")),
                    HasFirstAider = false,
                    HasRequiredStaff = false,
                };

                var foodBookingDto = new FoodBookingDto()
                {
                    ClientReferenceId = newEvent.ClientReferenceId,
                    MenuId = int.Parse(VM.SelectedMenuId.ToString()),
                    NumberOfGuests = 0
                };

                var reservationDto = new ReservationPostDto { 
                    EventDate = DateTime.Parse(eventDateandvenue[0]), 
                    VenueCode = eventDateandvenue[1], StaffId = "1" };


                if(foodBookingDto.MenuId != 0)
                {
                    var JsonFoodBooking = JsonConvert.SerializeObject(foodBookingDto);
                    var cateringResponse = await cateringClient.PostAsJsonAsync("api/FoodBookings", foodBookingDto);
                    var returnFoodBookingDto = await cateringResponse.Content.ReadAsAsync<FoodBookingDto>();

                }

                var JsonReservation = JsonConvert.SerializeObject(reservationDto);
                var venueResponse = await venueClient.PostAsJsonAsync("api/Reservations", reservationDto);

                if (venueResponse.IsSuccessStatusCode)
                {
                    var returnReservationDto = await venueResponse.Content.ReadAsAsync<ReservationGetDto>();
                    
                    newEvent.Reference = returnReservationDto.Reference;
                    _context.Add(newEvent);
                    await _context.SaveChangesAsync();
                    
                    Debug.WriteLine("food booking and reservation was successful");
                    return RedirectToAction(nameof(Index));
                    //AllEventTypes = await response.Content.ReadAsAsync<IEnumerable<EventTypeDto>>();
                }
                else
                {
                    Debug.WriteLine("Index received a bad response from the web service.");
                }


            }

            return View();
        }

        #endregion

        #region Edit
        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int eventid)
        {
            if (_context.Event == null)
            {
                return NotFound();
            }

            var eventToEdit = await _context.Event.FindAsync(eventid);
            if (eventToEdit == null)
            {
                return NotFound();
            }
            return View(eventToEdit);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventViewModel eventToEdit)
        {
            //if (id != eventToEdit.EventId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventToEdit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventToEdit.EventId))
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
            return View(eventToEdit);
        }
        #endregion

        #region Delete

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int eventId)
        {
            if (_context.Event == null)
            {
                return NotFound();
            }

            var eventToDelete = await _context.Event
                .FirstOrDefaultAsync(m => m.EventId == eventId);
            if (eventToDelete == null)
            {
                return NotFound();
            }
            var VM = new EventViewModel(eventToDelete);
            return View(VM);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int eventid)
        {
            if (_context.Event == null)
            {
                return Problem("Entity set 'EventsDBContext.Event'  is null.");
            }
            var eventToDelete = await _context.Event.FindAsync(eventid);
            if (eventToDelete != null)
            {
                // load guestBookings and staffing for an event and update to reflect the deletion
                var guestBookingsToUpDate = await _context.GuestBooking.Where(gb => gb.EventId== eventid).ToListAsync();
                var staffingToUpDate = await _context.Staffing.Where(s => s.EventId == eventid).ToListAsync();

                guestBookingsToUpDate.ForEach(gb => { gb.IsDeleted = true; _context.GuestBooking.Update(gb); });
                staffingToUpDate.ForEach(s => { s.IsDeleted = true; _context.Staffing.Update(s); });
                eventToDelete.IsDeleted = true;

                _context.Event.Update(eventToDelete);

            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        private bool EventExists(int id)
        {
          return _context.Event.Any(e => e.EventId == id);
        }
    }
}
