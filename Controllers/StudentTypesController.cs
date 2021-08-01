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
    public class StudentTypesController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: StudentTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.StudentTypes.ToListAsync());
        }

        // GET: StudentTypes/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentType studentType = await db.StudentTypes.FindAsync(id);
            if (studentType == null)
            {
                return HttpNotFound();
            }
            return View(studentType);
        }

        // GET: StudentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Value,Description")] StudentType studentType)
        {
            if (ModelState.IsValid)
            {
                db.StudentTypes.Add(studentType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(studentType);
        }

        // GET: StudentTypes/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentType studentType = await db.StudentTypes.FindAsync(id);
            if (studentType == null)
            {
                return HttpNotFound();
            }
            return View(studentType);
        }

        // POST: StudentTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Value,Description")] StudentType studentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(studentType);
        }

        // GET: StudentTypes/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentType studentType = await db.StudentTypes.FindAsync(id);
            if (studentType == null)
            {
                return HttpNotFound();
            }
            return View(studentType);
        }

        // POST: StudentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            StudentType studentType = await db.StudentTypes.FindAsync(id);
            db.StudentTypes.Remove(studentType);
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
