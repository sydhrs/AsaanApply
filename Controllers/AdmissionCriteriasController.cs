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
    public class AdmissionCriteriasController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: AdmissionCriterias
        public async Task<ActionResult> Index()
        {
            return View(await db.AdmissionCriteria.ToListAsync());
        }

        // GET: AdmissionCriterias/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdmissionCriteria admissionCriteria = await db.AdmissionCriteria.FindAsync(id);
            if (admissionCriteria == null)
            {
                return HttpNotFound();
            }

            var dpt = await db.DegreePursuingTypes.FindAsync(admissionCriteria.DegreePursuingTypeId);
            ViewBag.DPName = dpt.Name ?? "Not Defined";

            return View(admissionCriteria);
        }

    
        // GET: AdmissionCriterias/Create
        public ActionResult Create()
        {
            ViewBag.DegreePursuingTypeId = new SelectList(db.DegreePursuingTypes, "Id", "Name");

            return View();
        }

        // POST: AdmissionCriterias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Criteria,DegreePursuingTypeId")] AdmissionCriteria admissionCriteria)
        {
            if (ModelState.IsValid)
            {
                db.AdmissionCriteria.Add(admissionCriteria);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(admissionCriteria);
        }

        //GET: AdmissionCriterias/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            ViewBag.DegreePursuingTypeId = new SelectList(db.DegreePursuingTypes, "Id", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdmissionCriteria admissionCriteria = await db.AdmissionCriteria.FindAsync(id);
            if (admissionCriteria == null)
            {
                return HttpNotFound();
            }
            return View(admissionCriteria);
        }

        // POST: AdmissionCriterias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Criteria,DegreePursuingTypeId")] AdmissionCriteria admissionCriteria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admissionCriteria).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(admissionCriteria);
        }

        // GET: AdmissionCriterias/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdmissionCriteria admissionCriteria = await db.AdmissionCriteria.FindAsync(id);
            if (admissionCriteria == null)
            {
                return HttpNotFound();
            }

            var dpt = await db.DegreePursuingTypes.FindAsync(admissionCriteria.DegreePursuingTypeId);
            ViewBag.DPName = dpt.Name ?? "Not Defined";
            
            return View(admissionCriteria);
        }

        // POST: AdmissionCriterias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            AdmissionCriteria admissionCriteria = await db.AdmissionCriteria.FindAsync(id);
            db.AdmissionCriteria.Remove(admissionCriteria);
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
