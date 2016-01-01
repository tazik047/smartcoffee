using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartQueue.Model.Services;
using SmartQueue.Web.Models;

namespace SmartQueue.Web.Controllers
{
    public class CoffeeMachineController : Controller
    {
        private readonly ISmartQueueServices _smartQueueServices;

        public CoffeeMachineController(ISmartQueueServices smartQueueServices)
        {
            _smartQueueServices = smartQueueServices;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult AddCoffeeMachine()
        {
            var coffeMachine = new CreateCoffeeMachineViewModel();
            FillCompanies(coffeMachine);
            return View(coffeMachine);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult AddCoffeeMachine(CreateCoffeeMachineViewModel model)
        {

            FillCompanies(model);
            return View(model);
        }

        private void FillCompanies(CreateCoffeeMachineViewModel user)
        {
            user.Companies =
                _smartQueueServices.CompanyService.GetAllCompanies()
                    .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                    .ToList();
        }
    }
}