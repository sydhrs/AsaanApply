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
    public class AcademicDegreeTypesController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: AcademicDegreeTypes
        public async Task<ActionResult> Index()
        {
            var academicDegreeTypes = db.AcademicDegreeTypes.Include(a => a.DegreePursuingType);
            return View(await academicDegreeTypes.ToListAsync());
        }

        // GET: AcademicDegreeTypes/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcademicDegreeType academicDegreeType = await db.AcademicDegreeTypes.FindAsync(id);
            if (academicDegreeType == null)
            {
                return HttpNotFound();
            }
            return View(academicDegreeType);
        }

        // GET: AcademicDegreeTypes/Create
        public ActionResult Create()
        {
            ViewBag.PrerequisiteDegreeTypeId = new SelectList(db.DegreePursuingTypes, "Id", "Name");
            return View();
        }

        // POST: AcademicDegreeTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PrerequisiteDegreeTypeId,Name,Value,Description")] AcademicDegreeType academicDegreeType)
        {
            if (ModelState.IsValid)
            {
                db.AcademicDegreeTypes.Add(academicDegreeType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.PrerequisiteDegreeTypeId = new SelectList(db.DegreePursuingTypes, "Id", "Name", academicDegreeType.PrerequisiteDegreeTypeId);
            return View(academicDegreeType);
        }

        // GET: AcademicDegreeTypes/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcademicDegreeType academicDegreeType = await db.AcademicDegreeTypes.FindAsync(id);
            if (academicDegreeType == null)
            {
                return HttpNotFound();
            }
            ViewBag.PrerequisiteDegreeTypeId = new SelectList(db.DegreePursuingTypes, "Id", "Name", academicDegreeType.PrerequisiteDegreeTypeId);
            return View(academicDegreeType);
        }

        // POST: AcademicDegreeTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PrerequisiteDegreeTypeId,Name,Value,Description")] AcademicDegreeType academicDegreeType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(academicDegreeType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PrerequisiteDegreeTypeId = new SelectList(db.DegreePursuingTypes, "Id", "Name", academicDegreeType.PrerequisiteDegreeTypeId);
            return View(academicDegreeType);
        }

        // GET: AcademicDegreeTypes/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcademicDegreeType academicDegreeType = await db.AcademicDegreeTypes.FindAsync(id);
            if (academicDegreeType == null)
            {
                return HttpNotFound();
            }
            return View(academicDegreeType);
        }

        // POST: AcademicDegreeTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            AcademicDegreeType academicDegreeType = await db.AcademicDegreeTypes.FindAsync(id);
            db.AcademicDegreeTypes.Remove(academicDegreeType);
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
