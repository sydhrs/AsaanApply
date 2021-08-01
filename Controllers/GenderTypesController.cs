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
    public class GenderTypesController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: GenderTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.GenderTypes.ToListAsync());
        }

        // GET: GenderTypes/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenderType genderType = await db.GenderTypes.FindAsync(id);
            if (genderType == null)
            {
                return HttpNotFound();
            }
            return View(genderType);
        }

        // GET: GenderTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GenderTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Value,Description")] GenderType genderType)
        {
            if (ModelState.IsValid)
            {
                db.GenderTypes.Add(genderType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(genderType);
        }

        // GET: GenderTypes/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenderType genderType = await db.GenderTypes.FindAsync(id);
            if (genderType == null)
            {
                return HttpNotFound();
            }
            return View(genderType);
        }

        // POST: GenderTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Value,Description")] GenderType genderType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genderType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(genderType);
        }

        // GET: GenderTypes/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenderType genderType = await db.GenderTypes.FindAsync(id);
            if (genderType == null)
            {
                return HttpNotFound();
            }
            return View(genderType);
        }

        // POST: GenderTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            GenderType genderType = await db.GenderTypes.FindAsync(id);
            db.GenderTypes.Remove(genderType);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<JsonResult> GetList()
        {
            var list = await db.GenderTypes.Where(f => f.IsActive).ToListAsync();
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
