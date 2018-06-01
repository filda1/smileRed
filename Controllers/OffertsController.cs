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
    public class OffertsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Offerts
        public async Task<ActionResult> Index()
        {
            return View(await db.Offerts.ToListAsync());
        }

        // GET: Offerts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offert offert = await db.Offerts.FindAsync(id);
            if (offert == null)
            {
                return HttpNotFound();
            }
            return View(offert);
        }

        // GET: Offerts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Offerts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OffertId,Name,Description,Price,Image,Stock,StartDate,EndofDate,IsActive,Remarks")] Offert offert)
        {
            if (ModelState.IsValid)
            {
                db.Offerts.Add(offert);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(offert);
        }

        // GET: Offerts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offert offert = await db.Offerts.FindAsync(id);
            if (offert == null)
            {
                return HttpNotFound();
            }
            return View(offert);
        }

        // POST: Offerts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OffertId,Name,Description,Price,Image,Stock,StartDate,EndofDate,IsActive,Remarks")] Offert offert)
        {
            if (ModelState.IsValid)
            {
                db.Entry(offert).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(offert);
        }

        // GET: Offerts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offert offert = await db.Offerts.FindAsync(id);
            if (offert == null)
            {
                return HttpNotFound();
            }
            return View(offert);
        }

        // POST: Offerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Offert offert = await db.Offerts.FindAsync(id);
            db.Offerts.Remove(offert);
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
