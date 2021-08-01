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
    public class RoadMapsController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: RoadMaps
        public async Task<ActionResult> Index()
        {
            return View(await db.RoadMaps.ToListAsync());
        }

        // GET: RoadMaps/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoadMap roadMap = await db.RoadMaps.FindAsync(id);
            if (roadMap == null)
            {
                return HttpNotFound();
            }

            var dpt = await db.DegreePursuingTypes.FindAsync(roadMap.DegreePursuingTypeId);
            ViewBag.DPName = dpt.Name ?? "Not Defined";

            return View(roadMap);
        }

        // GET: RoadMaps/Create
        public ActionResult Create()
        {
            ViewBag.DegreePursuingTypeId = new SelectList(db.DegreePursuingTypes, "Id", "Name");
            return View();
        }

        // POST: RoadMaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Details,DegreePursuingTypeId")] RoadMap roadMap)
        {
            if (ModelState.IsValid)
            {
                db.RoadMaps.Add(roadMap);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(roadMap);
        }

        // GET: RoadMaps/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            ViewBag.DegreePursuingTypeId = new SelectList(db.DegreePursuingTypes, "Id", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoadMap roadMap = await db.RoadMaps.FindAsync(id);
            if (roadMap == null)
            {
                return HttpNotFound();
            }
            return View(roadMap);
        }

        // POST: RoadMaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Details,DegreePursuingTypeId")] RoadMap roadMap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roadMap).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(roadMap);
        }
        // GET: RoadMaps/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoadMap roadMap = await db.RoadMaps.FindAsync(id);
            if (roadMap == null)
            {
                return HttpNotFound();
            }

            var dpt = await db.DegreePursuingTypes.FindAsync(roadMap.DegreePursuingTypeId);
            ViewBag.DPName = dpt.Name ?? "Not Defined";

            return View(roadMap);
        }
        // POST: RoadMaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            RoadMap roadMap = await db.RoadMaps.FindAsync(id);
            db.RoadMaps.Remove(roadMap);
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
