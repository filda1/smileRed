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

namespace smileRed.Backend.Controllers
{
    public class NutritionsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Nutritions
        public async Task<ActionResult> Index()
        {
            return View(await db.Nutritions.ToListAsync());
        }

        // GET: Nutritions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nutrition nutrition = await db.Nutritions.FindAsync(id);
            if (nutrition == null)
            {
                return HttpNotFound();
            }
            return View(nutrition);
        }

        // GET: Nutritions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Nutritions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "NutritionId,ProductId,Calories,TotalFat,Carbohydrates,Proteins")] Nutrition nutrition)
        {
            int productId = int.Parse(Request["ProductId"]);

            var existP = db.Nutritions.Where(n => n.ProductId == productId).FirstOrDefault();
            if (existP != null)
            {
                ViewBag.Error = "The product already exists!";
                return View();
            }

            if (ModelState.IsValid)
            {
                db.Nutritions.Add(nutrition);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(nutrition);
        }

        // GET: Nutritions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nutrition nutrition = await db.Nutritions.FindAsync(id);
            if (nutrition == null)
            {
                return HttpNotFound();
            }
            return View(nutrition);
        }

        // POST: Nutritions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "NutritionId,ProductId,Calories,TotalFat,Carbohydrates,Proteins")] Nutrition nutrition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nutrition).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(nutrition);
        }

        // GET: Nutritions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nutrition nutrition = await db.Nutritions.FindAsync(id);
            if (nutrition == null)
            {
                return HttpNotFound();
            }
            return View(nutrition);
        }

        // POST: Nutritions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Nutrition nutrition = await db.Nutritions.FindAsync(id);
            db.Nutritions.Remove(nutrition);
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
