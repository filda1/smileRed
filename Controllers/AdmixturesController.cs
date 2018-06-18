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
using smileRed.Domain;

namespace smileRed.API.Controllers
{
    public class AdmixturesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Admixtures
        public IQueryable<Admixtures> GetAdmixtures()
        {
            return db.Admixtures;
        }

        // GET: api/Admixtures/5
        [ResponseType(typeof(Admixtures))]
        public async Task<IHttpActionResult> GetAdmixtures(int id)
        {
            Admixtures admixtures = await db.Admixtures.FindAsync(id);
            if (admixtures == null)
            {
                return NotFound();
            }

            return Ok(admixtures);
        }

        // PUT: api/Admixtures/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAdmixtures(int id, Admixtures admixtures)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != admixtures.IngredientsId)
            {
                return BadRequest();
            }

            db.Entry(admixtures).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdmixturesExists(id))
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

        // POST: api/Admixtures
        [ResponseType(typeof(Admixtures))]
        public async Task<IHttpActionResult> PostAdmixtures(Admixtures admixtures)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Admixtures.Add(admixtures);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = admixtures.IngredientsId }, admixtures);
        }

        // DELETE: api/Admixtures/5
        [ResponseType(typeof(Admixtures))]
        public async Task<IHttpActionResult> DeleteAdmixtures(int id)
        {
            Admixtures admixtures = await db.Admixtures.FindAsync(id);
            if (admixtures == null)
            {
                return NotFound();
            }

            db.Admixtures.Remove(admixtures);
            await db.SaveChangesAsync();

            return Ok(admixtures);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AdmixturesExists(int id)
        {
            return db.Admixtures.Count(e => e.IngredientsId == id) > 0;
        }
    }
}