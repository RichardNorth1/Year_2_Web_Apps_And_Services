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
    public class StaffsController : Controller
    {
        private readonly EventsDBContext _context;

        public StaffsController(EventsDBContext context)
        {
            _context = context;
        }
        #region Index
        /// <summary>
        /// TThis method is designed to find all members of staff currently stored 
        /// within the database. after all members of staff have been located it is 
        /// then transformed in to a collection of staff view models and then 
        /// displayed in the view
        /// </summary>
        /// <returns>a collection of staff view models to the view</returns>
        // GET: Staffs
        public async Task<IActionResult> Index()
        {
            var allStaff = await _context.Staff.ToListAsync();
            var VM = allStaff.Select(s => new StaffViewModel(s));
              return View(VM);
        }

        #endregion

        #region Details

        /// <summary>
        /// This method is used to display the details of a specific staff member. 
        /// This is achieved by finding the corresponding member of staff to the 
        /// Staff ID that is provided with the parameter this member of staff is 
        /// then transformed into a staff view model and then displayed in the view
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns>a staff view model to the view if successful otherwise returns not found</returns>
        // GET: Staffs/Details/5
        public async Task<IActionResult> Details(int? staffId)
        {
            // find the member of staff
            var staff = await _context.Staff
                .Where(s => s.StaffId == staffId)
                .FirstOrDefaultAsync();
            // if staff doesnt exist return not found
            if (staff == null)
            {
                return NotFound();
            }
            // get the staff members events list
            var staffsUpComingEvents = await _context.Staffing
                //.Include(s => s.Staff)
                .Include(s => s.Event)
                .Where(s => s.StaffId == staffId)
                .ToListAsync();

            // transform the staff member to a staff member view model
            var VM = new StaffViewModel(staff);
            VM.Events = staffsUpComingEvents.Select(e => new EventViewModel(e.Event)).ToList();


            return View(VM);
        }

        #endregion

        #region Create

        /// <summary>
        /// This method is designed to return the CREATE view for creating a new staff member
        /// </summary>
        /// <returns>a CREATE view</returns>
        // GET: Staffs/Create
        public IActionResult Create()
        {
            return View(new StaffViewModel());
        }

        /// <summary>
        /// This method is used to create a new staff member based on the details provided to
        /// the view. Once the staff member has been created the staff member is then added to 
        /// the database and the changes are saved.
        /// </summary>
        /// <param name="staff"></param>
        /// <returns>redirects the user to INDEX if successful otherwise returns the invalid staff creation to the user</returns>
        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StaffViewModel VM)
        {
            var staff = new Staff { Forename = VM.Forename, 
                Surname = VM.Surname, 
                Email = VM.Email, 
                FirstAidQualified = VM.SelectedQualifiedValue == "yes", 
                JobRole = VM.JobRole, 
                Password = VM.Password };
            if (ModelState.IsValid)
            {
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(VM);
        }

        #endregion

        #region Edit
        /// <summary>
        /// This method is designed to find the staff member the user wishes to edit and then transforms the staff member into a staff view model to be displayed in the view
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns>a staff view model to the view</returns>
        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int staffId)
        {
            var staff = await _context.Staff
                .Where(s => s.StaffId == staffId)
                .FirstOrDefaultAsync();
            if (staff == null)
            {
                return NotFound();
            }
            var VM = new StaffViewModel(staff);
            return View(VM);
        }

        /// <summary>
        /// This method is designed to apply the changes that the user has made to a staff 
        /// member and then save the changes made to the staff member to the database.
        /// </summary>
        /// <param name="staff"></param>
        /// <returns>redirects the user to the INDEX view if successful otherwise returns the invalid staff edit</returns>
        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StaffViewModel VM)
        { 
            var staff = new Staff {StaffId = VM.StaffId, Forename = VM.Forename, 
                Surname =VM.Surname, Email = VM.Email, 
                FirstAidQualified = VM.SelectedQualifiedValue == "yes", 
                JobRole = VM.JobRole, Password = VM.Password };
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                    // find all events this staff member is assigned to
                    var eventsToUpdate = await _context.Staffing
                        .Include(s => s.Event)
                        .Where(s => s.StaffId == staff.StaffId)
                        .ToListAsync();

                    // check if they currently have any events
                    if (eventsToUpdate != null)
                    {
                        // if staff is now first aid qualified update all of their events to reflect this
                        if (staff.FirstAidQualified)
                        {
                            // iterate through their events
                            foreach(var events in eventsToUpdate)
                            {

                                // update the events that didnt already have a first aider
                                if (!(bool)events.Event.HasFirstAider)
                                {
                                    events.Event.HasFirstAider = true;
                                    _context.Update(events);
                                }
                            }
                            
                        }
                        else
                        {
                            // iterate through their events
                            foreach (var events in eventsToUpdate)
                            {
                                // load all staff assigned to this event
                                var staffAssignedToEvent = await _context.Staffing
                                    .Include(s => s.Staff)
                                    .Where(s => s.EventId == events.EventId)
                                    .ToListAsync();
                                if (!staffAssignedToEvent.Any(s => s.Staff.FirstAidQualified))
                                {
                                    events.Event.HasFirstAider = false;
                                    _context.Update(events);
                                }
                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.StaffId))
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
            return View(staff);
        }
        #endregion

        #region Delete
        /// <summary>
        /// This method is designed to find the corresponding staff member to the staff ID
        /// supplied in the parameters once the staff member is located it is then transformed 
        /// into a staff view model to be displayed in the view for confirmation of deletion.
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns>a staff view model to the view if succesful otherwise returns not found</returns>
        // GET: Staffs/Delete/5
        public async Task<IActionResult> Delete(int staffId)
        {
            var staff = await _context.Staff
                .Where(s => s.StaffId == staffId)
                .FirstOrDefaultAsync();
            if (staff == null)
            {
                return NotFound();
            }
            var VM = new StaffViewModel(staff);
            return View(VM);
        }

        /// <summary>
        /// This method is designed to locate a staff member in the database and then 
        /// remove them after conformation has been given then the changes are saved 
        /// to the database.
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns>redirects the user to the INDEX view if successful</returns>
        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int staffId)
        {
            var staff = await _context.Staff
                .Where(s => s.StaffId == staffId)
                .FirstOrDefaultAsync();
            if (staff != null)
            {
                _context.Staff.Remove(staff);
                // find all events this staff member is assigned to
                var eventsToUpdate = await _context.Staffing
                    .Include(s => s.Event)
                    .Where(s => s.StaffId == staff.StaffId)
                    .ToListAsync();



                // check if they currently have any events
                if (eventsToUpdate != null)
                {
                    // iterate through their events
                    foreach (var events in eventsToUpdate)
                    {
                        // load all staff assigned to this event other than the staff member for deletion
                        var staffAssignedToEvent = await _context.Staffing
                            .Include(s => s.Staff)
                            .Where(s => s.EventId == events.EventId && s.StaffId != staff.StaffId)
                            .ToListAsync();
                        // if no first aider is assigned set value to false
                        if (!staffAssignedToEvent.Any(s => s.Staff.FirstAidQualified))
                        {
                            events.Event.HasFirstAider = false;
                            _context.Update(events);
                        }
                        // remove the staff member assignment to the event
                        _context.Remove(events);
                    }
                    
                    
                }

                await _context.SaveChangesAsync();
                

            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region private methods
        /// <summary>
        /// This method is designed to determine wether a staff member exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if the staff member exists otherwise returns false</returns>
        private bool StaffExists(int id)
        {
          return _context.Staff.Any(e => e.StaffId == id);
        }

        #endregion
    }
}
