using smileRed.Backend.Models;
using smileRed.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace smileRed.Backend.Controllers
{
    public class FavoriteController : Controller
    {
        LocalDataContext db = new LocalDataContext();
        // GET: Favorite
        public ActionResult ViewFavorites()
        {
            var q = (from f in db.Favorites
                     join u in db.Users on f.Email equals u.Email
                     join p in db.Products on f.ProductId equals p.ProductId
                     select new
                     {
                         u.FirstName,
                         u.LastName,
                         f.FavoriteId,
                         f.FavoriteDate,
                         p.ProductId,
                         p.Name,
                         p.Description,
                         p.Image,
                         u.UserId,
                         f.Email
                     });

            var favoritesusersproducts = new List<FavoritesUsersProducts>();
            foreach (var t in q)
            {
                favoritesusersproducts.Add(new FavoritesUsersProducts()
                {
                    FavoriteId = t.FavoriteId,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    Name = t.Name,
                    Email = t.Email,
                    Description = t.Description,
                    //Image = t.Image,
                    FavoriteDate = t.FavoriteDate,
                    ProductId = t.ProductId,
                });
            }
            return View(favoritesusersproducts);
        }

        public ActionResult DeleteFavorites(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favorite mov = db.Favorites.Find(id);
            if (mov == null)
            {
                return HttpNotFound();
            }
            return View(mov);
        }


        [HttpPost, ActionName("DeleteFavorites")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFavoritesConfirmed(int id)
        {
            Favorite mov = db.Favorites.Find(id);
            db.Favorites.Remove(mov);
            db.SaveChanges();
            return RedirectToAction("ViewFavorites");
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
