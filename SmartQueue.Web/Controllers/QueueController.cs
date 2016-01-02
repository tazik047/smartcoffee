using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using SmartQueue.Web.Models;
using SmartQueue.Model.Services;
using SmartQueue.Authorization.Infrastructure;
using SmartQueue.Model.Entities;

namespace SmartQueue.Web.Controllers
{
    public class QueueController : Controller
    {
        private readonly ISmartQueueServices _service;

        public QueueController(ISmartQueueServices service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            if (!_service.QueueService.IsWait(User.Identity.GetUser().Id))
            {
                return RedirectToAction("AddToQueue");
            }
            return View();
        }

        [HttpGet]
        public ActionResult AddToQueue()
        {
            if (_service.QueueService.IsWait(User.Identity.GetUser().Id))
            {
                return RedirectToAction("Index");
            }
            var prefs = _service.PreferencesService.GetUserPreferences(User.Identity.GetUser());
            var order = Mapper.Map<OrderViewModel>(prefs);
            FillCoffeeMachines(order);
            return View(order);
        }

        [HttpPost]
        public ActionResult AddToQueue(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = Mapper.Map<Order>(model);
                order.UserId = User.Identity.GetUser().Id;
                _service.QueueService.AddToQueue(order);
                return RedirectToAction("Index");
            }
            FillCoffeeMachines(model);
            return View(model);
        }

        private void FillCoffeeMachines(OrderViewModel model)
        {
            model.CoffeeMachines = _service.CoffeeMachineService.GetAllCoffeeMachines(User.Identity.GetUser().CompanyId.Value)
                    .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                    .ToList();
        }
    }
}