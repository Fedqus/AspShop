using kurs.Attributes;
using kurs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kurs.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DatabaseBackupManager _backupManager;

        public DashboardController(DatabaseBackupManager backupManager)
        {
            _backupManager = backupManager;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
        [AjaxOnly]
        [Authorize(Roles = "Admin")]
        public IActionResult Backups()
        {
            return View(_backupManager.GetBackups());
        }
        [AjaxOnly]
        [Authorize(Roles = "Admin")]
        public IActionResult Backup()
        {
            _backupManager.Backup();
            return Ok();
        }
        [AjaxOnly]
        [Authorize(Roles = "Admin")]
        public IActionResult Restore(string filename)
        {
            _backupManager.Restore(filename);
            return Ok();
        }
        [AjaxOnly]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteBackup(string filename)
        {
            _backupManager.DeleteBackup(filename);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Categories()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Companies()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Products()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult TagGroups()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Tags()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Countries()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Cities()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Shippings()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Reviews()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Orders()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Users()
        {
            return View();
        }
    }
}
