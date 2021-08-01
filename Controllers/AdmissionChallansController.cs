using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebProject_NetFramework.DbContext;
using WebProject_NetFramework.DbContext.DbModels;
using WebProject_NetFramework.Models;

namespace WebProject_NetFramework.Controllers
{
    public class AdmissionChallansController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: AdmissionChallans
        public async Task<ActionResult> Index()
        {
            var admissionChallans = db.AdmissionChallans.Include(a => a.Admission).Include(a => a.EntityStatusType);
            return View(await admissionChallans.ToListAsync());
        }

        // GET: AdmissionChallans/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AdmissionChallan admissionChallan = await db.AdmissionChallans.FindAsync(id);
            if (admissionChallan == null)
            {
                return HttpNotFound();
            }

            return View(admissionChallan);
        }

        // GET: AdmissionChallans/Create
        public ActionResult Create()
        {
            ViewBag.AdmissionId = new SelectList(db.Admissions, "Id", "StatusReason");
            ViewBag.EntityStatusTypeId = new SelectList(db.EntityStatusTypes, "Id", "Name");
            return View();
        }

        // POST: AdmissionChallans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include =
                "Id,RefNo,Amount,FeeConcession,NetAmount,VerifiedBy,IsPaid,DatePaid,StatusReason,EntityStatusTypeId,AdmissionId,DueDate,MimeType,PaidChallan,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsActive")]
            AdmissionChallan admissionChallan)
        {
            if (ModelState.IsValid)
            {
                db.AdmissionChallans.Add(admissionChallan);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AdmissionId = new SelectList(db.Admissions, "Id", "StatusReason", admissionChallan.AdmissionId);
            ViewBag.EntityStatusTypeId =
                new SelectList(db.EntityStatusTypes, "Id", "Name", admissionChallan.EntityStatusTypeId);
            return View(admissionChallan);
        }

        // GET: AdmissionChallans/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AdmissionChallan admissionChallan = await db.AdmissionChallans.FindAsync(id);
            if (admissionChallan == null)
            {
                return HttpNotFound();
            }

            ViewBag.AdmissionId = new SelectList(db.Admissions, "Id", "StatusReason", admissionChallan.AdmissionId);
            ViewBag.EntityStatusTypeId =
                new SelectList(db.EntityStatusTypes, "Id", "Name", admissionChallan.EntityStatusTypeId);
            return View(admissionChallan);
        }

        // POST: AdmissionChallans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include =
                "Id,RefNo,Amount,FeeConcession,NetAmount,VerifiedBy,IsPaid,DatePaid,StatusReason,EntityStatusTypeId,AdmissionId,DueDate,MimeType,PaidChallan,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsActive")]
            AdmissionChallan admissionChallan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admissionChallan).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AdmissionId = new SelectList(db.Admissions, "Id", "StatusReason", admissionChallan.AdmissionId);
            ViewBag.EntityStatusTypeId =
                new SelectList(db.EntityStatusTypes, "Id", "Name", admissionChallan.EntityStatusTypeId);
            return View(admissionChallan);
        }

        [HttpGet]
        public async Task<JsonResult> MarkAs(long id)
        {
            object dd;
            try
            {
                if (id == 0)
                {
                    throw new NullReferenceException("Invalid Challan Form Reference!");
                }

                var paid = bool.Parse(Request.QueryString.Get("isPaid") ?? "true");
                var reason = Request.QueryString.Get("reason") ?? "";

                using (db)
                {
                    using (var trans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var admissionChallan = await db.AdmissionChallans.FindAsync(id);

                            if (admissionChallan == null) throw new NullReferenceException("No Challan Form Found!");

                            if (paid)
                            {
                                admissionChallan.DatePaid = DateTime.UtcNow;
                                admissionChallan.Admission.EntityStatusTypeId = 4;
                            }
                            else
                            {
                                admissionChallan.Admission.EntityStatusTypeId = 2;
                            }

                            admissionChallan.EntityStatusTypeId = paid ? 4 : 3;
                            admissionChallan.Admission.StatusReason = admissionChallan.StatusReason =
                                string.IsNullOrEmpty(reason)
                                    ? (paid ? "Payment Approved! Verified!" : "Payment Rejected! Not Valid!")
                                    : reason;
                            admissionChallan.IsPaid = paid;
                            admissionChallan.Admission.UpdatedBy =
                                admissionChallan.UpdatedBy = Session.GetSession().ApplicationUser.Id;
                            admissionChallan.Admission.UpdatedDate = admissionChallan.UpdatedDate = DateTime.UtcNow;

                            db.Entry(admissionChallan).State = EntityState.Modified;

                            await db.SaveChangesAsync();

                            var studentSection = await db.Sections.FirstOrDefaultAsync(f =>
                                f.Students.Count < f.Capacity &&
                                f.IsMorningShift == admissionChallan.Admission.IsMorningShift);

                            if (studentSection == null)
                                throw new NullReferenceException(
                                    "Contact Administrator as there aren't any Sections available and one section is only for one student.");

                            studentSection.Students.Add(
                                await db.Students.FindAsync(admissionChallan.Admission.StudentId));

                            db.Entry(studentSection).State = EntityState.Modified;

                            await db.SaveChangesAsync();

                            trans.Commit();
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                }

                dd = new {StatusCode = 200, Message = "Done!"};
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                dd = new {StatusCode = 400, e.Message};
            }

            return Json(dd, "application/json",
                Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        // GET: AdmissionChallans/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AdmissionChallan admissionChallan = await db.AdmissionChallans.FindAsync(id);
            if (admissionChallan == null)
            {
                return HttpNotFound();
            }

            return View(admissionChallan);
        }

        // POST: AdmissionChallans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            AdmissionChallan admissionChallan = await db.AdmissionChallans.FindAsync(id);
            db.AdmissionChallans.Remove(admissionChallan);
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