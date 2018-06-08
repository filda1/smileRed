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
    public class AdmixturesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Admixtures
        public async Task<ActionResult> Index()
        {
            var admixtures = db.Admixtures.Include(a => a.Product);
            return View(await admixtures.ToListAsync());
        }

        // GET: Admixtures/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admixtures admixtures = await db.Admixtures.FindAsync(id);
            if (admixtures == null)
            {
                return HttpNotFound();
            }
            return View(admixtures);
        }

        // GET: Admixtures/Create
        public ActionResult Create()
        {
            var products = db.Products.ToList();
            products.Add(new Product { ProductId = 0, Name = "Select a product..." });
            ViewBag.ProductId = new SelectList(
                products.OrderBy(p => p.ProductId),
                "ProductID", "Name", "Name");

            //ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name");
            return View();
        }

        // POST: Admixtures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IngredientsId,ProductId,Ingredient")] Admixtures admixtures)
        {
            string ingredient = Convert.ToString(Request["Ingredient"]);
            int productId = int.Parse(Request["ProductId"]);
           

            if (productId == 0)
            {
                ViewBag.Error = "You must select  a product";
                var products = db.Products.ToList();
                products.Add(new Product { ProductId = 0, Name = "Select a product..." });
                ViewBag.ProductID = new SelectList(
                    products.OrderBy(p => p.Name),
                    "ProductID", "Name", "name");

                return View();
            }

            var existI = db.Admixtures.Where(i =>
                       i.Ingredient == ingredient).FirstOrDefault();

            if (existI != null)
            {              
                ViewBag.Error = "The ingredient already exists!";
                var products = db.Products.ToList();
                products.Add(new Product { ProductId = 0, Name = "Select a product..." });
                ViewBag.ProductID = new SelectList(
                    products.OrderBy(p => p.Name),
                    "ProductID", "Name", "Name");
                return View();
            }

            if (ModelState.IsValid)
            {
                db.Admixtures.Add(admixtures);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", admixtures.ProductId);
            return View(admixtures);
        }

        // GET: Admixtures/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            string name = db.Admixtures.Where(a => a.IngredientsId == id).FirstOrDefault().Product.Name;
            ViewBag.Name = name;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admixtures admixtures = await db.Admixtures.FindAsync(id);
            if (admixtures == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", admixtures.ProductId);
            return View(admixtures);
        }

        // POST: Admixtures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IngredientsId,ProductId,Ingredient")] Admixtures admixtures)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(admixtures).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.Error = "Error:" + ex.Message;
                    ViewBag.Message = "The ingredient must be unique";
                    var products = db.Products.ToList();
                    products.Add(new Product { ProductId = 0, Name = "Select a product..." });
                    ViewBag.ProductID = new SelectList(
                        products.OrderBy(p => p.Name),
                        "ProductID", "Name", "Name");
                    return View();
                }
            }

            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", admixtures.ProductId);
            return View(admixtures);
        }

        // GET: Admixtures/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admixtures admixtures = await db.Admixtures.FindAsync(id);
            if (admixtures == null)
            {
                return HttpNotFound();
            }
            return View(admixtures);
        }

        // POST: Admixtures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Admixtures admixtures = await db.Admixtures.FindAsync(id);
            db.Admixtures.Remove(admixtures);
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
