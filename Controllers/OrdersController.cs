using Rotativa;
using smileRed.Backend.Models;
using smileRed.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
            DateTime thisTime = DateTime.Now;
            TimeZoneInfo InfoZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            DateTime TimePT = TimeZoneInfo.ConvertTime(thisTime, TimeZoneInfo.Local, InfoZone);

            var q = (from o in db.Orders
                     join od in db.OrderDetails on o.OrderID equals od.OrderID
                     join os in db.OrderStatus on o.OrderStatusID equals os.OrderStatusID
                     join u in db.Users on o.Email equals u.Email
                     join p in db.Products on od.ProductID equals p.ProductId
                     where o.ActiveOrders == true
                     && od.ActiveOrderDetails == true
                     && o.Delete == false
                     && o.DateOrder >= TimePT
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
            var countBD = productorders.Count();
            ViewBag.TIMEZONE = TimePT;
            ViewBag.Count = countBD;

            int id = 1;
            var n = db.Data.Find(id);
            int numVirtual = n.VirtualCounter;
            int _newCounter = countBD - numVirtual;
            ViewBag.TEST = _newCounter;

            if (countBD > numVirtual)
            {
                int newCounter = countBD - numVirtual;
                ViewBag.TEST = newCounter;
                TempData["msg"] = "<script> var snd = new Audio('/Sound/blip.wav');snd.play(); " +
                "alert('New Orders');</script>";
                var n2 = db.Data.Find(id);
                n2.VirtualCounter = countBD;
                db.SaveChanges();
            }

            if (countBD < numVirtual)
            {
                int newCounter = countBD;
                ViewBag.TEST = newCounter;
                var n2 = db.Data.Find(id);
                n2.VirtualCounter = countBD;
                db.SaveChanges();
            }

            return View(productorders);
        }

        // GET: Products/Details/5
        public ActionResult DetailsAllOrders(int? id)
        {
            DateTime thisTime = DateTime.Now;
            TimeZoneInfo InfoZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            DateTime TimePT = TimeZoneInfo.ConvertTime(thisTime, TimeZoneInfo.Local, InfoZone);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Order product = await db.Orders.FindAsync(id);
            var q = (from o in db.Orders
                     join od in db.OrderDetails on o.OrderID equals od.OrderID
                     join os in db.OrderStatus on o.OrderStatusID equals os.OrderStatusID
                     join u in db.Users on o.Email equals u.Email
                     join p in db.Products on od.ProductID equals p.ProductId
                     where o.ActiveOrders == true && od.ActiveOrderDetails == true
                     && o.OrderID == id && o.DateOrder >= TimePT
                     select new
                     {
                         o.OrderID,
                         od.OrderDetailsID,
                         p.Name,
                         p.Description,
                         p.Image,
                         p.Price,
                         od.Quantity,
                         o.DateOrder,
                         p.Remarks,
                         u.FirstName,
                         u.LastName,
                         p.VAT,
                         u.Address,
                         u.Location,
                         u.Code,
                         u.Door,
                         u.Telephone,
                         os.OrderStatusName,
                         os.OrderStatusID,
                         o.Email,
                         o.Delete,
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
                    Location = t.Location,
                    Code = t.Code,
                    Door = t.Door,
                    Telephone = t.Telephone,
                    OrderStatusName = t.OrderStatusName,
                    Delete = t.Delete,
                   
                });
            }
            string fullname = productorders.Where(p => p.OrderID == id).FirstOrDefault().FullName;
            string address = productorders.Where(p => p.OrderID == id).FirstOrDefault().Address;
            string location = productorders.Where(p => p.OrderID == id).FirstOrDefault().Location;
            int code = productorders.Where(p => p.OrderID == id).FirstOrDefault().Code;
            int door = productorders.Where(p => p.OrderID == id).FirstOrDefault().Door;
            string telephone = productorders.Where(p => p.OrderID == id).FirstOrDefault().Telephone;

            ViewBag.Fullname = fullname;
            ViewBag.Address = address;
            ViewBag.Location = location;
            ViewBag.Code = code;
            ViewBag.Door = door;
            ViewBag.Telephone = telephone;

            decimal SUM_PriceQuantity = productorders.Where(p => p.OrderID == id).Sum(p => p.PriceQuantity);
            decimal SUM_PriceVATQuantity = productorders.Where(p => p.OrderID == id).Sum(p => p.PriceVATQuantity);
            decimal _VAT = productorders.Where(p => p.OrderID == id).FirstOrDefault().VAT;
            //decimal _VATamount =  (_VAT / 100);

            ViewBag.SUM_PriceQuantity = SUM_PriceQuantity;
            ViewBag.VAT = _VAT;
            ViewBag.SUM_PriceVATQuantity = SUM_PriceVATQuantity;

            if (productorders == null)
            {
                return HttpNotFound();
            }
            return View(productorders);
        }

        public ActionResult DeleteAllOrders(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            order.Delete = true;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            if (order == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("ViewOrders");
        }

        private ProductsOrders ToView(Order order)
        {
            return new ProductsOrders
            {
                OrderID = order.OrderID,
                Delete = order.Delete,
            };
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
