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
    public class TypeofUsersController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/TypeofUsers
        public IQueryable<TypeofUser> GetTypeofUsers()
        {
            return db.TypeofUsers;
        }

        // GET: api/TypeofUsers/5
        [ResponseType(typeof(TypeofUser))]
        public async Task<IHttpActionResult> GetTypeofUser(int id)
        {
            TypeofUser typeofUser = await db.TypeofUsers.FindAsync(id);
            if (typeofUser == null)
            {
                return NotFound();
            }

            return Ok(typeofUser);
        }

        // PUT: api/TypeofUsers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTypeofUser(int id, TypeofUser typeofUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != typeofUser.TypeofUserId)
            {
                return BadRequest();
            }

            db.Entry(typeofUser).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeofUserExists(id))
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

        // POST: api/TypeofUsers
        [ResponseType(typeof(TypeofUser))]
        public async Task<IHttpActionResult> PostTypeofUser(TypeofUser typeofUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TypeofUsers.Add(typeofUser);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = typeofUser.TypeofUserId }, typeofUser);
        }

        // DELETE: api/TypeofUsers/5
        [ResponseType(typeof(TypeofUser))]
        public async Task<IHttpActionResult> DeleteTypeofUser(int id)
        {
            TypeofUser typeofUser = await db.TypeofUsers.FindAsync(id);
            if (typeofUser == null)
            {
                return NotFound();
            }

            db.TypeofUsers.Remove(typeofUser);
            await db.SaveChangesAsync();

            return Ok(typeofUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TypeofUserExists(int id)
        {
            return db.TypeofUsers.Count(e => e.TypeofUserId == id) > 0;
        }
    }
}