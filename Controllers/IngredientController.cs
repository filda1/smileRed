using smileRed.Backend.Models;
using smileRed.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace smileRed.Backend.Controllers
{
    public class IngredientController : Controller
    {
        LocalDataContext db = new LocalDataContext();
        // GET: Ingredient
        public ActionResult ViewIngredients()
        {
            var q = (from t in db.Products
                     join sc in db.Ingredients on t.ProductId equals sc.ProductId
                     join c in db.Groups on t.CategoryId equals c.CategoryId
                     orderby c.Description
                     select new { t.ProductId, t.CategoryId, c.Description, t.Name, sc.Ingredient, sc.IngredientsId });

            var productingredient = new List<ProductsIngredients>();
            foreach (var t in q)
            {
                productingredient.Add(new ProductsIngredients()
                {
                    ProductId = t.ProductId,
                    Name = t.Name,
                    Ingredient = t.Ingredient,
                    Category = t.Description,
                    CategoryId = t.CategoryId,
                    IngredientId = t.IngredientsId
                });
            }
            return View(productingredient);
        }

        [HttpGet]
        public ActionResult AddIngredient()
        {
            var ingredientView = new IngredientView();
            ingredientView.Product = new Product();
            ingredientView.Ingredients = new List<Ingredients>();
            Session["ingredientView"] = ingredientView;

            var product = db.Products.ToList();
            product.Add(new Product { ProductId = 0, Name = "Select a product..." });
            ViewBag.ProductID = new SelectList(
                product.OrderBy(p => p.Name),
                "ProductID", "Name", ingredientView.Product.ProductId);

            return View(ingredientView);
        }

        [HttpPost]
        public ActionResult AddIngredient(IngredientView ingredientView)
        {
            ingredientView = Session["ingredientView"] as IngredientView;
            int productId = int.Parse(Request["ProductId"]);
            string ingredient = Convert.ToString(Request["Ingredient.Ingredient"]);

            if (productId == 0)
            {
                ViewBag.Error = "You must select  a product";
                var products = db.Products.ToList();
                products.Add(new Product { ProductId = 0, Name = "Select a product..." });
                ViewBag.ProductID = new SelectList(
                    products.OrderBy(p => p.Name),
                    "ProductID", "Name", ingredientView.Product.ProductId);

                return View(ingredientView);
            }

            /*var existI = (from e in db.Ingredients
                          where e.Ingredient == ingredient && e.ProductId == productId
                          select e);*/

            var existI = db.Ingredients.Where(i => i.Ingredient == ingredient &&  
                       i.ProductId == productId).FirstOrDefault();

            if (existI != null)
            {
                ViewBag.Error = "The product ingredient already exists!";
                var products = db.Products.ToList();
                products.Add(new Product { ProductId = 0, Name = "Select a product..." });
                ViewBag.ProductID = new SelectList(
                    products.OrderBy(p => p.Name),
                    "ProductID", "Name", ingredientView.Product.ProductId);

                return View(ingredientView);
            };

            Ingredients ingre = new Ingredients
            {
                ProductId = productId,
                Ingredient = ingredient
            };

            db.Ingredients.Add(ingre);
            db.SaveChanges();

            ViewBag.Message = "Success!!";

            var product = db.Products.ToList();
            product.Add(new Product { ProductId = 0, Name = "Select a product..." });
            ViewBag.ProductID = new SelectList(
                product.OrderBy(p => p.Name),
                "ProductID", "Name", ingredientView.Product.ProductId);

            return View(ingredientView);
        }

        // GET: Products/Edit/5
        public ActionResult EditIngredients(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingredients mov = db.Ingredients.Find(id);
            if (mov == null)
            {
                return HttpNotFound();
            }
            return View(mov);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIngredients(Ingredients mov)
        {
            int productId = int.Parse(Request["ProductId"]);
            string ingredient = Convert.ToString(Request["Ingredient"]);
            var existI = db.Ingredients.Where(i => i.Ingredient == ingredient &&
                  i.ProductId == productId).FirstOrDefault();

            if (existI != null)
            {
                ViewBag.Error = "The product and ingredient already exists!";
                return View(mov);
            };

            if (ModelState.IsValid)
            {
                db.Entry(mov).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewIngredients");
            }
            return View(mov);
        }

        public ActionResult DeleteIngredients(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingredients mov = db.Ingredients.Find(id);
            if (mov == null)
            {
                return HttpNotFound();
            }
            return View(mov);
        }

        // POST: /Movies/Delete/5
        [HttpPost, ActionName("DeleteIngredients")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteIngredientsConfirmed(int id)
        {
            Ingredients mov = db.Ingredients.Find(id);
            db.Ingredients.Remove(mov);
            db.SaveChanges();
            return RedirectToAction("ViewIngredients");
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