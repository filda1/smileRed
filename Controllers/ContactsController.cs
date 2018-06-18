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
    public class ContactsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Contacts
        public IQueryable<Contacts> GetContacts()
        {
            return db.Contacts;
        }

        // GET: api/Contacts/5
        [ResponseType(typeof(Contacts))]
        public async Task<IHttpActionResult> GetContacts(int id)
        {
            Contacts contacts = await db.Contacts.FindAsync(id);
            if (contacts == null)
            {
                return NotFound();
            }

            return Ok(contacts);
        }

        // PUT: api/Contacts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutContacts(int id, Contacts contacts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contacts.ContactsId)
            {
                return BadRequest();
            }

            db.Entry(contacts).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactsExists(id))
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

        // POST: api/Contacts
        [ResponseType(typeof(Contacts))]
        public async Task<IHttpActionResult> PostContacts(Contacts contacts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Contacts.Add(contacts);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = contacts.ContactsId }, contacts);
        }

        // DELETE: api/Contacts/5
        [ResponseType(typeof(Contacts))]
        public async Task<IHttpActionResult> DeleteContacts(int id)
        {
            Contacts contacts = await db.Contacts.FindAsync(id);
            if (contacts == null)
            {
                return NotFound();
            }

            db.Contacts.Remove(contacts);
            await db.SaveChangesAsync();

            return Ok(contacts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContactsExists(int id)
        {
            return db.Contacts.Count(e => e.ContactsId == id) > 0;
        }
    }
}