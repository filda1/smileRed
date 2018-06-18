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
    public class OffertsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Offerts
        [ResponseType(typeof(Offert))]
        public async Task<IHttpActionResult> GetOfferts()
        {
            var q = (from o in db.Offerts
                     join p in db.Products on o.ProductId equals p.ProductId
                     select new
                     {
                         o.OffertId,
                         o.ProductId,
                         o.Offer,
                         o.Description,
                         o.Image,
                         o.StartDate,
                         o.EndofDate,
                         o.IsActive,
                         o.Remarks,
                         p.Name,
                         p.Price,
                         p.VAT,
                         p.Stock,                      
                     });

            var offertsProduct = new List<OffertsView>();
            foreach (var t in q)
            {
                offertsProduct.Add(new OffertsView()
                {
                 OffertId = t.OffertId,
                 ProductId = t.ProductId,
                 Offer = t.Offer,
                 Description = t.Description,
                 Image = t.Image,
                 StartDate = t.StartDate,
                 EndofDate = t.EndofDate,
                 IsActive = t.IsActive,
                 Remarks = t.Remarks,
                 Name = t.Name,
                 Price = t.Price,
                 VAT = t.VAT,
                 Stock = t.Stock,
                });
            }
            if (offertsProduct == null)
            {
                return NotFound();
            }

            return Ok(offertsProduct);
        }

        // GET: api/Offerts/5
        [ResponseType(typeof(Offert))]
        public async Task<IHttpActionResult> GetOffert(int id)
        {
            var q = (from o in db.Offerts
                     join p in db.Products on o.ProductId equals p.ProductId
                     where o.OffertId == id
                     select new
                     {
                         o.OffertId,
                         o.ProductId,
                         o.Offer,
                         o.Description,
                         o.Image,
                         o.StartDate,
                         o.EndofDate,
                         o.IsActive,
                         o.Remarks,
                         p.Name,
                         p.Price,
                         p.VAT,
                         p.Stock,
                     });

            var offertsProduct = new List<OffertsView>();
            foreach (var t in q)
            {
                offertsProduct.Add(new OffertsView()
                {
                    OffertId = t.OffertId,
                    ProductId = t.ProductId,
                    Offer = t.Offer,
                    Description = t.Description,
                    Image = t.Image,
                    StartDate = t.StartDate,
                    EndofDate = t.EndofDate,
                    IsActive = t.IsActive,
                    Remarks = t.Remarks,
                    Name = t.Name,
                    Price = t.Price,
                    VAT = t.VAT,
                    Stock = t.Stock,
                });
            }
            if (offertsProduct == null)
            {
                return NotFound();
            }

            return Ok(offertsProduct);
        }

        // PUT: api/Offerts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOffert(int id, Offert offert)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != offert.OffertId)
            {
                return BadRequest();
            }

            db.Entry(offert).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OffertExists(id))
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

        // POST: api/Offerts
        [ResponseType(typeof(Offert))]
        public async Task<IHttpActionResult> PostOffert(Offert offert)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Offerts.Add(offert);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = offert.OffertId }, offert);
        }

        // DELETE: api/Offerts/5
        [ResponseType(typeof(Offert))]
        public async Task<IHttpActionResult> DeleteOffert(int id)
        {
            Offert offert = await db.Offerts.FindAsync(id);
            if (offert == null)
            {
                return NotFound();
            }

            db.Offerts.Remove(offert);
            await db.SaveChangesAsync();

            return Ok(offert);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OffertExists(int id)
        {
            return db.Offerts.Count(e => e.OffertId == id) > 0;
        }
    }
}