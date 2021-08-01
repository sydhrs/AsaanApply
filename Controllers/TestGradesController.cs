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
using WebProject_NetFramework.Models;

namespace WebProject_NetFramework.Controllers
{
    public class TestGradesController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        // GET: TestGrades
        public async Task<ActionResult> Index()
        {
            var testGrades = db.TestGrades.Include(t => t.Student);
            return View(await testGrades.ToListAsync());
        }

        // GET: TestGrades/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TestGrade testGrade = await db.TestGrades.FindAsync(id);
            if (testGrade == null)
            {
                return HttpNotFound();
            }

            return View(testGrade);
        }

        // GET: TestGrades/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList((from stu in db.Students
                join per in db.Persons on stu.Id equals per.Id
                select new
                {
                    stu.Id,
                    FullName = per.FirstName + " " + per.MiddleName + " " + per.LastName
                }), "Id", "FullName");
            return View();
        }

        // POST: TestGrades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include =
                "Id,Score,Grade,Comment,StudentId,AdmissionId,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsActive")]
            TestGrade testGrade)
        {
            if (ModelState.IsValid)
            {
                var admission = await db.Admissions.FirstOrDefaultAsync(f => f.Id == testGrade.AdmissionId);
                if (admission == null)
                {
                    ViewBag.StudentId = new SelectList(
                        (from stu in db.Students
                            join per in db.Persons on stu.Id equals per.Id
                            select new
                            {
                                stu.Id, FullName = per.FirstName + " " + per.MiddleName + " " + per.LastName
                            }), "Id", "FullName", testGrade.StudentId);
                    var vdd = new ViewDataDictionary
                    {
                        {"MessageContent", "Please Specify an Admission Id!"},
                        {"MessageType", "error"}
                    };
                    ViewData = vdd;
                    return View(testGrade);
                }

                db.TestGrades.Add(testGrade);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList((from stu in db.Students
                join per in db.Persons on stu.Id equals per.Id
                select new
                {
                    stu.Id,
                    FullName = per.FirstName + " " + per.MiddleName + " " + per.LastName
                }), "Id", "FulllName", testGrade.StudentId);
            return View(testGrade);
        }


        public async Task<JsonResult> FinalizeMeritList()
        {
            var res = new {Message = "", StatusCode = 200};

            try
            {
                using (var con = db)
                {
                    using (var trans = con.Database.BeginTransaction())
                    {
                        try
                        {
                            (await con.TestGrades.ToListAsync()).ForEach(f =>
                            {
                                var adm = con.Admissions.FirstOrDefault(ff => ff.Id == f.AdmissionId);

                                con.MeritLists.Add(new MeritList
                                {
                                    DegreePursuingTypeId = adm.DegreePursuingTypeId,
                                    StudentId = f.StudentId,
                                    TestGradeId = f.Id,
                                    Aggregate = ((adm.StudentAcademics
                                        .FirstOrDefault(fff => fff.AcademicDegreeTypeId == 2)
                                        ?.MarksObtained ?? 0) * (decimal) 0.6) + (decimal.Parse(f.Score) *
                                        (decimal) 0.4)
                                });
                            });
                            await con.SaveChangesAsync();
                            trans.Commit();
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                    // 

                    using (var trans = con.Database.BeginTransaction())
                    {
                        try
                        {
                            var morningFinals = await con.MeritLists.OrderByDescending(f => f.Aggregate)
                                .Where(f => f.TestGrade.Admission.IsMorningShift).Take(150).ToListAsync();

                            var eveningFinals = await con.MeritLists.OrderByDescending(f => f.Aggregate)
                                .Where(f => !f.TestGrade.Admission.IsMorningShift).Take(100).ToListAsync();

                            var fc = new List<MeritList>(morningFinals);
                            fc.AddRange(eveningFinals);

                            foreach (var meritList in fc)
                            {
                                var admission =
                                    await db.Admissions.FirstOrDefaultAsync(
                                        f => f.Id == meritList.TestGrade.AdmissionId);

                                var acc = new AdmissionChallan
                                {
                                    Amount = 75000,
                                    IsActive = true,
                                    IsPaid = false,
                                    StatusReason = "Pending Payment",
                                    EntityStatusTypeId = 1,
                                    CreatedBy = Session.GetSession().ApplicationUser.Id,
                                    CreatedDate = DateTime.UtcNow,
                                    RefNo = SharedClass.GetChallanRefNo(admission),
                                    AdmissionId = admission.Id
                                };

                                db.AdmissionChallans.Add(acc);
                                await db.SaveChangesAsync();
                            }

                            await con.SaveChangesAsync();
                            trans.Commit();
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                res = new {Message = e.Message, StatusCode = 402};
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        // GET: TestGrades/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TestGrade testGrade = await db.TestGrades.FindAsync(id);
            if (testGrade == null)
            {
                return HttpNotFound();
            }

            ViewBag.StudentId = new SelectList((from stu in db.Students
                join per in db.Persons on stu.Id equals per.Id
                select new
                {
                    stu.Id,
                    FullName = per.FirstName + " " + per.MiddleName + " " + per.LastName
                }), "Id", "FullName", testGrade.StudentId);
            return View(testGrade);
        }

        // POST: TestGrades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include =
                "Id,Score,Grade,Comment,StudentId,AdmissionId,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsActive")]
            TestGrade testGrade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testGrade).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList((from stu in db.Students
                join per in db.Persons on stu.Id equals per.Id
                select new
                {
                    stu.Id,
                    FullName = per.FirstName + " " + per.MiddleName + " " + per.LastName
                }), "Id", "FullName", testGrade.StudentId);
            return View(testGrade);
        }

        // GET: TestGrades/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TestGrade testGrade = await db.TestGrades.FindAsync(id);
            if (testGrade == null)
            {
                return HttpNotFound();
            }

            return View(testGrade);
        }

        // POST: TestGrades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            TestGrade testGrade = await db.TestGrades.FindAsync(id);
            db.TestGrades.Remove(testGrade);
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