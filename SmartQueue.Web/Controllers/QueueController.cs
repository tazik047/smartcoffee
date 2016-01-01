using System.Web.Mvc;
using SmartQueue.Web.Models;
using SmartQueue.Model.Services;
using SmartQueue.Authorization.Infrastructure;

namespace SmartQueue.Web.Controllers
{
    public class QueueController : Controller
    {
        private readonly ISmartQueueServices _service;

        public QueueController(ISmartQueueServices service)
        {
            _service = service;
        }

        // GET: Queue
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddToQueue()
        {
            var queue = new QueueCreationViewModel();
            var prefs = _service.PreferencesService.GetUserPreferences(User.Identity.GetUser());
            queue.Order = AutoMapper.Mapper.Map<OrderViewModel>(prefs);
            return View(queue);
        }
    }
}