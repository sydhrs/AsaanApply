using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebProject_NetFramework.DbContext;
using WebProject_NetFramework.DbContext.DbModels;

namespace WebProject_NetFramework.Controllers
{
    public class EntranceTestVenuesController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: EntranceTestVenues
        public async Task<ActionResult> Index()
        {
            return View(await db.EntranceTestVenues.ToListAsync());
        }

        // GET: EntranceTestVenues/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntranceTestVenue entranceTestVenue = await db.EntranceTestVenues.FindAsync(id);
            if (entranceTestVenue == null)
            {
                return HttpNotFound();
            }
            return View(entranceTestVenue);
        }

        // GET: EntranceTestVenues/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EntranceTestVenues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,VenueName,VenueAddress,OpeningTime,ClosingTime,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsActive")] EntranceTestVenue entranceTestVenue)
        {
            if (ModelState.IsValid)
            {
                db.EntranceTestVenues.Add(entranceTestVenue);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(entranceTestVenue);
        }

        // GET: EntranceTestVenues/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntranceTestVenue entranceTestVenue = await db.EntranceTestVenues.FindAsync(id);
            if (entranceTestVenue == null)
            {
                return HttpNotFound();
            }
            return View(entranceTestVenue);
        }

        // POST: EntranceTestVenues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,VenueName,VenueAddress,OpeningTime,ClosingTime,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsActive")] EntranceTestVenue entranceTestVenue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entranceTestVenue).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(entranceTestVenue);
        }

        // GET: EntranceTestVenues/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntranceTestVenue entranceTestVenue = await db.EntranceTestVenues.FindAsync(id);
            if (entranceTestVenue == null)
            {
                return HttpNotFound();
            }
            return View(entranceTestVenue);
        }

        // POST: EntranceTestVenues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            EntranceTestVenue entranceTestVenue = await db.EntranceTestVenues.FindAsync(id);
            db.EntranceTestVenues.Remove(entranceTestVenue);
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
