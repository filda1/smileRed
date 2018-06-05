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
    public class ReservationController : Controller
    {
        LocalDataContext db = new LocalDataContext();

        // GET: Reservation
        public ActionResult ViewReservation()
        {
            var today = DateTime.Today;
            var q = (from r in db.Reservations
                     join u in db.Users on r.Email equals u.Email
                     where  r.ReservationDate >= today
                     select new
                     {
                         r.ReservationId,
                         u.FirstName,
                         u.LastName,
                         r.Email,
                         r.Active,
                         r.ReservationDate,
                         r.Remarks
                     });

            var reservationUsers = new List<ReservationsUsers>();
            foreach (var t in q)
            {
                reservationUsers.Add(new ReservationsUsers()
                {
                    ReservationId = t.ReservationId,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    Email = t.Email,
                    Active = t.Active,
                    ReservationDate = t.ReservationDate,
                    Remarks = t.Remarks
                });
            }
            ViewBag.Count = reservationUsers.Count();
            return View(reservationUsers);
        }

        public ActionResult DeleteReservations(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation mov = db.Reservations.Find(id);
            if (mov == null)
            {
                return HttpNotFound();
            }
            return View(mov);
        }

        [HttpPost, ActionName("DeleteReservations")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteReservationsConfirmed(int id)
        {
            Reservation mov = db.Reservations.Find(id);
            db.Reservations.Remove(mov);
            db.SaveChanges();
            return RedirectToAction("ViewReservation");
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