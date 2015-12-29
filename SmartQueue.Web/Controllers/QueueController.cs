using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartQueue.Web.Models;

namespace SmartQueue.Web.Controllers
{
    public class QueueController : Controller
    {
        // GET: Queue
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddToQueue(QueueCreationViewModel model)
        {
            return View();
        }
    }
}