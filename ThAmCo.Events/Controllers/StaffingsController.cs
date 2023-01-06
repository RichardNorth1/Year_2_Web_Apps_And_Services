using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Controllers
{
    public class StaffingsController : Controller
    {
        private readonly EventsDBContext _context;

        public StaffingsController(EventsDBContext context)
        {
            _context = context;
        }

        #region Index
        /// <summary>
        /// This method is used to to find all staff currently assigned to an event 
        /// this is achieved by searching the database for staffing that corresponds 
        /// to event ID supplied by the event ID in the parameters once all staffing 
        /// for an event as been found they are then transformed into a staffing view 
        /// model and passed to the view to be displayed to the user.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>a collection of staffing for an event if successful other wise returns not found</returns>
        // GET: Staffings
        public async Task<IActionResult> Index(int eventId)
        {
            if (!EventExists(eventId))
            {
                return NotFound();
            }
            // find the event 
            var eventBooking = await _context.Event.FindAsync(eventId);
            // find all staff assigned to the event
            var events = await _context.Staffing
                //.Include(s => s.Event)
                .Include(s => s.Staff)
                .Where(s => s.EventId == eventId)
                .ToListAsync();
            // transform the staffing to the view model
            var VM = events.Select(s => new StaffingViewModel(s));
            // provide event data to display to the view
            ViewData["EventBookingName"] = eventBooking.EventName;
            ViewData["EventId"] = eventId;
            ViewData["HasRequiredStaff"] = (bool)eventBooking.HasRequiredStaff;
            ViewData["HasFirstAider"] = (bool)eventBooking.HasFirstAider;
            return View(VM);

        }
        #endregion

        #region Details

        //// GET: Staffings/Details/5
        //public async Task<IActionResult> Details(int eventId, int staffId)
        //{
        //    //if (id == null || _context.Staffing == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    var staffing = await _context.Staffing
        //        .Include(s => s.Event)
        //        .Include(s => s.Staff)
        //        .Where(s => s.EventId == eventId && s.StaffId == staffId)
        //        .FirstOrDefaultAsync();
        //    if (staffing == null)
        //    {
        //        return NotFound();
        //    }

        //    var VM = new StaffingViewModel(staffing);

        //    return View(staffing);
        //}
        #endregion

        #region Create

        /// <summary>
        /// This method is used to assign staff members to an event this is achieved 
        /// by first loading in all staff members currently assigned to an event and 
        /// storing the IDs in a collection this list of IDs is then used to filter 
        /// out any staff memebers that are already assigned to the event and then 
        /// the list of not assigned staff is converted into a select list to be 
        /// used in the view
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>a staffing view model to the CREATE view otherwise not found is returned</returns>
        // GET: Staffings/Create
        public async Task<IActionResult> Create(int eventId)
        {
            if (!EventExists(eventId))
            {
                return NotFound();
            }
            // find all staff currently assigned
            var staffingForEvent = await _context.Staffing
                .Where(gb => gb.EventId == eventId).ToListAsync();

            //// if not staff
            //if (staffingForEvent == null)
            //{
            //    return NotFound();
            //}
            // create a list of IDs from the assigned staff
            var listOfIds = staffingForEvent.Select(g => g.StaffId).ToList();

            // create list that doesnt contain those already booked
            var staff = await _context.Staff
                .Where(s => !listOfIds.Contains(s.StaffId))
                .ToListAsync();

            // create the view model
            var VM = new StaffingViewModel
            {
                EventId = eventId,
                StaffSelectList = new List<SelectListItem>()
            };
            // populate the select list
            staff.ForEach(g => VM.StaffSelectList.Add(new SelectListItem { Text = g.FullName, Value = g.StaffId.ToString() }));

            return View(VM);
        }

        /// <summary>
        /// This method is used to create a new staff assignment which is achieved
        /// by determining which staff member the user wants to asssign to an event 
        /// from the select list after this the staff ID value which was assigned to 
        /// options provided in the select list. Once the staff ID has been obtained
        /// a new staffing object is created based on this and is then added to the 
        /// database and the changes are then saved.
        /// </summary>
        /// <param name="VM"></param>
        /// <returns>redirects the user to the INDEX view otherwise the invalid selection is returned</returns>
        // POST: Staffings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StaffingViewModel VM)
        {
            if (ModelState.IsValid)
            {
                // assign the staff member to the event
                var staffing = new Staffing { 
                    EventId = VM.EventId, 
                    StaffId = int.Parse(VM.SelectedStaff) 
                };
                try
                {
                    // save newly assigned staff member
                    _context.Add(staffing);
                    await _context.SaveChangesAsync();

                    // check if that member of staff is first aid qualified
                    var addedStaff =  await _context.Staff.FindAsync(staffing.StaffId);

                    if (addedStaff.FirstAidQualified)
                    {
                        // get the event assigned to and update the HasFirstAider value
                        var eventToUpdate = await _context.Event.FindAsync(staffing.EventId);
                        eventToUpdate.HasFirstAider = true;
                        _context.Update(eventToUpdate);
                    }
                    //else
                    //{
                    //    var eventToUpdate = await _context.Event.FindAsync(staffing.EventId);
                    //    eventToUpdate.HasFirstAider = false;
                    //    _context.Update(eventToUpdate);
                    //}
                    
                    // check if the event has enough staff
                    var amountOfGuests = await _context.GuestBooking.Where(gb => gb.EventId == staffing.EventId).CountAsync();
                    var amountOfStaff = await _context.Staffing.Where(s => s.EventId == staffing.EventId).CountAsync();

                    // if required amount less than or equal to the amount of staff
                    if ( (float)amountOfGuests / 10  <= amountOfStaff)
                    {
                        var eventToUpdate = await _context.Event.FindAsync(staffing.EventId);
                        eventToUpdate.HasRequiredStaff = true;
                        _context.Update(eventToUpdate);
                    }
                    //else
                    //{
                    //    var eventToUpdate = await _context.Event.FindAsync(staffing.EventId);
                    //    eventToUpdate.HasRequiredStaff = false;
                    //    _context.Update(eventToUpdate);
                    //}

                    // save changes
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index), new { eventId = VM.EventId });
                }
                catch(DbUpdateConcurrencyException)
                {
                    throw;
                }
                //_context.Add(staffing);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index), new {eventId = VM.EventId});
            }
            //ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId", staffing.EventId);
            //ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "Email", staffing.StaffId);
            return View(VM);
        }
        #endregion

        #region Edit

        //// GET: Staffings/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Staffing == null)
        //    {
        //        return NotFound();
        //    }

        //    var staffing = await _context.Staffing.FindAsync(id);
        //    if (staffing == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId", staffing.EventId);
        //    ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "Email", staffing.StaffId);
        //    return View(staffing);
        //}

        //// POST: Staffings/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("StaffId,EventId")] Staffing staffing)
        //{
        //    if (id != staffing.StaffId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(staffing);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!StaffingExists(staffing.StaffId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId", staffing.EventId);
        //    ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "Email", staffing.StaffId);
        //    return View(staffing);
        //}
        #endregion

        #region Delete
        /// <summary>
        /// This method is used to find the corresponding staff assignement based on 
        /// the staff ID and the event ID provided in the parameter once the asssignment
        /// has been found it is then tansformed in to a staffing view model which is 
        /// then displayed to the user to provide the details of the staff assignment 
        /// they wish to delete for confirmation before the action takes place.
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="eventId"></param>
        /// <returns>a staffing view model to the view if successful otherwise not found is returned</returns>
        // GET: Staffings/Delete/5
        public async Task<IActionResult> Delete(int staffId, int eventId)
        {
            var staffing = await _context.Staffing
                .Include(s => s.Event)
                .Include(s => s.Staff)
                .FirstOrDefaultAsync(m => m.StaffId == staffId && m.EventId == eventId);
            if (staffing == null)
            {
                return NotFound();
            }

            var VM = new StaffingViewModel(staffing);
            return View(VM);
        }

        /// <summary>
        /// This method is used to remove a member of staff from an event after 
        /// conformation has been given. this achieved by finding the corresponding 
        /// the event and removing it from the database and saving the changes. 
        /// This method is also used to alter the wether an event has the required 
        /// staffing level and wether it has a first adier assigned afteer the 
        /// deletion has taken place
        /// </summary>
        /// <param name="VM"></param>
        /// <returns>redirct the user to INDEX if successful otherwise the invalid staff assignment is returned</returns>
        // POST: Staffings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(StaffingViewModel VM)
        {

            var staffing = await _context.Staffing
                .FirstOrDefaultAsync(s => s.StaffId == VM.StaffId && s.EventId == VM.EventId);
            
            if (staffing != null)
            {            
                try
                {
                    _context.Staffing.Remove(staffing);

                    // check if the event has enough staff
                    var amountOfGuests = await _context.GuestBooking.Where(gb => gb.EventId == staffing.EventId).CountAsync();
                    var amountOfStaff = await _context.Staffing.Where(s => s.EventId == staffing.EventId && s.StaffId != staffing.StaffId).CountAsync();

                    if ((float)amountOfGuests / 10 > amountOfStaff || amountOfStaff == 0 )
                    {
                        var eventToUpdate = await _context.Event.FindAsync(staffing.EventId);
                        eventToUpdate.HasRequiredStaff = false;
                        _context.Update(eventToUpdate);
                    }
                    
                    // check if there will no longer be a first aider assigned
                    var firstAidQualifedStaff = await _context.Staffing
                        .Include(s => s.Staff)
                        .Where(s => s.Staff.FirstAidQualified == true && s.StaffId != staffing.StaffId) 
                        .FirstOrDefaultAsync();
                    if(firstAidQualifedStaff == null)
                    {
                        var eventToUpdate = await _context.Event.FindAsync(staffing.EventId);
                        eventToUpdate.HasFirstAider = false;
                        _context.Update(eventToUpdate);
                    }

                    // save changes
                    
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index), new { eventId = VM.EventId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            //await _context.SaveChangesAsync();
            return View(VM);
        }

        #endregion

        #region private methods
        /// <summary>
        /// This method is designed to determine wether staffing exists or not 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>true if the staffing exists other wise returns false</returns>
        private bool StaffingExists(int staffId , int eventId)
        {
          return _context.Staffing.Any(e => e.StaffId == staffId && e.EventId == eventId);
        }

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
