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
    public class SystemSettingsController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: SystemSettings
        public async Task<ActionResult> Index()
        {
            return View(await db.SystemSettings.ToListAsync());
        }

        // GET: SystemSettings/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemSetting systemSetting = await db.SystemSettings.FindAsync(id);
            if (systemSetting == null)
            {
                return HttpNotFound();
            }
            return View(systemSetting);
        }

        // GET: SystemSettings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SystemSettings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,AdmissionsOpen")] SystemSetting systemSetting)
        {
            if (ModelState.IsValid)
            {
                db.SystemSettings.Add(systemSetting);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(systemSetting);
        }

        // GET: SystemSettings/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemSetting systemSetting = await db.SystemSettings.FindAsync(id);
            if (systemSetting == null)
            {
                return HttpNotFound();
            }
            return View(systemSetting);
        }

        // POST: SystemSettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AdmissionsOpen")] SystemSetting systemSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(systemSetting).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(systemSetting);
        }

        // GET: SystemSettings/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemSetting systemSetting = await db.SystemSettings.FindAsync(id);
            if (systemSetting == null)
            {
                return HttpNotFound();
            }
            return View(systemSetting);
        }

        // POST: SystemSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            SystemSetting systemSetting = await db.SystemSettings.FindAsync(id);
            db.SystemSettings.Remove(systemSetting);
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
