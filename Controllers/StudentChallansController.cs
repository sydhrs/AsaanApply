using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
    public class StudentChallansController : Controller
    {
        private readonly SchoolDbContext _db = new SchoolDbContext();

        [OasAuthorize(true)]
        // GET: StudentChallans
        public async Task<ActionResult> Index()
        {
            var studentChallans = _db.StudentChallans.Include(s => s.EntityStatusType);
            return View(await studentChallans.ToListAsync());
        }

        // GET: StudentChallans/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            StudentChallan studentChallan = await _db.StudentChallans.FindAsync(id);
            if (studentChallan == null)
            {
                return HttpNotFound();
            }

            return View(studentChallan);
        }

        // GET: StudentChallans/Create
        public ActionResult Create()
        {
            ViewBag.EntityStatusTypeId = new SelectList(_db.EntityStatusTypes, "Id", "Name");
            return View();
        }

        // POST: StudentChallans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include =
                "Id,RefNo,Amount,VerifiedBy,IsPaid,DatePaid,StatusReason,EntityStatusTypeId,MimeType,PaidChallan,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsActive")]
            StudentChallan studentChallan)
        {
            if (ModelState.IsValid)
            {
                _db.StudentChallans.Add(studentChallan);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EntityStatusTypeId =
                new SelectList(_db.EntityStatusTypes, "Id", "Name", studentChallan.EntityStatusTypeId);
            return View(studentChallan);
        }

        // GET: StudentChallans/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            StudentChallan studentChallan = await _db.StudentChallans.FindAsync(id);
            if (studentChallan == null)
            {
                return HttpNotFound();
            }

            ViewBag.EntityStatusTypeId =
                new SelectList(_db.EntityStatusTypes, "Id", "Name", studentChallan.EntityStatusTypeId);
            return View(studentChallan);
        }

        // POST: StudentChallans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include =
                "Id,RefNo,Amount,VerifiedBy,IsPaid,DatePaid,StatusReason,EntityStatusTypeId,MimeType,PaidChallan,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsActive")]
            StudentChallan studentChallan)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(studentChallan).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EntityStatusTypeId =
                new SelectList(_db.EntityStatusTypes, "Id", "Name", studentChallan.EntityStatusTypeId);
            return View(studentChallan);
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

                using (_db)
                {
                    using (var trans = _db.Database.BeginTransaction())
                    {
                        try
                        {
                            var studentChallan = await _db.StudentChallans.FindAsync(id);

                            if (studentChallan == null) throw new NullReferenceException("No Challan Form Found!");

                            if (paid)
                            {
                                studentChallan.DatePaid = DateTime.UtcNow;
                                studentChallan.Admission.EntityStatusTypeId = 4;
                            }else{ studentChallan.Admission.EntityStatusTypeId = 2; }

                            studentChallan.EntityStatusTypeId = paid ? 4 : 3;
                            studentChallan.Admission.StatusReason = studentChallan.StatusReason = string.IsNullOrEmpty(reason)
                                ? (paid ? "Payment Approved! Verified!" : "Payment Rejected! Not Valid!")
                                : reason;
                            studentChallan.IsPaid = paid;
                            studentChallan.Admission.UpdatedBy = studentChallan.UpdatedBy = Session.GetSession().ApplicationUser.Id;
                            studentChallan.Admission.UpdatedDate = studentChallan.UpdatedDate = DateTime.UtcNow;

                            _db.Entry(studentChallan).State = EntityState.Modified;

                            await _db.SaveChangesAsync();
                            var newStudentTestSchedule = new StudentTestSchedule();

                            var testSchedule =
                                await _db.TestSchedules.Include(f=>f.StudentTestSchedules).FirstOrDefaultAsync(t =>
                                    t.ReservedSeat < t.TotalSeat &&
                                    t.StudentTestSchedules.FirstOrDefault(f =>
                                        f.StudentId == studentChallan.Admission.StudentId) == null);
                            if (testSchedule == null)
                                throw new NullReferenceException(
                                    "Contact Administrator as there aren't any Test Venues available.");

                            newStudentTestSchedule.AdmissionId = studentChallan.AdmissionId;
                            newStudentTestSchedule.StudentId = studentChallan.Admission.StudentId;
                            newStudentTestSchedule.TestScheduleId = testSchedule.Id;

                            _db.StudentTestSchedules.Add(newStudentTestSchedule);
                            await _db.SaveChangesAsync();

                            testSchedule.ReservedSeat += 1;

                            _db.Entry(testSchedule).State = EntityState.Modified;
                            await _db.SaveChangesAsync();

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

        // GET: StudentChallans/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            StudentChallan studentChallan = await _db.StudentChallans.FindAsync(id);
            if (studentChallan == null)
            {
                return HttpNotFound();
            }

            return View(studentChallan);
        }

        // POST: StudentChallans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            StudentChallan studentChallan = await _db.StudentChallans.FindAsync(id);
            _db.StudentChallans.Remove(studentChallan);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}