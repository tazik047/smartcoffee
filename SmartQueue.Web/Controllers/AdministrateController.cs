using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using SmartQueue.Authorization.Infrastructure;
using SmartQueue.Model.Services;
using SmartQueue.Web.Models;

namespace SmartQueue.Web.Controllers
{
    public class AdministrateController : Controller
    {
        private readonly ISmartQueueServices _smartQueueServices;

        public AdministrateController(ISmartQueueServices smartQueueServices)
        {
            _smartQueueServices = smartQueueServices;
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ActivateCompany()
        {
            var companies = _smartQueueServices.CompanyService.GetNotActiveCompany();

            return View(Mapper.Map<IEnumerable<NotActiveCompanyViewModel>>(companies));
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ActivateConcreteCompany(long id)
        {
            _smartQueueServices.CompanyService.ActivateCompany(id);
            TempData["Activated"] = "activated";

            return RedirectToAction("ActivateCompany");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ViewAllCompanies()
        {
            var companies = _smartQueueServices.CompanyService.GetAllCompanies();

            return View(Mapper.Map<IEnumerable<AllCompaniesViewModel>>(companies));
        }

        [Authorize(Roles = "Administrator,Director")]
        public ActionResult ActivateAllEmployees(long id)
        {
            if (User.IsInRole("Director"))
            {
                if (id != User.Identity.GetUser().CompanyId.Value)
                {
                    throw new UnauthorizedAccessException();
                }
            }
            _smartQueueServices.CompanyService.ActivateAllEmployeesCompany(id);
            if (User.IsInRole("Director"))
            {
                return RedirectToAction("AllEmployees");
            }
            return RedirectToAction("ViewAllCompanies");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult BanCompany(long id)
        {
            _smartQueueServices.CompanyService.BanCompany(id);

            return RedirectToAction("ViewAllCompanies");
        }

        [Authorize(Roles = "Director")]
        public ActionResult AllEmployees()
        {
            long companyId = User.Identity.GetUser().CompanyId.Value;
            var employees = _smartQueueServices.CompanyService.GetAllEmployees(companyId);

            return View(Mapper.Map<IEnumerable<UserViewModel>>(employees));
        }

        [Authorize(Roles = "Director")]
        public ActionResult ActivateEmployee()
        {
            long companyId = User.Identity.GetUser().CompanyId.Value;
            var employees = _smartQueueServices.CompanyService.GetAllNotActiveEmployees(companyId);

            return View(Mapper.Map<IEnumerable<UserViewModel>>(employees));
        }

        [Authorize(Roles = "Director")]
        public ActionResult ActivateConcreteEmployee(long id)
        {
            _smartQueueServices.UserService.ActivateUser(id);
            return RedirectToAction("AllEmployees");
        }

        [Authorize(Roles = "Director")]
        public ActionResult DeactivateEmployee(long id)
        {
            _smartQueueServices.UserService.BanUser(id);
            return RedirectToAction("AllEmployees");
        }
    }
}