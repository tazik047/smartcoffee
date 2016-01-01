using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using SmartQueue.Authorization.Infrastructure;
using SmartQueue.Model.Entities;
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
            if (ModelState.IsValid)
            {
                var coffeeMachine = Mapper.Map<CoffeeMachine>(model);
                _smartQueueServices.CoffeeMachineService.AddCoffeeMachie(coffeeMachine);
                return RedirectToAction("AllCoffeeMachine");
            }
            FillCompanies(model);
            return View(model);
        }

        [Authorize(Roles = "Administrator,Director")]
        public ActionResult AllCoffeeMachine()
        {
            IEnumerable<CoffeeMachine> coffeeMachines;
            if (User.IsInRole("Director"))
            {
                coffeeMachines =
                    _smartQueueServices.CoffeeMachineService.GetAllCoffeeMachines(
                        User.Identity.GetUser().CompanyId.Value);
            }
            else
            {
                coffeeMachines = _smartQueueServices.CoffeeMachineService.GetAllCoffeeMachines();
            }

            return View(Mapper.Map<IEnumerable<CoffeeMachineViewModel>>(coffeeMachines));
        }

        [HttpGet]
        [Authorize(Roles = "Director")]
        public ActionResult Edit(long id)
        {
            var result = _smartQueueServices.CoffeeMachineService.GetCoffeeMachie(id);
            var coffeeMachine = Mapper.Map<EditCoffeeMachineViewModel>(result);
            FillCompanies(coffeeMachine, User.Identity.GetUser().CompanyId.Value);
            return View(coffeeMachine);
        }

        [HttpPost]
        [Authorize(Roles = "Director")]
        public ActionResult Edit(EditCoffeeMachineViewModel model)
        {
            if (ModelState.IsValid)
            {
                var coffeeMachine = Mapper.Map<CoffeeMachine>(model);
                _smartQueueServices.CoffeeMachineService.EditCoffeeMachie(coffeeMachine);

                return RedirectToAction("AllCoffeeMachine");
            }
            FillCompanies(model, User.Identity.GetUser().CompanyId.Value);

            return View(model);
        }

        private void FillCompanies(EditCoffeeMachineViewModel coffeeMachine, long companyId)
        {
            coffeeMachine.Employees =
                _smartQueueServices.CompanyService.GetAllEmployees(companyId)
                    .Where(e => e.IsActive)
                    .Select(c => new SelectListItem {Text = c.Surname + " " + c.Name, Value = c.Id.ToString()})
                    .ToList();
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