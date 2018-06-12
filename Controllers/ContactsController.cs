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
using smileRed.Backend.Helpers;

namespace smileRed.Backend.Controllers
{
    public class ContactsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Contacts
        public async Task<ActionResult> Index()
        {
            return View(await db.Contacts.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacts contacts = await db.Contacts.FindAsync(id);
            if (contacts == null)
            {
                return HttpNotFound();
            }
            return View(contacts);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ContactsView view)
        {
            string company = Convert.ToString(Request["Company"]);
            var existC = db.Contacts.Where(n => n.Company == company).FirstOrDefault();

            if (existC != null)
            {
                ViewBag.Error = "The Company already exists!";
                return View();
            }

            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Images";

                if (view.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }
                var contacts = ToContacts(view);
                contacts.ImagePath = pic;

                db.Contacts.Add(contacts);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        private Contacts ToContacts(ContactsView view)
        {
            return new Contacts
            {
                ContactsId = view.ContactsId,
                Company = view.Company,
                Description = view.Description,
                Email = view.Email,
                Telephone = view.Telephone,
                Address = view.Address,
                Code = view.Code,
                Door = view.Door,
                Location = view.Location,
                ImagePath = view.ImagePath,
                Active = view.Active,
            };
        }

        // GET: Contacts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacts contacts = await db.Contacts.FindAsync(id);
            if (contacts == null)
            {
                return HttpNotFound();
            }
            var view = ToView(contacts);
            return View(view);
        }

        private ContactsView ToView(Contacts contacts)
        {
            return new ContactsView
            {
                ContactsId = contacts.ContactsId,
                Company = contacts.Company,
                Description = contacts.Description,
                Email = contacts.Email,
                Telephone = contacts.Telephone,
                Address = contacts.Address,
                Code = contacts.Code,
                Door = contacts.Door,
                Location = contacts.Location,
                ImagePath = contacts.ImagePath,
                Active = contacts.Active,
            };
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ContactsView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Images";

                if (view.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }
                var contacts = ToContacts(view);
                contacts.ImagePath = pic;
                db.Entry(contacts).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(view);
        }

        // GET: Contacts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacts contacts = await db.Contacts.FindAsync(id);
            if (contacts == null)
            {
                return HttpNotFound();
            }
            return View(contacts);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Contacts contacts = await db.Contacts.FindAsync(id);
            db.Contacts.Remove(contacts);
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
