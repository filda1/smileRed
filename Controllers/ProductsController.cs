﻿using System;
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
        public async Task<ActionResult> Create(Product product)
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

                return View(product);
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

                return View(product);
            }

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Groups, "CategoryId", "Description", product.CategoryId);
            return View(product);
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
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product product)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                  {
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

            ViewBag.CategoryId = new SelectList(db.Groups, "CategoryId", "Description", product.CategoryId);
            return View(product);
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
