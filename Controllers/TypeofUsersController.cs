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
    public class TypeofUsersController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: TypeofUsers
        public async Task<ActionResult> Index()
        {
            return View(await db.TypeofUsers.ToListAsync());
        }

        // GET: TypeofUsers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeofUser typeofUser = await db.TypeofUsers.FindAsync(id);
            if (typeofUser == null)
            {
                return HttpNotFound();
            }
            return View(typeofUser);
        }

        // GET: TypeofUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeofUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TypeofUserId,TypeofUsers,Description,Active")] TypeofUser typeofUser)
        {
            string nameTypeofUsers = Convert.ToString(Request["TypeofUsers"]);
            var existT = db.TypeofUsers.Where(ty =>
                       ty.TypeofUsers == nameTypeofUsers).FirstOrDefault();

            if (existT != null)
            {
                ViewBag.Error = "The Type of users already exist!";
                return View();
            }

            if (ModelState.IsValid)
            {
                db.TypeofUsers.Add(typeofUser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(typeofUser);
        }

        // GET: TypeofUsers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeofUser typeofUser = await db.TypeofUsers.FindAsync(id);
            if (typeofUser == null)
            {
                return HttpNotFound();
            }
            return View(typeofUser);
        }

        // POST: TypeofUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TypeofUserId,TypeofUsers,Description,Active")] TypeofUser typeofUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeofUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(typeofUser);
        }

        // GET: TypeofUsers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeofUser typeofUser = await db.TypeofUsers.FindAsync(id);
            if (typeofUser == null)
            {
                return HttpNotFound();
            }
            return View(typeofUser);
        }

        // POST: TypeofUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TypeofUser typeofUser = await db.TypeofUsers.FindAsync(id);
            db.TypeofUsers.Remove(typeofUser);
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
