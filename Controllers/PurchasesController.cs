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
    public class PurchasesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Purchases
        public IQueryable<Order> GetOrders()
        {
            return db.Orders;
        }

        // GET: api/Purchases/id?id=Gordon@defens.com
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrder(string id)
        {
            var q = (from o in db.Orders
                     join od in db.OrderDetails on o.OrderID equals od.OrderID
                     join os in db.OrderStatus on o.OrderStatusID equals os.OrderStatusID
                     join u in db.Users on o.Email equals u.Email
                     join p in db.Products on od.ProductID equals p.ProductId
                     where o.ActiveOrders == false
                     && od.ActiveOrderDetails == false
                     && o.VisibleOrders == false
                     && od.VisibleOrderDetails == false
                     && o.Email == id
                     select new
                     {
                         o.OrderID,
                         od.OrderDetailsID,
                         p.Name,
                         p.Description,
                         p.Image,
                         p.Price,
                         od.Quantity,
                         o
                         .DateOrder,
                         p.Remarks,
                         u.FirstName,
                         u.LastName,
                         p.VAT,
                         u.Address,
                         os.OrderStatusName,
                     });

            var productorders = new List<ProductsOrders>();
            foreach (var t in q)
            {
                productorders.Add(new ProductsOrders()
                {
                    OrderID = t.OrderID,
                    OrderDetailsID = t.OrderDetailsID,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    Name = t.Name,
                    Description = t.Description,
                    Image = t.Image,
                    Price = t.Price,
                    Quantity = t.Quantity,
                    DateOrder = t.DateOrder,
                    VAT = t.VAT,
                    Address = t.Address,
                    OrderStatusName = t.OrderStatusName,
                });
            }
            //Order order = await db.Orders.FindAsync(id);
            if (productorders == null)
            {
                return NotFound();
            }

            return Ok(productorders);
        }

        // PUT: api/Purchases/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.OrderID)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Purchases
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(order);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = order.OrderID }, order);
        }

        // DELETE: api/Purchases/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> DeleteOrder(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            await db.SaveChangesAsync();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.OrderID == id) > 0;
        }
    }
}