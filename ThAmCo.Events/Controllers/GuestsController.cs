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
    public class GuestsController : Controller
    {
        private readonly EventsDBContext _context;

        public GuestsController(EventsDBContext context)
        {
            _context = context;
        }

        #region Index

        /// <summary>
        /// This method is used to to find all guests currently contained within
        /// the database and add them to a collection once all have been retrieved
        /// they are then transformed into a guest view model and passed to the view 
        /// to display to the user
        /// </summary>
        /// <returns>a collection of guest view models to the view</returns>
        // GET: Guests
        public async Task<IActionResult> Index()
        {
            var allGuests = await _context.Guest.ToListAsync();
            var VM = allGuests.Select(g => new GuestViewModel(g));
            return View(VM);
        }

        #endregion

        #region Details

        /// <summary>
        /// This method is designed to display the details of a specific guest. 
        /// This is achieved by finding the corresponding guest to the guest ID provided to the method. 
        /// Once the guest has been found the guest is then transformed into a  guest view model which 
        /// is then returned to the view.
        /// </summary>
        /// <param name="guestId"></param>
        /// <returns> a guest view model to the DETAILS view if successful, if unsuccessful not found is returned</returns>
        // GET: Guests/Details/5
        public async Task<IActionResult> Details(int guestId)
        {
            if (_context.Guest == null)
            {
                return NotFound();
            }
            // find the corresponding guest to the guest ID provided to the method
            var guest = await _context.Guest.Include(g => g.Events).ThenInclude(gb => gb.Event)
                .FirstOrDefaultAsync(m => m.GuestId == guestId);
            // if guest doesnt exist return not found
            if (guest == null)
            {
                return NotFound();
            }
            // else create the guest view model
            var listofevents = guest.Events;
            var VM = new GuestViewModel(guest);
            VM.Bookings = guest.Events.Select(e => new GuestBookingViewModel(e)).ToList();

            return View(VM);
        }
        #endregion

        #region Create
        /// <summary>
        /// This method is used to return the CREATE view to the user so a new guest can be created 
        /// </summary>
        /// <returns> the CREATE view</returns>
        // GET: Guests/Create
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// This method is used to create a new guest from the details provided in the view once the guest is created it is then saved to the database
        /// </summary>
        /// <param name="VM"></param>
        /// <returns>Redirects the user to the INDEX view if successful, otherwise the invalid guest is redisplayed to the user to make the relevant changes</returns>
        // POST: Guests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GuestViewModel VM)
        {
            var guest = new Guest
            {
                Forename = VM.Forename,
                Surname = VM.Surname,
                Email = VM.Email,
                Address = VM.Address,
                Telephone = VM.Telephone
            };
            if (ModelState.IsValid)
            {
                _context.Add(guest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(VM);
        }
        #endregion

        #region AnonymiseGuest
        /// <summary>
        /// This method is used to locate the corresponding guest to the specified guest ID
        /// in the parameters. Once the guest has been found the guest is then transformed 
        /// into a guest view model and is returned to the view for confirmation if the user 
        /// wants to annoymise the guest
        /// </summary>
        /// <param name="guestId"></param>
        /// <returns>returns the relevant guest view model to the ANONYMISEGUEST view. if no guest is found  NotFound is returned</returns>
        // GET: Guests/Create
        public async Task<IActionResult> AnonymiseGuest(int guestId)
        {
            // find the guest
            var guestToAnonymise = await _context.Guest
                .Where(g => g.GuestId == guestId).
                FirstOrDefaultAsync();
            if (guestToAnonymise == null)
            {
                return NotFound();
            };
            var VM = new GuestViewModel(guestToAnonymise);
            return View(VM);
        }

        /// <summary>
        /// This method is used once the user has confirmed that they wish to annonymise the relevant guest, 
        /// this is achieved by finding the corresponding guest in the database to the one specified by the 
        /// guest ID in the parameters. Once the guest has been found the guests name is overwritten with 
        /// a '#' symbol and all other non required fields are removed from the database entirely.
        /// </summary>
        /// <param name="guestId"></param>
        /// <returns></returns>
        /// // POST: Guests/Create
        public async Task<IActionResult> AnonymiseGuestData(int guestId)
        {
            // find the guest in the database
            var guestToAnonymise = await _context.Guest
                .Where(g => g.GuestId == guestId)
                .FirstOrDefaultAsync();
            if (guestToAnonymise == null)
            {
                return NotFound();
            }
            // remove and overwrite required guest details
            guestToAnonymise.Forename = "#";
            guestToAnonymise.Surname = "#";
            guestToAnonymise.Telephone = null;
            guestToAnonymise.Address = null;
            guestToAnonymise.Email = null;
            // save changes
            try
            {
                _context.Update(guestToAnonymise);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(ModelState);
            }


            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit

        /// <summary>
        /// This method is designed to find the  corresponding guest to the guest ID
        /// specified in the parameters once the guest has been found in the database
        /// the guest is then transformed into a guest view model and returned to EDIT view
        /// </summary>
        /// <param name="guestId"></param>
        /// <returns>a guest view model to the view if successful otherwise returns not found</returns>
        // GET: Guests/Edit/5
        public async Task<IActionResult> Edit(int guestId)
        {
            //if (_context.Guest == null)
            //{
            //    return NotFound();
            //}

            var guest = await _context.Guest
                .Where(g => g.GuestId == guestId)
                .FirstOrDefaultAsync();

            if (guest == null)
            {
                return NotFound();
            }
            // else transform to view model and return the view
            var VM = new GuestViewModel(guest);
            return View(VM);
        }

        /// <summary>
        /// This method is designed to update the changes that the user has applied the user. 
        /// This is achieved by locating the guest in the database then updating the relevant 
        /// fields and then finally applying these changes and saving to the database
        /// </summary>
        /// <param name="guest"></param>
        /// <returns> redirects the the user to the INDEX view if successful otherwise the unvalid guest update is returned</returns>
        // POST: Guests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GuestViewModel VM)
        {

            if (ModelState.IsValid)
            {
                var guest = await _context.Guest
                    .Where(g => g.GuestId == VM.GuestId)
                    .FirstOrDefaultAsync();
                // apply changes
                guest.Forename = VM.Forename;
                guest.Surname = VM.Surname;
                guest.Email = VM.Email;
                guest.Address = VM.Address;
                guest.Telephone = VM.Telephone;
                try
                {
                    _context.Update(guest);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuestExists(guest.GuestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            return View(VM);
        }
        #endregion

        #region Delete

        /// <summary>
        /// This method is used to find the guest in the database that corresponds to the guest ID 
        /// provided in the parameters and display the guest details to the user and allow the user 
        /// to confirm the deletion
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a guest view model to the DELETE view if successful other wise returns not found</returns>
        // GET: Guests/Delete/5
        public async Task<IActionResult> Delete(int guestId)
        {
            if (_context.Guest == null)
            {
                return NotFound();
            }
            // find the guest in the database
            var guest = await _context.Guest
                .FirstOrDefaultAsync(m => m.GuestId == guestId);
            // if null return not found
            if (guest == null)
            {
                return NotFound();
            }
            // otherwise transform the guest into a guest view model and return to the view
            var VM = new GuestViewModel(guest);

            return View(VM);
        }

        /// <summary>
        /// This method is used to delete a user from the data base once confirmation has been given. 
        /// This achieved by locating the guest that corresponds to the guest ID supplied in the parameters 
        /// once found the guest is remoed and the changes are then saved in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns> redirects the user to the INDEX view id successful otherwise not found is displayed</returns>
        // POST: Guests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int guestId)
        {
            if (_context.Guest == null)
            {
                // preferablly return a something went wrong page
                return Problem("Entity set 'EventsDBContext.Guest'  is null.");
            }

            var guest = await _context.Guest
                .Where(g => g.GuestId == guestId)
                .FirstOrDefaultAsync();

            // check if exists 
            if (guest == null)
            {
                return NotFound();
            }
            // else delete the guest and save changes
            _context.Guest.Remove(guest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region private methods
        /// <summary>
        /// This method is used to determine if a guest exists by searching the database for a guest that correspondes to the guest ID provided in the parameters 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if the guest exists otherwise return false</returns>
        private bool GuestExists(int id)
        {
            return _context.Guest.Any(e => e.GuestId == id);
        }

        #endregion
    }
}
