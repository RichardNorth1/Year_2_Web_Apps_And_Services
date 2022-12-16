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
        // GET: Staffings
        public async Task<IActionResult> Index(int eventId)
        {
            var eventBooking = await _context.Event.FindAsync(eventId);
            var events = await _context.Staffing
                .Include(s => s.Event)
                .Include(s => s.Staff)
                .Where(s => s.EventId == eventId)
                .ToListAsync();
            var VM = events.Select(s => new StaffingViewModel(s));
            ViewData["EventBookingName"] = eventBooking.EventName;
            ViewData["EventId"] = eventId;
            ViewData["HasRequiredStaff"] = (bool)eventBooking.HasRequiredStaff;
            ViewData["HasFirstAider"] = (bool)eventBooking.HasFirstAider;
            return View(VM);

        }
        #endregion

        #region Details

        // GET: Staffings/Details/5
        public async Task<IActionResult> Details(int eventId)
        {
            //if (id == null || _context.Staffing == null)
            //{
            //    return NotFound();
            //}

            var staffing = await _context.Staffing
                .Include(s => s.Event)
                .Include(s => s.Staff)
                .FirstOrDefaultAsync(m => m.StaffId == eventId);
            if (staffing == null)
            {
                return NotFound();
            }

            var VM = new StaffingViewModel(staffing);

            return View(staffing);
        }
        #endregion

        #region Create

        // GET: Staffings/Create
        public async Task<IActionResult> Create(int eventId)
        {
            var staffingForEvent = await _context.Staffing
                .Where(gb => gb.EventId == eventId).ToListAsync();

            if (staffingForEvent == null)
            {
                return NotFound();
            }

            var listOfIds = staffingForEvent.Select(g => g.StaffId).ToList();

            // create list that doesnt contain those already booked
            var staff = await _context.Staff
                .Where(s => !listOfIds.Contains(s.StaffId))
                .ToListAsync();


            var VM = new StaffingViewModel
            {
                EventId = eventId,
                StaffSelectList = new List<SelectListItem>()
            };

            staff.ForEach(g => VM.StaffSelectList.Add(new SelectListItem { Text = g.FullName, Value = g.StaffId.ToString() }));

            return View(VM);
        }

        // POST: Staffings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StaffingViewModel VM)
        {
            if (ModelState.IsValid)
            {
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

        // GET: Staffings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Staffing == null)
            {
                return NotFound();
            }

            var staffing = await _context.Staffing.FindAsync(id);
            if (staffing == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId", staffing.EventId);
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "Email", staffing.StaffId);
            return View(staffing);
        }

        // POST: Staffings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StaffId,EventId")] Staffing staffing)
        {
            if (id != staffing.StaffId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staffing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffingExists(staffing.StaffId))
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
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventId", staffing.EventId);
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "Email", staffing.StaffId);
            return View(staffing);
        }
        #endregion

        #region Delete

        // GET: Staffings/Delete/5
        public async Task<IActionResult> Delete(int staffId, int eventId)
        {
            if (_context.Staffing == null)
            {
                return NotFound();
            }

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

        // POST: Staffings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(StaffingViewModel VM)
        {
            if (_context.Staffing == null)
            {
                return Problem("Entity set 'EventsDBContext.Staffing'  is null.");
            }
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
            return RedirectToAction(nameof(Index));
        }

        #endregion
        private bool StaffingExists(int id)
        {
          return _context.Staffing.Any(e => e.StaffId == id);
        }
    }
}
