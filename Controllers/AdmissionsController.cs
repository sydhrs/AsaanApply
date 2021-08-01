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
    public class AdmissionsController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: Admissions
        public async Task<ActionResult> Index()
        {
            var admissions = db.Admissions.Include(a => a.DegreePursuingType).Include(a => a.EntityStatusType)
                .Include(a => a.Student).Include(a => a.StudentType);
            return View(await admissions.ToListAsync());
        }

        // GET: Admissions/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Admission admission = await db.Admissions.FindAsync(id);
            if (admission == null)
            {
                return HttpNotFound();
            }

            return View(admission);
        }

        // GET: Admissions/Create
        public ActionResult Create()
        {
            ViewBag.DegreePursuingTypeId = new SelectList(db.DegreePursuingTypes, "Id", "Name");
            ViewBag.EntityStatusTypeId = new SelectList(db.EntityStatusTypes, "Id", "Name");
            ViewBag.StudentId = new SelectList(db.Persons, "Id", "FirstName");
            ViewBag.StudentTypeId = new SelectList(db.StudentTypes, "Id", "Name");
            ViewBag.TestGradeId = new SelectList(db.TestGrades, "Id", "Score");
            return View();
        }

        // POST: Admissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include =
                "Id,StudentTypeId,DegreePursuingTypeId,StudentId,EntityStatusTypeId,StatusReason,TestGradeId,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsActive")]
            Admission admission)
        {
            if (ModelState.IsValid)
            {
                db.Admissions.Add(admission);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DegreePursuingTypeId =
                new SelectList(db.DegreePursuingTypes, "Id", "Name", admission.DegreePursuingTypeId);
            ViewBag.EntityStatusTypeId =
                new SelectList(db.EntityStatusTypes, "Id", "Name", admission.EntityStatusTypeId);
            ViewBag.StudentId = new SelectList(db.Persons, "Id", "FirstName", admission.StudentId);
            ViewBag.StudentTypeId = new SelectList(db.StudentTypes, "Id", "Name", admission.StudentTypeId);
            ViewBag.TestGradeId = new SelectList(db.TestGrades, "Id", "Score", admission.TestGradeId);
            return View(admission);
        }

        // GET: Admissions/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Admission admission = await db.Admissions.FindAsync(id);
            if (admission == null)
            {
                return HttpNotFound();
            }

            ViewBag.DegreePursuingTypeId =
                new SelectList(db.DegreePursuingTypes, "Id", "Name", admission.DegreePursuingTypeId);
            ViewBag.EntityStatusTypeId =
                new SelectList(db.EntityStatusTypes, "Id", "Name", admission.EntityStatusTypeId);
            ViewBag.StudentId = new SelectList(db.Persons, "Id", "FirstName", admission.StudentId);
            ViewBag.StudentTypeId = new SelectList(db.StudentTypes, "Id", "Name", admission.StudentTypeId);
            ViewBag.TestGradeId = new SelectList(db.TestGrades, "Id", "Score", admission.TestGradeId);
            return View(admission);
        }

        // POST: Admissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include =
                "Id,StudentTypeId,DegreePursuingTypeId,StudentId,EntityStatusTypeId,StatusReason,TestGradeId,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsActive")]
            Admission admission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admission).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DegreePursuingTypeId =
                new SelectList(db.DegreePursuingTypes, "Id", "Name", admission.DegreePursuingTypeId);
            ViewBag.EntityStatusTypeId =
                new SelectList(db.EntityStatusTypes, "Id", "Name", admission.EntityStatusTypeId);
            ViewBag.StudentId = new SelectList(db.Persons, "Id", "FirstName", admission.StudentId);
            ViewBag.StudentTypeId = new SelectList(db.StudentTypes, "Id", "Name", admission.StudentTypeId);
            ViewBag.TestGradeId = new SelectList(db.TestGrades, "Id", "Score", admission.TestGradeId);
            return View(admission);
        }

        // GET: Admissions/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Admission admission = await db.Admissions.FindAsync(id);
            if (admission == null)
            {
                return HttpNotFound();
            }

            return View(admission);
        }

        // POST: Admissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Admission admission = await db.Admissions.FindAsync(id);
            db.Admissions.Remove(admission);
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