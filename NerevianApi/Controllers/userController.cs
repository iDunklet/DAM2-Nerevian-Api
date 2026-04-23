using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NerevianApi.Controllers
{
    public class userController : Controller
    {
        // GET: userController
        public ActionResult Index()
        {
            return View();
        }

        // GET: userController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: userController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: userController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: userController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: userController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: userController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: userController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
