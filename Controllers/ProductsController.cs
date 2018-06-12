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

namespace smileRed.Backend.Controllers
{
    public class ProductsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var ty = db.Groups.ToList();
            ty.Add(new Group { CategoryId = 0, Description = "Select a product..." });
            ViewBag.CategoryId = new SelectList(
                 ty.OrderBy(c => c.CategoryId),
                "CategoryId", "Description", "Description");

            //ViewBag.CategoryId = new SelectList(db.Groups, "CategoryId", "Description");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductView view)
        {
            string nameProduct = Convert.ToString(Request["Name"]);
            int categoryId = int.Parse(Request["CategoryId"]);

            if (categoryId == 0)
            {
                ViewBag.Error = "You must select  a product";
                var ty = db.Groups.ToList();
                ty.Add(new Group { CategoryId = 0, Description = "Select a product..." });
                ViewBag.CategoryId = new SelectList(
                     ty.OrderBy(c => c.CategoryId),
                    "CategoryId", "Description", "Description");

                return View(view);
            }

            var existPC = db.Products.Where(pc => 
                       pc.Name == nameProduct ).FirstOrDefault();

            if (existPC != null)
            {
                ViewBag.Error = "The product already exist!";
                var ty = db.Groups.ToList();
                ty.Add(new Group { CategoryId = 0, Description = "Select a product..." });
                ViewBag.CategoryId = new SelectList(
                     ty.OrderBy(c => c.CategoryId),
                    "CategoryId", "Description", "Description");

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
                var product = ToProduct(view);
                product.Image = pic;

                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Groups, "CategoryId", "Description", view.CategoryId);
            return View(view);
        }

        private Product ToProduct(ProductView view)
        {
            return new Product
            {
                ProductId = view.ProductId,
                Name = view.Name,
                CategoryId = view.CategoryId,
                Description = view.Description,
                Price = view.Price,
                VAT = view.VAT,
                Image = view.Image,
                IsActive = view.IsActive,
                Stock = view.Stock,
                Remarks = view.Remarks,
            };
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            string name = db.Products.Where(p => p.ProductId == id).FirstOrDefault().Name;
            ViewBag.Name = name;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Groups, "CategoryId", "Description", product.CategoryId);
            var view = ToView(product);
            return View(view);
        }

        private ProductView ToView(Product product)
        {
            return new ProductView
            {
                ProductId = product.ProductId,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Price = product.Price,
                VAT = product.VAT,
                Image = product.Image,
                IsActive = product.IsActive,
                Stock = product.Stock,
                Remarks = product.Remarks,
            };
        }


        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductView view)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                  if (ModelState.IsValid)
                  {
                        var pic = view.Image;
                        var folder = "~/Content/Images";

                        if (view.ImageFile != null)
                        {
                            pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                            pic = string.Format("{0}/{1}", folder, pic);
                        }

                        var product = ToProduct(view);
                        product.Image = pic;
                        db.Entry(product).State = EntityState.Modified;
                     await db.SaveChangesAsync();
                     transaction.Commit();
                      return RedirectToAction("Index");
                   }
                  
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.Error = "Error:" + ex.Message;
                    var ty = db.Groups.ToList();
                    ty.Add(new Group { CategoryId = 0, Description = "Select a product..." });
                    ViewBag.CategoryId = new SelectList(
                         ty.OrderBy(c => c.CategoryId),
                        "CategoryId", "Description", "Description");

                    return View();
                }
            }

            ViewBag.CategoryId = new SelectList(db.Groups, "CategoryId", "Description", view.CategoryId);
            return View(view);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
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
