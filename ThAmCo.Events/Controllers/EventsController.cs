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

        /// <summary>
        /// This method is designed to create a collection of events and transform them in an 'EventViewModel' and display these events in an mvc view
        /// </summary>
        /// <returns>a collection of event view models</returns>
        // GET: Events
        public async Task<IActionResult> Index()
        {
            if (_context.Event == null)
            {
                return NotFound();
            }

            // get all of the events that are currently not soft deleted
            var allEvents = await _context.Event.Include(s => s.GuestBookings).Include(s => s.Staffs).Where(e => e.IsDeleted != true).ToListAsync();
            // transform all events in to a collection of event view models
            var VM = allEvents.Select(e => new EventViewModel(e) { GuestCount = e.GuestBookings.Count , StaffCount = e.Staffs.Count});


            return View(VM);
        }
        #endregion

        #region Details

        /// <summary>
        /// This method is designed to take an event ID find the corresponding event in the database then transform the event in to an event view model 
        /// which can then be sent to the view
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>a view to display the the events details</returns>
        // GET: Events/Details/5
        public async Task<IActionResult> Details(int eventId)
        {
            if (_context.Event == null)
            {
                return NotFound();
            }
            // find the event that matches the event ID supplied to the method
            var eventToDisplay = await _context.Event.Include(e => e.GuestBookings).Include(e => e.Staffs)
                .FirstOrDefaultAsync(m => m.EventId == eventId);

            // if not found 
            if (eventToDisplay == null)
            {
                return NotFound();
            }
            // after the event has been verified as not null load all associated staff and events for that event
            var listofStaffForEvent = await _context.Staffing.Include(s => s.Staff).Where(s => s.EventId == eventId).ToListAsync();
            var listOfGuestsForEvent = await _context.GuestBooking.Include(gb => gb.Guest).Where(gb => gb.EventId == eventId).ToListAsync();

            // transform the event into an event view model
            var VM = new EventViewModel(eventToDisplay) { 
                GuestBookings = listOfGuestsForEvent.Select(g => new GuestViewModel(g.Guest)).ToList(),
                Staffing = listofStaffForEvent.Select(s => new StaffViewModel(s.Staff)).ToList() 
            };

            return View(VM);
        }
        #endregion

        #region Create

        /// <summary>
        /// this method is dessigned to load all the required data to populate any drop downlist and form data to the view
        /// but this method does not currently require this data because of the use of jquery and ajax being used for this form
        /// </summary>
        /// <returns> the create event view</returns>
        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// this method is used to take the form data that is entered into the form view which can then be used to create
        /// a new event based on the details that are entered
        /// </summary>
        /// <param name="VM"></param>
        /// <returns></returns>
        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [Bind("EventName, EventTypeId, EventDate, MenuId")] EventViewModel VM
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventViewModel VM)
        {
            // if the supplied details  fufill the validation
            if (ModelState.IsValid)
            {
                // split the strings that have been created  when a user selects an item from a drop down into individual values
                var eventDatvenueAndPrice = VM.SelectedEventDate.Split(',');
                var eventTypeIdAndType = VM.SelectedEventType.Split(',');
                var menuIdAndName = VM.SelectedMenuIdAndName.Split(',');

                // create a new event based off the CreateEventViewModel data
                var newEvent = new Event
                {
                    EventName = VM.EventName,
                    EventType = eventTypeIdAndType[1],
                    MenuForEvent = menuIdAndName[1],
                    EventDate = DateTime.Parse(eventDatvenueAndPrice[0]),
                    VenueName = eventDatvenueAndPrice[2],
                    HasFirstAider = false,
                    HasRequiredStaff = false,
                    EventDuration = VM.EventDuration,
                    EventCost = decimal.Parse(eventDatvenueAndPrice[3]) * VM.EventDuration
                };

                // create a food booking based on the CreateEventViewModel
                var foodBookingDto = new FoodBookingDto()
                {
                    ClientReferenceId = newEvent.ClientReferenceId,
                    MenuId = int.Parse(menuIdAndName[0]),
                    NumberOfGuests = VM.NumberOfGuestsForFoodBooking
                };

                // create a reservation based on the CreateEventViewModel
                var reservationDto = new ReservationPostDto { 
                    EventDate = DateTime.Parse(eventDatvenueAndPrice[0]), 
                    VenueCode = eventDatvenueAndPrice[1], StaffId = "1" };

                // check if the event required catering or not
                if(foodBookingDto.MenuId != 0)
                {
                    // post the data to the food booking api
                    newEvent.ClientReferenceId = int.Parse(DateTime.Now.ToString("MMddHHmmss"));
                    var JsonFoodBooking = JsonConvert.SerializeObject(foodBookingDto);
                    var cateringResponse = await cateringClient.PostAsJsonAsync("api/FoodBookings", foodBookingDto);
                    var returnFoodBookingDto = await cateringResponse.Content.ReadAsAsync<FoodBookingDto>();

                }
                // a reservation is required for all events so always create 
                var JsonReservation = JsonConvert.SerializeObject(reservationDto);
                var venueResponse = await venueClient.PostAsJsonAsync("api/Reservations", reservationDto);

                if (venueResponse.IsSuccessStatusCode)
                {

                    // if the reservation post request is successful then save the event and redirect to the index
                    var returnReservationDto = await venueResponse.Content.ReadAsAsync<ReservationGetDto>();

                    // save the newly created reference no to the event for later access
                    newEvent.Reference = returnReservationDto.Reference;
                    _context.Add(newEvent);
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Debug.WriteLine("Index received a bad response from the web service.");
                }
            }
            // return the incorrectly filled form
            return View(VM);
        }

        #endregion

        #region Edit

        /// <summary>
        /// This method is designed to take an event ID  find the corresponding event in the database 
        /// then create a new viewmodel to display this information to the user whilst also prefilling all
        /// details in the event incase not all details need to be changed
        /// </summary>
        /// <param name="eventid"></param>
        /// <returns></returns>
        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int eventid)
        {
           
            if (_context.Event == null)
            {
                return NotFound();
            }
            // find event
            var eventToEdit = await _context.Event.Where(e => e.EventId == eventid).FirstOrDefaultAsync();

            // null check
            if (eventToEdit == null)
            {
                return NotFound();
            }
            // if not null create a view model based on it 
            var VM = new EventViewModel(eventToEdit);
            return View(VM);
        }


        /// <summary>
        /// This method is designed to take the a data that is updated in the event view model and then find 
        /// the event in the database that corresponds to the event ID of the model and update the Necessary 
        /// fields
        /// </summary>
        /// <param name="VM"></param>
        /// <returns> The </returns>
        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventViewModel VM)
        {
            //if (id != eventToEdit.EventId)
            //{
            //    return NotFound();
            //}
            var eventToEdit = await _context.Event.Where(e => e.EventId == VM.EventId).FirstOrDefaultAsync();
            if (ModelState.IsValid)
            {
                try
                {
                    if (eventToEdit == null)
                    {
                        return NotFound();
                    }
                    var baseHourlyPrice = eventToEdit.EventCost / eventToEdit.EventDuration;
                    eventToEdit.EventDuration = VM.EventDuration;
                    eventToEdit.EventName = VM.EventName;
                    eventToEdit.EventCost = baseHourlyPrice * VM.EventDuration;
                    
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
            return View(VM);
        }
        #endregion

        #region Delete
        /// <summary>
        /// This method is used to take an event ID find the corresponding event in the database and then display it to the view
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>an event view model to the view</returns>
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
        /// <summary>
        /// This method is used to perform a soft delete on an event that corresponds to event ID parameter
        /// </summary>
        /// <param name="eventid"></param>
        /// <returns>a view</returns>
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


                var venueResponse = await venueClient.DeleteAsync("api/Reservations/" + eventToDelete.Reference);

                //if (venueResponse.IsSuccessStatusCode)
                //{
                //    var returnReservationDto = await venueResponse.Content.ReadAsAsync<ReservationGetDto>();

                //    newEvent.Reference = returnReservationDto.Reference;
                //    _context.Add(newEvent);
                //    await _context.SaveChangesAsync();

                //    Debug.WriteLine("food booking and reservation was successful");
                //    return RedirectToAction(nameof(Index));
                //    //AllEventTypes = await response.Content.ReadAsAsync<IEnumerable<EventTypeDto>>();
                //}
                //else
                //{
                //    Debug.WriteLine("Index received a bad response from the web service.");
                //}

                _context.Event.Update(eventToDelete);
                await _context.SaveChangesAsync();
            }
            
            
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region private methods
        /// <summary>
        /// This method is designed to determine wether an event exists or not 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>true if the event exists other wise returns false</returns>
        private bool EventExists(int eventId)
        {
          return _context.Event.Any(e => e.EventId == eventId);
        }
        #endregion
    }
}
