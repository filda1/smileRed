using Rotativa;
using smileRed.Backend.Models;
using smileRed.Domain;
using System;
using System.Collections.Generic;
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
                     join u in db.Users on o.Email equals u.Email
                     join p in db.Products on od.ProductID equals p.ProductId
                     where o.ActiveOrders == true && od.ActiveOrderDetails == true
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
                         u.Address
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
                    Address = t.Address
                });
            }
            ViewBag.TIMEZONE = TimePT;
            ViewBag.Count = productorders.Count();
            return View(productorders);
        }

        // GET: Products/Details/5
        public  ActionResult DetailsAllOrders(int? id)
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
                         u.Telephone
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
                    Telephone = t.Telephone
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

        public ActionResult PrintPDF()
        {
            return new ActionAsPdf("DetailsAllOrders")
            //new { iddd = id }

            { FileName = "DetailsAllOrders.pdf" };
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