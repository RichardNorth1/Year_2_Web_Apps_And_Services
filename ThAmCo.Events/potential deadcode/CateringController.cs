using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ThAmCo.Events.DTOs;
using ThAmCo.Events.Models.DTOs;

namespace ThAmCo.Events.Controllers
{
    public class CateringController : Controller
    {
        private readonly HttpClient client;
        public CateringController()
        {
            client = new HttpClient();
            client.BaseAddress = new System.Uri("https://localhost:7173/");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }

        // GET: CateringController
        public async Task<ActionResult> Index()
        {

            IEnumerable<FullFoodBookingDto> FoodBooking = new List<FullFoodBookingDto>();

            HttpResponseMessage response = await client.GetAsync("api/FoodBookings/FullBooking");
            if (response.IsSuccessStatusCode)
            {

                FoodBooking = await response.Content.ReadAsAsync<IEnumerable<FullFoodBookingDto>>();
            }
            else
            {
                Debug.WriteLine("Index received a bad response from the web service.");
            }

            var VM = FoodBooking.Select(fb => new FoodBookingViewModel(fb)).ToList();
            
            return View(VM);
        }

        // GET: CateringController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var FullFoodBooking = new FullFoodBookingDto();

            HttpResponseMessage response = await client.GetAsync("api/FoodBookings/" + id);
            if (response.IsSuccessStatusCode)
            {
                FullFoodBooking = await response.Content.ReadAsAsync<FullFoodBookingDto>();
            }
            else
            {
                Debug.WriteLine("Index received a bad response from the web service.");
            }
            var VM = new FoodBookingViewModel(FullFoodBooking);
            return View(VM);
        }

        //// GET: CateringController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: CateringController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(IndexAsync));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: CateringController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: CateringController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(IndexAsync));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: CateringController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: CateringController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(IndexAsync));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
