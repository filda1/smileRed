using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using smileRed.API.Models;
using smileRed.Domain;

namespace smileRed.API.Controllers
{
    public class FavoritesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Favorites
        public IQueryable<Favorite> GetFavorites()
        {
            return db.Favorites;
        }

        // GET: api/Favorites/5
        // http://localhost:50577/api/Favorites/id?id=Gordon@defens.com
        [ResponseType(typeof(Favorite))]
        public async Task<IHttpActionResult> GetFavorite(string id)
        {
            var q = (from f in db.Favorites
                     join u in db.Users on f.Email equals u.Email
                     join p in db.Products on f.ProductId equals p.ProductId
                     where u.Email == id
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

            //Favorite favorite = await db.Favorites.FindAsync(id);
            if (favoritesusersproducts == null)
            {
                return NotFound();
            }

            return Ok(favoritesusersproducts);
        }

        // PUT: api/Favorites/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFavorite(int id, Favorite favorite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != favorite.FavoriteId)
            {
                return BadRequest();
            }

            db.Entry(favorite).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoriteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Favorites
        [ResponseType(typeof(Favorite))]
        public async Task<IHttpActionResult> PostFavorite(Favorite favorite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Favorites.Add(favorite);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = favorite.FavoriteId }, favorite);
        }

        // DELETE: api/Favorites/5
        [ResponseType(typeof(Favorite))]
        public async Task<IHttpActionResult> DeleteFavorite(int id)
        {
            Favorite favorite = await db.Favorites.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }

            db.Favorites.Remove(favorite);
            await db.SaveChangesAsync();

            return Ok(favorite);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FavoriteExists(int id)
        {
            return db.Favorites.Count(e => e.FavoriteId == id) > 0;
        }
    }
}