using Newtonsoft.Json;
using WebProject_NetFramework.DbContext;
using WebProject_NetFramework.DbContext.DbModels;
using WebProject_NetFramework.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace WebProject_NetFramework.Controllers
{
    public class StudentController : Controller
    {
        private readonly SchoolDbContext _db;

        public StudentController()
        {
            _db = new SchoolDbContext();
        }

        [OasAuthorize]
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        [OasAuthorize]
        public ActionResult Dashboard()
        {
            return View();
        }

        [OasAuthorize]
        public ActionResult Instruction()
        {
            return View();
        }

        // GET: Student
        [OasAuthorize]
        public async Task<ActionResult> Form()
        {
            Student std = new Student();
            var auId = Session.GetSession().ApplicationUser.Id;
            var person = await _db.Persons.Include(f => f.Address).Include(f => f.PersonContact)
                .Include(f => f.MaritalStatusType).Include(f => f.GenderType)
                .FirstOrDefaultAsync(f => f.ApplicationUserId == auId);
            //var auId = Session.GetSession().ApplicationUser.Id;
            //var person =  _db.Persons.Include(f => f.Address).Include(f => f.PersonContact).Include(f => f.MaritalStatusType).Include(f => f.GenderType).FirstOrDefaultAsync(f => f.ApplicationUserId == auId);
            return View(person);
        }

        [OasAuthorize]
        [HttpPost]
        public async Task<JsonResult> PostForm()
        {
            object dd;
            try
            {
                var form = Request.Form;
                var morningShiftType = form.GetValues("isMorningShift")?[0] ?? "single";

                var admission = JsonConvert.DeserializeObject<Admission>(form.Get("admissionJson"));
                //TODO: Replace this id with the Dynamic one...
                var studentId = Session.GetSession().ApplicationUser.PersonId;
                using (var con = _db)
                {
                    using (var trans = con.Database.BeginTransaction())
                    {
                        try
                        {
                            var student = await con.Students.FindAsync(studentId);
                            if (student == null)
                                throw new NullReferenceException("User Doesn't Exist");

                            student.Address = admission.Student.Address;
                            if (admission.Student.MaritalStatusTypeId != null &&
                                admission.Student.MaritalStatusTypeId != 0)
                                student.MaritalStatusTypeId = admission.Student.MaritalStatusTypeId;
                            student.PersonContact = admission.Student.PersonContact;
                            admission.Student = null;
                            con.Entry(student).State = EntityState.Modified;
                            await con.SaveChangesAsync();
                            admission.StudentId = student.Id;
                            admission.EntityStatusTypeId = 1;
                            if (morningShiftType == "single")
                            {
                                var existingAdm = student.Admissions.FirstOrDefault(f =>
                                    f.IsMorningShift == admission.IsMorningShift);
                                if (existingAdm != null &&
                                    (existingAdm.DegreePursuingTypeId == admission.DegreePursuingTypeId &&
                                     existingAdm.EntityStatusTypeId != 3))
                                    throw new NullReferenceException(
                                        "An Admission for the Selected Degree in the Selected Shift already Exists. Please Change and try Again.");
                                con.Admissions.Add(admission);
                                await con.SaveChangesAsync();

                                var cf = new StudentChallan //cf : challan form
                                {
                                    Amount = 700,
                                    IsActive = true,
                                    IsPaid = false,
                                    StatusReason = "Pending Payment",
                                    EntityStatusTypeId = 1,
                                    CreatedBy = Session.GetSession().ApplicationUser.Id,
                                    CreatedDate = DateTime.UtcNow,
                                    RefNo = SharedClass.GetChallanRefNo(admission),
                                    AdmissionId = admission.Id
                                };
                                con.StudentChallans.Add(cf);
                            }
                            else
                            {
                                var admission2 = JsonConvert.DeserializeObject<Admission>(form.Get("admissionJson"));
                                admission2.IsMorningShift = !admission.IsMorningShift;
                                admission2.Student = null;
                                admission2.StudentId = student.Id;
                                admission2.EntityStatusTypeId = 1;

                                var existingAdm = student.Admissions.FirstOrDefault(f =>
                                    f.IsMorningShift == admission.IsMorningShift);
                                if (existingAdm != null &&
                                    existingAdm.DegreePursuingTypeId == admission.DegreePursuingTypeId &&
                                    existingAdm.EntityStatusTypeId != 3)
                                    throw new NullReferenceException(
                                        "An Admission for the Selected Degree in the Selected Shift already Exists. Please Change and try Again.");

                                var existingAdm2 = student.Admissions.FirstOrDefault(f =>
                                    f.IsMorningShift == admission2.IsMorningShift);
                                if (existingAdm2 != null &&
                                    (existingAdm2.DegreePursuingTypeId == admission2.DegreePursuingTypeId &&
                                     existingAdm2.EntityStatusTypeId != 3))
                                    throw new NullReferenceException(
                                        "An Admission for the Selected Degree in the Selected Shift already Exists. Please Change and try Again.");

                                con.Admissions.Add(admission);
                                con.Admissions.Add(admission2);
                                await con.SaveChangesAsync();

                                var cf = new StudentChallan
                                {
                                    Amount = 700,
                                    IsActive = true,
                                    IsPaid = false,
                                    StatusReason = "Pending Payment",
                                    EntityStatusTypeId = 1,
                                    CreatedBy = Session.GetSession().ApplicationUser.Id,
                                    CreatedDate = DateTime.UtcNow,
                                    RefNo = SharedClass.GetChallanRefNo(admission),
                                    AdmissionId = admission.Id
                                };
                                var cf2 = new StudentChallan
                                {
                                    Amount = 700,
                                    IsActive = true,
                                    IsPaid = false,
                                    StatusReason = "Pending Payment",
                                    EntityStatusTypeId = 1,
                                    CreatedBy = Session.GetSession().ApplicationUser.Id,
                                    CreatedDate = DateTime.UtcNow,
                                    RefNo = SharedClass.GetChallanRefNo(admission),
                                    AdmissionId = admission2.Id
                                };

                                con.StudentChallans.Add(cf);
                                con.StudentChallans.Add(cf2);
                            }

                            await con.SaveChangesAsync();
                            trans.Commit();
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            Console.WriteLine(e);
                            //throw e;
                            throw;
                        }
                    }
                }

                dd = new {StatusCode = 200, Message = "Done!", Data = admission.Id};
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                dd = new {StatusCode = 400, Message = e.Message};
            }

            return Json(dd, JsonRequestBehavior.AllowGet);
        }

        [OasAuthorize]
        public async Task<JsonResult> GetTypes()
        {
            var degreePTs = await _db.DegreePursuingTypes.Where(f => f.IsActive).ToListAsync();
            var sTs = await _db.StudentTypes.Where(f => f.IsActive).ToListAsync();

            return Json(new {DegreePursuingTypes = degreePTs, StudentTypes = sTs}, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetStudentAcademicTypes(long id)
        {
            var res = (await _db.AcademicDegreeTypes.Where(f =>
                    f.IsActive && f.PrerequisiteDegreeTypeId == id)
                .ToListAsync()).Select(f => new
            {
                f.Id,
                f.Name,
                f.Value,
                f.Description,
                Boards = new List<AcademicDegreeBoardType>()
            }).ToList();

            var sss = await _db.AcademicDegreeBoardTypes.Where(f => f.IsActive).ToListAsync();

            foreach (var re in res)
            {
                var ss = sss.Where(f =>
                    f.PrerequisiteAcademicDegreeTypeIds.Trim().Split(',').Contains(re.Id.ToString())).ToList();
                if (ss.Count > 0)
                    re.Boards.AddRange(ss);
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding,
            JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }

        [HttpPost]
        public async Task<JsonResult> Register()
        {
            object dd;
            try
            {
                var form = Request.Form;
                var files = Request.Files;
                if (files.Count <= 0)
                    throw new NullReferenceException("For Profile Pic Selected!");
                var profilePic = files[0];
                if (profilePic == null)
                    throw new NullReferenceException("For Profile Pic Selected!");

                var student = JsonConvert.DeserializeObject<Student>(form.Get("personJson"));

                using (_db)
                {
                    using (var trans = _db.Database.BeginTransaction())
                    {
                        try
                        {
                            using (var binaryReader = new BinaryReader(profilePic.InputStream))
                            {
                                student.Photo = student.ApplicationUser.ProfilePic =
                                    binaryReader.ReadBytes(profilePic.ContentLength);
                            }

                            student.ApplicationUser.Password = SharedClass.GetMd5Hash(student.ApplicationUser.Password);
                            student.ApplicationUser.Role = "Student";
                            student.EnrollmentDate = DateTime.Now;
                            _db.Students.Add(student);
                            await _db.SaveChangesAsync();
                            student.ApplicationUser.PersonId = student.Id;
                            _db.Entry(student.ApplicationUser).State = EntityState.Modified;
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

                dd = new {StatusCode = 200, Message = "Done!", Data = student};
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                dd = new {StatusCode = 400, Message = e.Message};
            }

            return Json(dd, "application/json",
                Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> Login()
        {
            object dd;
            try
            {
                var form = Request.Form;
                var username = form.Get("Username");
                var password = form.Get("Password");

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    throw new NullReferenceException("Please Specify Username and Password!");

                username = username.ToLower();
                var md5Password = SharedClass.GetMd5Hash(password);

                using (_db)
                {
                    using (var trans = _db.Database.BeginTransaction())
                    {
                        try
                        {
                            var au = await _db.ApplicationUsers.FirstOrDefaultAsync(f =>
                                (f.Email.ToLower() == username || f.Username.ToLower() == username) &&
                                f.Password == md5Password);

                            if (au == null)
                                throw new NullReferenceException("Incorrect Username or Password!");

                            var ls = new LoginSession
                            {
                                Agent = Request.UserAgent,
                                Browser = Request.Browser.Browser,
                                IP = Request.UserHostAddress,
                                Device = "Browser",
                                IsActive = true,
                                OS = Request.Browser.Platform,
                                AppUserId = au.Id,
                                SessionToken = Guid.NewGuid().ToString(),
                                CreationDate = DateTime.UtcNow,
                            };
                            ls.RequestToken = Guid.NewGuid().ToString() + ls.SessionToken;

                            _db.LoginSessions.Add(ls);
                            await _db.SaveChangesAsync();

                            trans.Commit();
                            dd = new {StatusCode = 200, Message = "Logged In Successfully!", Data = au};
                            Session.SetSession(au, ls);
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                dd = new {StatusCode = 400, Message = e.Message};
            }

            return Json(dd, JsonRequestBehavior.AllowGet);
        }

        [OasAuthorize]
        public async Task<ActionResult> TestChallanForm()
        {
            var type = Request.QueryString?.Get("challanType") ?? "both";
            //Student/_TestChallanForm?challanType=both
            var stdId = Session.GetSession().ApplicationUser.PersonId;
            var sc = await _db.StudentChallans.Where(f =>
                    (type == "morning"
                        ? f.Admission.IsMorningShift
                        : (type == "evening"
                            ? !f.Admission.IsMorningShift
                            : f.EntityStatusTypeId == 1 || f.EntityStatusTypeId == 3)) &&
                    f.Admission.StudentId == stdId && f.EntityStatusTypeId == 1 || f.EntityStatusTypeId == 3)
                .ToListAsync();
            return View(sc);
        }

        [HttpPost]
        public async Task<JsonResult> UploadTestChallanForm(long id = 0)
        {
            object dd;
            try
            {
                if (id == 0)
                {
                    throw new NullReferenceException("Invalid Challan Form Reference!");
                }

                var files = Request.Files;
                if (files.Count <= 0)
                    throw new NullReferenceException("No Challan File Selected!");
                var profilePic = files[0];
                if (profilePic == null)
                    throw new NullReferenceException("No challan File Selected!");

                using (_db)
                {
                    using (var trans = _db.Database.BeginTransaction())
                    {
                        try
                        {
                            var challanForm = await _db.StudentChallans.FindAsync(id);
                            if (challanForm == null) throw new NullReferenceException("No Challan Form Found!");

                            using (var binaryReader = new BinaryReader(profilePic.InputStream))
                            {
                                challanForm.PaidChallan = binaryReader.ReadBytes(profilePic.ContentLength);
                            }

                            challanForm.UpdatedDate = DateTime.UtcNow;
                            challanForm.EntityStatusTypeId = 2;
                            challanForm.MimeType = profilePic.ContentType;
                            challanForm.StatusReason = "Awaiting Approval!";

                            _db.Entry(challanForm).State = EntityState.Modified;

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

        [OasAuthorize]
        public async Task<ActionResult> TestSchedule()
        {
            var userId = Session.GetSession().ApplicationUser.Id;
            var testSchedule = new List<TestScheduleDBModel>();

            using (var db = new SchoolDbContext())
            {
                var tempShSchedules =
                    db.StudentTestSchedules.Where(s => s.Student.ApplicationUserId == userId).ToList();
                foreach (var stdSchedule in tempShSchedules)
                {
                    var tempSchedule = await db.TestSchedules.FindAsync(stdSchedule.TestScheduleId);
                    if (!tempSchedule.IsActive) continue;
                    var venue = await db.EntranceTestVenues.FindAsync(tempSchedule.EntranceTestVenueId);
                    var tempModel = new TestScheduleDBModel
                        {TestSchedule = tempSchedule, EntranceTestVenues = venue};
                    testSchedule.Add(tempModel);
                }
            }

            return View(testSchedule);
        }

        [OasAuthorize]
        public async Task<ActionResult> Meritlist()
        {
            var meritListMorning = await _db.MeritLists.Where(f => f.TestGrade.Admission.IsMorningShift)
                .OrderByDescending(f => f.Aggregate).Select(f => new MeritListSelectModel
                {
                    StudentId = f.StudentId,
                    AdmissionId = f.TestGrade.AdmissionId,
                    StudentName = f.Student.FirstName + " " + f.Student.MiddleName + " " + f.Student.LastName,
                    FatherName = f.Student.PersonContact.GuardianName,
                    Score = f.TestGrade.Score,
                    TestGrade = f.TestGrade.Grade,
                    Aggregate = f.Aggregate,
                    DegreePursuingType = f.DegreePursuingType.Name,
                    StudentType = f.TestGrade.Admission.StudentType.Name
                }).ToListAsync();

            var meritListEvening = await _db.MeritLists.Where(f => !f.TestGrade.Admission.IsMorningShift)
                .OrderByDescending(f => f.Aggregate).Select(f => new MeritListSelectModel
                {
                    StudentId = f.StudentId,
                    AdmissionId = f.TestGrade.AdmissionId,
                    StudentName = f.Student.FirstName + " " + f.Student.MiddleName + " " + f.Student.LastName,
                    FatherName = f.Student.PersonContact.GuardianName,
                    Score = f.TestGrade.Score,
                    TestGrade = f.TestGrade.Grade,
                    Aggregate = f.Aggregate,
                    DegreePursuingType = f.DegreePursuingType.Name,
                    StudentType = f.TestGrade.Admission.StudentType.Name
                }).ToListAsync();


            var dd = new MeritListDataModel
            {
                MorningMeritList = meritListMorning,
                EveningMeritList = meritListEvening,
                HasNameInMorning =
                    meritListMorning.FirstOrDefault(f =>
                        f.StudentId == Session.GetSession().ApplicationUser.PersonId) != null,
                HasNameInEveningShift =
                    meritListEvening.FirstOrDefault(f =>
                        f.StudentId == Session.GetSession().ApplicationUser.PersonId) != null
            };

            return View(dd);
        }

        [OasAuthorize]
        public async Task<ActionResult> AdmissionChallanForm()
        {
            var type = Request.QueryString?.Get("challanType") ?? "both";
            var stdId = Session.GetSession().ApplicationUser.PersonId;
            var sc = await _db.AdmissionChallans.Where(f =>
                    (type == "morning"
                        ? f.Admission.IsMorningShift
                        : (type == "evening"
                            ? !f.Admission.IsMorningShift
                            : f.EntityStatusTypeId == 1 || f.EntityStatusTypeId == 3)) &&
                    f.Admission.StudentId == stdId && f.EntityStatusTypeId == 1 || f.EntityStatusTypeId == 3)
                .ToListAsync();
            return View(sc);
        }

        [HttpPost]
        public async Task<JsonResult> UploadAdmissionChallanForm(long id = 0)
        {
            object dd;
            try
            {
                if (id == 0)
                {
                    throw new NullReferenceException("Invalid Challan Form Reference!");
                }

                var files = Request.Files;
                if (files.Count <= 0)
                    throw new NullReferenceException("No Challan File Selected!");
                var profilePic = files[0];
                if (profilePic == null)
                    throw new NullReferenceException("No challan File Selected!");

                using (_db)
                {
                    using (var trans = _db.Database.BeginTransaction())
                    {
                        try
                        {
                            var challanForm = await _db.AdmissionChallans.FindAsync(id);
                            if (challanForm == null) throw new NullReferenceException("No Challan Form Found!");

                            using (var binaryReader = new BinaryReader(profilePic.InputStream))
                            {
                                challanForm.PaidChallan = binaryReader.ReadBytes(profilePic.ContentLength);
                            }

                            challanForm.UpdatedDate = DateTime.UtcNow;
                            challanForm.EntityStatusTypeId = 2;
                            challanForm.MimeType = profilePic.ContentType;
                            challanForm.StatusReason = "Awaiting Approval!";

                            _db.Entry(challanForm).State = EntityState.Modified;

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

        [OasAuthorize]
        public async Task<ActionResult> Sections()
        {
            var auId = Session.GetSession().ApplicationUser.Id;
            Person person = await _db.Persons.Include(f => f.Address).Include(f => f.PersonContact)
                .Include(f => f.MaritalStatusType).Include(f => f.GenderType)
                .FirstOrDefaultAsync(f => f.ApplicationUserId == auId);
            var admission = await _db.Admissions.Include(a => a.StudentAcademics).Include(a => a.DegreePursuingType)
                .Include(a => a.StudentAcademics).FirstOrDefaultAsync(a => a.StudentId == auId);
            var student = await _db.Students.Include(s => s.Section).FirstOrDefaultAsync(s => s.ApplicationUserId == auId);
            var meritlist = await _db.MeritLists.FirstOrDefaultAsync(s => s.StudentId == auId);
            var dd = new StudentProfileDataModel
                { ApplicationUser = Session.GetSession().ApplicationUser, Person = person, Admission = admission, Student = student, MeritList = meritlist};

            return View(dd);
        }

        [OasAuthorize]
        public async Task<ActionResult> _TestChallanForm(long id = 0)
        {
            var type = Request.QueryString?.Get("challanType") ?? "both";
            //Student/_TestChallanForm?challanType=both
            var stdId = Session.GetSession().ApplicationUser.PersonId;
            List<StudentChallan> sc;
            if (id == 0)
            {
                sc = await _db.StudentChallans.Where(f =>
                        (type == "morning"
                            ? f.Admission.IsMorningShift
                            : (type == "evening"
                                ? !f.Admission.IsMorningShift
                                : f.EntityStatusTypeId == 1 || f.EntityStatusTypeId == 3)) &&
                        f.Admission.StudentId == stdId && f.EntityStatusTypeId == 1 || f.EntityStatusTypeId == 3)
                    .ToListAsync();
            }
            else
            {
                sc = new List<StudentChallan> {await _db.StudentChallans.FindAsync(id)};
            }

            return View("_TestChallForm", sc);
        }

        [OasAuthorize]
        public ActionResult _AdmissionChallanForm(long id = 0)
        {
            return View("_AdmissionChallan");
        }

        [OasAuthorize]
        public ActionResult StudentProfile()
        {
            return View();
        }

        [OasAuthorize]
        public ActionResult Message()
        {
            return View();
            //return RedirectToAction("Dashboard", "Student");
        }

        [HttpPost]
        public async Task<ActionResult> MessageSD(DbContext.DbModels.Message message)
        {
            if (message == null || string.IsNullOrEmpty(message.Name) || string.IsNullOrEmpty(message.Email) ||
                string.IsNullOrEmpty(message.Message1))
            {
                TempData.Add("Message", "Invalid Parameters!");
                return RedirectToAction("Message", "Student");
            }

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();
            TempData.Add("Message", "Successfully Added!");
            return RedirectToAction("Message", "Student");
        }

        [OasAuthorize]
        public async Task<ActionResult> TrackApplication()
        {
            var stdId = Session.GetSession().ApplicationUser.PersonId;
            var list = await _db.Admissions.Include(f=>f.Student).Where(f => f.StudentId == stdId).ToListAsync();
            return View(list);
        }

        [OasAuthorize]
        public ActionResult AggregateCalculation()
        {
            return View();
        }

        [OasAuthorize]
        public ActionResult FAQ()
        {
            return View();
        }

        [OasAuthorize]
        public ActionResult GetHelp()
        {
            return View();
        }

        [OasAuthorize]
        public ActionResult Settings()
        {
            return View();
        }

        [OasAuthorize]
        public ActionResult Activity()
        {
            return View();
        }

        [OasAuthorize]
        public async Task<ActionResult> MyProfile()
        {
            //var auId = Session.GetSession().ApplicationUser.Id;
            //var person = await _db.Persons.Include(f => f.Address).Include(f => f.PersonContact)
            //    .Include(f => f.MaritalStatusType).Include(f => f.GenderType)
            //    .FirstOrDefaultAsync(f => f.ApplicationUserId == auId);
            //var admission = await _db.Admissions.Include(a => a.StudentAcademics).Include(a => a.DegreePursuingType)
            //    .Include(a => a.StudentAcademics).FirstOrDefaultAsync(a => a.StudentId == auId);
            //var dd = new StudentProfileDataModel
            //    {ApplicationUser = Session.GetSession().ApplicationUser, Person = person, Admission = admission};
            var auId = Session.GetSession().ApplicationUser.Id;
            Person person = await _db.Persons.Include(f => f.Address).Include(f => f.PersonContact)
                .Include(f => f.MaritalStatusType).Include(f => f.GenderType)
                .FirstOrDefaultAsync(f => f.ApplicationUserId == auId);
            var admission = await _db.Admissions.Include(a => a.StudentAcademics).Include(a => a.DegreePursuingType)
                .Include(a => a.StudentAcademics).FirstOrDefaultAsync(a => a.StudentId == auId);
            var student = await _db.Students.Include(s => s.Section).FirstOrDefaultAsync(s => s.ApplicationUserId == auId);
            var meritlist = await _db.MeritLists.FirstOrDefaultAsync(s => s.StudentId == auId);
            var dd = new StudentProfileDataModel
                { ApplicationUser = Session.GetSession().ApplicationUser, Person = person, Admission = admission, Student = student, MeritList = meritlist };
            return View(dd);
        }
    }
}