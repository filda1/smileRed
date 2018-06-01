using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using smileRed.Backend.Models;
using smileRed.Domain;

namespace smileRed.Backend.Controllers
{
    public class UsersController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Users
        public async Task<ActionResult> Index()
        {
            var users = db.Users.Include(u => u.TypeofUser);
            return View(await users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            var ty = db.TypeofUsers.ToList();
            ty.Add(new TypeofUser { TypeofUserId = 0, TypeofUsers = "Select a type of user..." });
            ViewBag.TypeofUserId = new SelectList(
                 ty.OrderBy(c => c.TypeofUserId),
                "TypeofUserId", "TypeofUsers", "TypeofUsers");

            //ViewBag.TypeofUserId = new SelectList(db.TypeofUsers, "TypeofUserId", "TypeofUsers");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,TypeofUserId,FirstName,LastName,Email,Telephone,Address,Location,Code,Door,ImagePath,Active")] User user)
        {
            string email = Convert.ToString(Request["Email"]);
            var existU = db.Users.Where(u =>
                       u.Email == email).FirstOrDefault();

            if (existU != null)
            {
                ViewBag.Error = "The email already exists!";
                var ty = db.TypeofUsers.ToList();
                ty.Add(new TypeofUser { TypeofUserId = 0, TypeofUsers = "Select a type of user..." });
                ViewBag.TypeofUserId = new SelectList(
                     ty.OrderBy(c => c.TypeofUserId),
                    "TypeofUserId", "TypeofUsers", "TypeofUsers");

                return View();
            }


            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TypeofUserId = new SelectList(db.TypeofUsers, "TypeofUserId", "TypeofUsers", user.TypeofUserId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeofUserId = new SelectList(db.TypeofUsers, "TypeofUserId", "TypeofUsers", user.TypeofUserId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,TypeofUserId,FirstName,LastName,Email,Telephone,Address,Location,Code,Door,ImagePath,Active")] User user)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (ModelState.IsValid)
                        {
                            db.Entry(user).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            transaction.Commit();
                            return RedirectToAction("Index");
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.Error = "Error:" + ex.Message;
                    var ty = db.TypeofUsers.ToList();
                    ty.Add(new TypeofUser { TypeofUserId = 0, TypeofUsers = "Select a type of user..." });
                    ViewBag.TypeofUserId = new SelectList(
                         ty.OrderBy(c => c.TypeofUserId),
                        "TypeofUserId", "TypeofUsers", "TypeofUsers");

                    return View();
                }
            }
            
            ViewBag.TypeofUserId = new SelectList(db.TypeofUsers, "TypeofUserId", "TypeofUsers", user.TypeofUserId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
