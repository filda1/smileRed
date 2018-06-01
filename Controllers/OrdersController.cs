using smileRed.Backend.Models;
using smileRed.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace smileRed.Backend.Controllers
{
    public class OrdersController : Controller
    {
        LocalDataContext db = new LocalDataContext();
        // GET: Orders
        public ActionResult ViewOrders()
        {
            var q = (from o in db.Orders
                     join od in db.OrderDetails on o.OrderID equals od.OrderID
                     join u in db.Users on o.Email equals u.Email
                     join p in db.Products on od.ProductID equals p.ProductId
                     where o.ActiveOrders == true && od.ActiveOrderDetails == true
                     select new
                     {
                         o.OrderID,
                         od.OrderDetailsID,
                         p.Name,
                         p.Description,
                         p.Image,
                         od.Price,
                         od.Quantity,
                         od.Value,
                         o
                         .DateOrder,
                         p.Remarks,
                         u.FirstName,
                         u.LastName
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
                    Value = t.Value,
                    DateOrder = t.DateOrder,

                });
            }
            ViewBag.Count = productorders.Count();
            return View(productorders);
        }

        public ActionResult DeleteAllOrders(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails mov = db.OrderDetails.Find(id);
            if (mov == null)
            {
                return HttpNotFound();
            }
            return View(mov);
        }

        [HttpPost, ActionName("DeleteAllOrders")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAllOrdersConfirmed(int id)
        {
            OrderDetails mov = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(mov);
            db.SaveChanges();
            return RedirectToAction("ViewOrders");
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