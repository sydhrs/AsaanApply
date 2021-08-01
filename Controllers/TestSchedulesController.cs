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
    public class TestSchedulesController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: TestSchedules
        public async Task<ActionResult> Index()
        {
            var testSchedules = db.TestSchedules.Include(t => t.EntranceTestVenue);
            return View(await testSchedules.ToListAsync());
        }

        // GET: TestSchedules/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestSchedule testSchedule = await db.TestSchedules.FindAsync(id);
            if (testSchedule == null)
            {
                return HttpNotFound();
            }
            return View(testSchedule);
        }

        // GET: TestSchedules/Create
        public ActionResult Create()
        {
            ViewBag.EntranceTestVenueId = new SelectList(db.EntranceTestVenues, "Id", "VenueName");
            return View();
        }

        // POST: TestSchedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TotalSeat,ReservedSeat,ExamDate,StartTime,EndTime,EntranceTestVenueId,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsActive")] TestSchedule testSchedule)
        {
            if (ModelState.IsValid)
            {
                // testSchedule.StartTime = Convert.ToDateTime(DateTime.Now.ToString("yy-MM-dd"));
                // testSchedule.EndTime = Convert.ToDateTime(DateTime.Now.ToString("yy-MM-dd"));
                db.TestSchedules.Add(testSchedule);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EntranceTestVenueId = new SelectList(db.EntranceTestVenues, "Id", "VenueName", testSchedule.EntranceTestVenueId);
            return View(testSchedule);
        }

        // GET: TestSchedules/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestSchedule testSchedule = await db.TestSchedules.FindAsync(id);
            if (testSchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.EntranceTestVenueId = new SelectList(db.EntranceTestVenues, "Id", "VenueName", testSchedule.EntranceTestVenueId);
            return View(testSchedule);
        }

        // POST: TestSchedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TotalSeat,ReservedSeat,ExamDate,StartTime,EndTime,EntranceTestVenueId,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsActive")] TestSchedule testSchedule)
        {
            if (ModelState.IsValid)
            {
                TestSchedule tS = new TestSchedule()
                {
                    StartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy MMMM dd")),

                }; 
                db.Entry(testSchedule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EntranceTestVenueId = new SelectList(db.EntranceTestVenues, "Id", "VenueName", testSchedule.EntranceTestVenueId);
            return View(testSchedule);
        }

        // GET: TestSchedules/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestSchedule testSchedule = await db.TestSchedules.FindAsync(id);
            if (testSchedule == null)
            {
                return HttpNotFound();
            }
            return View(testSchedule);
        }

        // POST: TestSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            TestSchedule testSchedule = await db.TestSchedules.FindAsync(id);
            db.TestSchedules.Remove(testSchedule);
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
