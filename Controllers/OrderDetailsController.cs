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
    public class OrderDetailsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/OrderDetails
        public IQueryable<OrderDetails> GetOrderDetails()
        {
            return db.OrderDetails;
        }

        // GET: api/OrderDetails/5
        [ResponseType(typeof(OrderDetails))]
        public async Task<IHttpActionResult> GetOrderDetails(int id)
        {
            OrderDetails orderDetails = await db.OrderDetails.FindAsync(id);
            if (orderDetails == null)
            {
                return NotFound();
            }

            return Ok(orderDetails);
        }

        // PUT: api/OrderDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrderDetails(int id, OrderDetails orderDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderDetails.OrderDetailsID)
            {
                return BadRequest();
            }

            db.Entry(orderDetails).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailsExists(id))
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

        // POST: api/OrderDetails
        [ResponseType(typeof(OrderDetails))]
        public async Task<IHttpActionResult> PostOrderDetails(OrderDetails orderDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OrderDetails.Add(orderDetails);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = orderDetails.OrderDetailsID }, orderDetails);
        }

        // DELETE: api/OrderDetails/5
        [ResponseType(typeof(OrderDetails))]
        public async Task<IHttpActionResult> DeleteOrderDetails(int id)
        {
            OrderDetails orderDetails = await db.OrderDetails.FindAsync(id);
            if (orderDetails == null)
            {
                return NotFound();
            }

            db.OrderDetails.Remove(orderDetails);
            await db.SaveChangesAsync();

            return Ok(orderDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderDetailsExists(int id)
        {
            return db.OrderDetails.Count(e => e.OrderDetailsID == id) > 0;
        }
    }
}