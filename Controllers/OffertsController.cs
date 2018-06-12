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
using smileRed.Backend.Helpers;
using System.Globalization;

namespace smileRed.Backend.Controllers
{
    public class OffertsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Offerts
        public async Task<ActionResult> Index()
        {
            var offerts = db.Offerts.Include(o => o.Product);
            return View(await offerts.ToListAsync());
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
            var pro = db.Products.ToList();
            pro.Add(new Product { ProductId = 0, Name = "Select a product..." });
            ViewBag.ProductId = new SelectList(
                 pro.OrderBy(c => c.ProductId),
                "ProductId", "Name", "Name");

            //ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name");
            return View();
        }

        // POST: Offerts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( OffertView view)
        {
            DateTime thisTime = DateTime.Now;
            TimeZoneInfo InfoZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            DateTime TimePT = TimeZoneInfo.ConvertTime(thisTime, TimeZoneInfo.Local, InfoZone);

            int productId = int.Parse(Request["ProductId"]);
            if (productId == 0)
            {
                ViewBag.Error = "You must select  a product!";
                var pro = db.Products.ToList();
                pro.Add(new Product { ProductId = 0, Name = "Select a product..." });
                ViewBag.ProductId = new SelectList(
                     pro.OrderBy(c => c.ProductId),
                    "ProductId", "Name", "Name");

                return View(view);
            }
            try
            {
                DateTime startDate = Convert.ToDateTime(Request["StartDate"]);
                DateTime endofDate = Convert.ToDateTime(Request["EndofDate"]);

                if (startDate < TimePT || endofDate < TimePT)
                {
                    ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", view.ProductId);
                    ViewBag.Error = "The date can not be less than today!";

                    return View(view);
                }
            }
            catch
            {
                var pro = db.Products.ToList();
                pro.Add(new Product { ProductId = 0, Name = "Select a product..." });
                ViewBag.ProductId = new SelectList(
                     pro.OrderBy(c => c.ProductId),
                    "ProductId", "Name", "Name");

                return View(view);
            }

            var existP = db.Offerts.Where(o =>
                       o.ProductId == productId).FirstOrDefault();

            if (existP != null)
            {
                ViewBag.Error = "The product already exists!";
                var pro = db.Products.ToList();
                pro.Add(new Product { ProductId = 0, Name = "Select a product..." });
                ViewBag.ProductId = new SelectList(
                     pro.OrderBy(c => c.ProductId),
                    "ProductId", "Name", "Name");

                return View(view);
            }

            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Images";

                if (view.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }
                var offert = ToOffert(view);
                offert.Image = pic;

                db.Offerts.Add(offert);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", view.ProductId);
            return View(view);
        }

        private Offert ToOffert(OffertView view)
        {
            return new Offert
            {
                OffertId = view.OffertId,
                ProductId = view.ProductId,
                Offer = view.Offer,
                Description = view.Description,
                Image = view.Image,
                StartDate = view.StartDate,
                EndofDate = view.EndofDate,
                IsActive = view.IsActive,
                Remarks = view.Remarks,
            };
        }

        // GET: Offerts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            string name = db.Offerts.Where(o => o.OffertId == id).FirstOrDefault().Product.Name;
            ViewBag.Name = name;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offert offert = await db.Offerts.FindAsync(id);
            if (offert == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", offert.ProductId);
            var view = ToView(offert);
            return View(view);
        }

        private object ToView(Offert offert)
        {
            return new OffertView
            {
                OffertId = offert.OffertId,
                ProductId = offert.ProductId,
                Offer = offert.Offer,
                Description = offert.Description,
                Image = offert.Image,
                StartDate = offert.StartDate,
                EndofDate = offert.EndofDate,
                IsActive = offert.IsActive,
                Remarks = offert.Remarks,
            };
        }
        // POST: Offerts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OffertView view)
        {
            DateTime thisTime = DateTime.Now;
            TimeZoneInfo InfoZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            DateTime TimePT = TimeZoneInfo.ConvertTime(thisTime, TimeZoneInfo.Local, InfoZone);

            var stardate = view.StartDate;
            var enddate = view.EndofDate;
            if (stardate < TimePT || enddate < TimePT)
            {
                ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", view.ProductId);
                ViewBag.Error = "The date can not be less than today!";

                return View(view);
            }

            if (ModelState.IsValid)
            {
                var pic = view.Image;
                var folder = "~/Content/Images";

                if (view.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var offert = ToOffert(view);
                offert.Image = pic;

                db.Entry(offert).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", view.ProductId);
            return View(view);
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
