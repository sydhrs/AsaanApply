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
    public class MaritalStatusTypesController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: MaritalStatusTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.MaritalStatusTypes.ToListAsync());
        }

        // GET: MaritalStatusTypes/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaritalStatusType maritalStatusType = await db.MaritalStatusTypes.FindAsync(id);
            if (maritalStatusType == null)
            {
                return HttpNotFound();
            }
            return View(maritalStatusType);
        }

        // GET: MaritalStatusTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MaritalStatusTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Value,Description")] MaritalStatusType maritalStatusType)
        {
            if (ModelState.IsValid)
            {
                db.MaritalStatusTypes.Add(maritalStatusType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(maritalStatusType);
        }

        // GET: MaritalStatusTypes/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaritalStatusType maritalStatusType = await db.MaritalStatusTypes.FindAsync(id);
            if (maritalStatusType == null)
            {
                return HttpNotFound();
            }
            return View(maritalStatusType);
        }

        // POST: MaritalStatusTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Value,Description")] MaritalStatusType maritalStatusType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(maritalStatusType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(maritalStatusType);
        }

        // GET: MaritalStatusTypes/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaritalStatusType maritalStatusType = await db.MaritalStatusTypes.FindAsync(id);
            if (maritalStatusType == null)
            {
                return HttpNotFound();
            }
            return View(maritalStatusType);
        }

        // POST: MaritalStatusTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            MaritalStatusType maritalStatusType = await db.MaritalStatusTypes.FindAsync(id);
            db.MaritalStatusTypes.Remove(maritalStatusType);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<JsonResult> GetList()
        {
            var list = await db.MaritalStatusTypes.Where(f => f.IsActive).ToListAsync();
            return Json(list, JsonRequestBehavior.AllowGet);
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
