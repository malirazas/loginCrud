using loginCrud.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace loginCrud.Controllers
{
    public class LoginController : Controller
    {
        private EmployeeEntities db = new EmployeeEntities();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User emp)
        {
            var data = db.Users.Where(model => model.Email == emp.Email && model.Password == emp.Password).FirstOrDefault();
            if (data != null)
            {
                Session["UserID"] = emp.ID.ToString();
                Session["Name"] = emp.Name;
                Session["Email"] = emp.Email.ToString();
                TempData["Message"] = "Login In Successfull!";
                return RedirectToAction("Dashboard", "Login");

            }
            else
            {
                ViewBag.Message = "Login In Failed!";
                return View();
            }
            //return View();
        }
        public ActionResult Dashboard()
        {
            if (Session["UserID"] != null)
            {
                return View(db.Users.ToList());
            }
            else
            {
                TempData["Message"] = "Please Login First!";
                return RedirectToAction("Index");
            }
            //return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup([Bind(Include = "ID,Name,Age,Gender,Email,Password,ConfirmPassword")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                int a = db.SaveChanges();
                if (a > 0)
                {
                    ModelState.Clear();
                    TempData["Message"] = "Registration Successfull! Now Login";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Error Signing UP!!! Contact Admin";
                    return View();
                }
            }

            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Age,Gender,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            TempData["Message"] = "Record Deleted Successfully";
            return RedirectToAction("Dashboard", "Login");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
