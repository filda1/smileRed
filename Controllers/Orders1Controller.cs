using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using smileRed.Backend.Models;
using smileRed.Domain;

namespace smileRed.Backend.Controllers
{
    public class Orders1Controller : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Orders1
        public async Task<ActionResult> Index()
        {
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.OrderStatuses);
            return View(await orders.ToListAsync());
        }

        // GET: Orders1/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders1/Create
        public ActionResult Create()
        {
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "OrderStatusID", "OrderStatusName");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FullName");
            return View();
        }

        // POST: Orders1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( Order order)
        {
            var orderStatus = order.OrderStatusID;
            DateTime thisTime = DateTime.Now;
            TimeZoneInfo InfoZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            DateTime TimePT = TimeZoneInfo.ConvertTime(thisTime, TimeZoneInfo.Local, InfoZone);

            DateTime dataOrder = Convert.ToDateTime(Request["DateOrder"]);
            if (dataOrder < TimePT)
            {
                ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "OrderStatusID", "OrderStatusName", order.OrderStatusID);
                ViewBag.UserId = new SelectList(db.Users, "UserId", "FullName", order.UserId);        
                ViewBag.Error = "The order date can not be less than today!";

                return View();
            }

            if (orderStatus == 0)
            {
                ViewBag.Error = "Select Order Status!";
                return View();
            }
             

            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "OrderStatusID", "OrderStatusName", order.OrderStatusID);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FullName", order.UserId);
            return View(order);
        }

        // GET: Orders1/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "OrderStatusID", "OrderStatusName", order.OrderStatusID);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FullName", order.UserId);
            return View(order);
        }

        // POST: Orders1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( Order order)
        {     
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "OrderStatusID", "OrderStatusName", order.OrderStatusID);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FullName", order.UserId);
            return View(order);
        }

        // GET: Orders1/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
