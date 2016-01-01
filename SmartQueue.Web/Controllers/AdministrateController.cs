using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            TempData["Activate"] = "activated";
            return RedirectToAction("ActivateCompany");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ViewAllCompanies()
        {
            var companies = _smartQueueServices.CompanyService.GetAllCompanies();
            return View();
        }

        [Authorize(Roles = "Administrator,Director")]
        public ActionResult ActivateAllEmployees(long companyId)
        {
            if (User.IsInRole("Director"))
            {
                if (companyId != User.Identity.GetUser().CompanyId.Value)
                {
                    throw new UnauthorizedAccessException();
                }

            }
            return View();
        }
    }
}