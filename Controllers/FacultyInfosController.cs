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
    public class FacultyInfosController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: FacultyInfos
        public async Task<ActionResult> Index()
        {
            var facultyInfos = db.FacultyInfos.Include(f => f.Staff);
            return View(await facultyInfos.ToListAsync());
        }

        // GET: FacultyInfos/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacultyInfo facultyInfo = await db.FacultyInfos.FindAsync(id);
            if (facultyInfo == null)
            {
                return HttpNotFound();
            }
            return View(facultyInfo);
        }

        // GET: FacultyInfos/Create
        public ActionResult Create()
        {
            ViewBag.StaffId = new SelectList(db.Persons, "Id", "FirstName");
            return View();
        }

        // POST: FacultyInfos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,PhoneNo,Position,Email,FacebookName,TwitterName,SkypeName,LinkedInName,PicName,StaffId")] FacultyInfo facultyInfo)
        {
            if (ModelState.IsValid)
            {
                db.FacultyInfos.Add(facultyInfo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.StaffId = new SelectList(db.Persons, "Id", "FirstName", facultyInfo.StaffId);
            return View(facultyInfo);
        }

        // GET: FacultyInfos/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            ViewBag.StaffId = new SelectList(db.Persons, "Id", "FirstName");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacultyInfo facultyInfo = await db.FacultyInfos.FindAsync(id);
            if (facultyInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.StaffId = new SelectList(db.Persons, "Id", "FirstName", facultyInfo.StaffId);
            return View(facultyInfo);
        }

        // POST: FacultyInfos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,PhoneNo,Position,Email,FacebookName,TwitterName,SkypeName,LinkedInName,PicName,StaffId")] FacultyInfo facultyInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facultyInfo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.StaffId = new SelectList(db.Persons, "Id", "FirstName", facultyInfo.StaffId);
            return View(facultyInfo);
        }

        // GET: FacultyInfos/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacultyInfo facultyInfo = await db.FacultyInfos.FindAsync(id);
            if (facultyInfo == null)
            {
                return HttpNotFound();
            }
            return View(facultyInfo);
        }

        // POST: FacultyInfos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            FacultyInfo facultyInfo = await db.FacultyInfos.FindAsync(id);
            db.FacultyInfos.Remove(facultyInfo);
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
