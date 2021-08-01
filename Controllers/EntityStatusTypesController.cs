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
    public class EntityStatusTypesController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: EntityStatusTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.EntityStatusTypes.ToListAsync());
        }

        // GET: EntityStatusTypes/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntityStatusType entityStatusType = await db.EntityStatusTypes.FindAsync(id);
            if (entityStatusType == null)
            {
                return HttpNotFound();
            }
            return View(entityStatusType);
        }

        // GET: EntityStatusTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EntityStatusTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Value,Description")] EntityStatusType entityStatusType)
        {
            if (ModelState.IsValid)
            {
                db.EntityStatusTypes.Add(entityStatusType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(entityStatusType);
        }

        // GET: EntityStatusTypes/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntityStatusType entityStatusType = await db.EntityStatusTypes.FindAsync(id);
            if (entityStatusType == null)
            {
                return HttpNotFound();
            }
            return View(entityStatusType);
        }

        // POST: EntityStatusTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Value,Description")] EntityStatusType entityStatusType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entityStatusType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(entityStatusType);
        }

        // GET: EntityStatusTypes/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntityStatusType entityStatusType = await db.EntityStatusTypes.FindAsync(id);
            if (entityStatusType == null)
            {
                return HttpNotFound();
            }
            return View(entityStatusType);
        }

        // POST: EntityStatusTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            EntityStatusType entityStatusType = await db.EntityStatusTypes.FindAsync(id);
            db.EntityStatusTypes.Remove(entityStatusType);
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
