using System.Web.Mvc;
using AutoMapper;
using SmartQueue.Authorization.Infrastructure;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Services;
using SmartQueue.Web.Models;

namespace SmartQueue.Web.Controllers
{
    public class PreferencesController : Controller
    {
        private readonly ISmartQueueServices _smartQueueServices;

        public PreferencesController(ISmartQueueServices smartQueueServices)
        {
            _smartQueueServices = smartQueueServices;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var user = User.Identity.GetUser();
            var preferences = _smartQueueServices.PreferencesService.GetUserPreferences(user);
            return View(Mapper.Map<OrderViewModel>(preferences));
        }

        [HttpPost]
        public ActionResult Index(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = User.Identity.GetUser();
                _smartQueueServices.PreferencesService
                    .UpdateUserPreferences(user, Mapper.Map<CoffeePreferences>(model));
                return RedirectToAction("Index", "Queue");
            }
            return View(model);
        }
    }
}