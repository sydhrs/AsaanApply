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
    public class DegreePursuingTypesController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: DegreePursuingTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.DegreePursuingTypes.ToListAsync());
        }

        // GET: DegreePursuingTypes/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DegreePursuingType degreePursuingType = await db.DegreePursuingTypes.FindAsync(id);
            if (degreePursuingType == null)
            {
                return HttpNotFound();
            }
            return View(degreePursuingType);
        }

        // GET: DegreePursuingTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DegreePursuingTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Value,Description")] DegreePursuingType degreePursuingType)
        {
            if (ModelState.IsValid)
            {
                db.DegreePursuingTypes.Add(degreePursuingType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(degreePursuingType);
        }

        // GET: DegreePursuingTypes/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DegreePursuingType degreePursuingType = await db.DegreePursuingTypes.FindAsync(id);
            if (degreePursuingType == null)
            {
                return HttpNotFound();
            }
            return View(degreePursuingType);
        }

        // POST: DegreePursuingTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Value,Description")] DegreePursuingType degreePursuingType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(degreePursuingType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(degreePursuingType);
        }

        // GET: DegreePursuingTypes/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DegreePursuingType degreePursuingType = await db.DegreePursuingTypes.FindAsync(id);
            if (degreePursuingType == null)
            {
                return HttpNotFound();
            }
            return View(degreePursuingType);
        }

        // POST: DegreePursuingTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            DegreePursuingType degreePursuingType = await db.DegreePursuingTypes.FindAsync(id);
            db.DegreePursuingTypes.Remove(degreePursuingType);
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
