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
    public class ReservationsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Reservations
        public IQueryable<Reservation> GetReservations()
        {
            return db.Reservations;
        }

        // GET: api/Reservations/id?id=Gordon@defens.com
        [ResponseType(typeof(Reservation))]
        public async Task<IHttpActionResult> GetReservation(string id)
        {
            var q = (from r in db.Reservations
                     join u in db.Users on r.Email equals u.Email
                     where  r.Email == id
                     select new
                     {
                         r.ReservationId,
                         u.FirstName,
                         u.LastName,
                         r.Email,
                         r.Active,
                         r.ReservationDate,
                         r.Remarks,
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
                    Remarks = t.Remarks,
                });
            }

            return Ok(reservationUsers);
        }

        // PUT: api/Reservations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutReservation(int id, Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reservation.ReservationId)
            {
                return BadRequest();
            }

            db.Entry(reservation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
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

        // POST: api/Reservations
        [ResponseType(typeof(Reservation))]
        public async Task<IHttpActionResult> PostReservation(Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reservations.Add(reservation);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = reservation.ReservationId }, reservation);
        }

        // DELETE: api/Reservations/5
        [ResponseType(typeof(Reservation))]
        public async Task<IHttpActionResult> DeleteReservation(int id)
        {
            Reservation reservation = await db.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            db.Reservations.Remove(reservation);
            await db.SaveChangesAsync();

            return Ok(reservation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReservationExists(int id)
        {
            return db.Reservations.Count(e => e.ReservationId == id) > 0;
        }
    }
}