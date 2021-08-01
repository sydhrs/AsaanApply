using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using Antlr.Runtime.Misc;
using WebProject_NetFramework.DbContext;
using WebProject_NetFramework.DbContext.DbModels;
using WebProject_NetFramework.Models;

namespace WebProject_NetFramework.Controllers
{ 
    public class LayoutController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [OasAuthorize]
        public async Task<JsonResult> ResetPassword()
        {
            dynamic dd;

            try
            {
                var oldPassword = Request.Form.Get("oldPassword") ?? "";
                var newPassword = Request.Form.Get("newPassword") ?? "";
                if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
                    throw new NullReferenceException("Invalid Data passed in Fields. Please provide both Passwords");
                using (var con = db)
                {
                    if (Session.GetSession().ApplicationUser.Password != SharedClass.GetMd5Hash(oldPassword))
                    {
                        throw new NullReferenceException("Old Password Not Correct. Please Fix it.");
                    }

                    using (var trans = con.Database.BeginTransaction())
                    {
                        try
                        {
                            var auId = Session.GetSession().ApplicationUser.Id;
                            var au = await con.ApplicationUsers.FirstOrDefaultAsync(f => f.Id == auId);
                            if (au == null)
                                throw new NullReferenceException(
                                    "Invalid Session. Please Re-Login!");

                            au.Password = SharedClass.GetMd5Hash(newPassword);
                            con.Entry(au).State = EntityState.Modified;
                            await con.SaveChangesAsync();
                            trans.Commit();
                            Session.GetSession().ApplicationUser = au;
                            dd = new {StatusCode = 200, Message = "Password Successfully Changed!"};
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            throw e;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                dd = new {StatusCode = 401, Message = e.Message};
            }

            return Json(dd, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Login");
        }

        public async Task<ActionResult> Index()
        {
            var sys = await db.SystemSettings.FirstOrDefaultAsync();
            var ss = new SystemSetting {AdmissionsOpen = false};

            if (sys != null) return View(sys);
            
            db.SystemSettings.Add(ss);
            await db.SaveChangesAsync();
            return View(ss);
        }

        public async Task<ActionResult> Staff()
        {
            var list = await db.FacultyInfos.ToListAsync();

            return View(list);
        }

        public ActionResult ForgetPassword(string email)
        {
            var obj = db.ApplicationUsers.Where(x => x.Email == email).FirstOrDefault();
            if (obj != null)
            {
                SendEmail(obj.Email, obj.Id);
                return RedirectToAction("Login", "Layout");
            }
            else
            {
                return HttpNotFound();
            }
        }

        [NonAction]
        public void SendEmail(string Email, long Id)
        {
            try
            {
                var CreatePasswordURL = "/Layout/ResetPassword/" + Id;
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, CreatePasswordURL);
                var fromEmail = "arqumfarooq1@gmail.com";
                var fromEmailPassword = "02334723005";
                var toEmail = Email;
                var subject = "Reset Your Password using the Link in detail";
                var body = "<br/><br/> Please click on the link to reset your password" + "<br/><br/><a href='" + link +
                           "'>" + link + "</a>";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(fromEmail, fromEmailPassword)
                };
                var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    //Body = "hi your reset link is given below",
                    IsBodyHtml = true,
                };

                smtp.Send(message);
                smtp.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "The SMTP server requires a secure connection or the client was not authenticated. The server respon...");
                throw;
            }
        }

        [HttpGet]
        public ActionResult ResetPassword(long Id)
        {
            var v = db.ApplicationUsers.FirstOrDefault(x => x.Id == Id);
            return View(v);
        }

        [HttpPost]
        public ActionResult ResetPassword(long Id, string password)
        {
            var user = db.ApplicationUsers.FirstOrDefault(x => x.Id == Id);
            if (user != null)
            {
                user.Password = SharedClass.GetMd5Hash(password);
            }

            db.SaveChanges();

            return RedirectToAction("Login", "Layout");
        }
        // public ActionResult Campus()
        // {
        //     return View();
        // }

        public ActionResult Societies()
        {
            return View();
        }

        public ActionResult Programs()
        {
            return View();
        }

        public ActionResult RoadMap()
        {
            var list = db.RoadMaps.ToList();
            var ss = list
                .GroupBy(f => f.DegreePursuingTypeId, ff => ff)
                .ToDictionary(f => f.Key, f => f.ToList());

            return View(ss);
        }

        public async Task<ActionResult> News()
        {
            var list = await db.News.ToListAsync();

            return View(list);
        }

        public ActionResult Admission()
        {
            var list = db.AdmissionCriteria.ToList();
            var s = list.GroupBy(f => f.DegreePursuingTypeId, ff => ff).ToDictionary(f => f.Key, f => f.ToList());
            return View(s);
        }

        [HttpPost]
        public async Task<ActionResult> Message(DbContext.DbModels.Message message)
        {
            if (message == null || string.IsNullOrEmpty(message.Name) || string.IsNullOrEmpty(message.Email) ||
                string.IsNullOrEmpty(message.Message1))
            {
                TempData.Add("Message", "Invalid Parameters!");
                RedirectToAction("Index");
            }

            db.Messages.Add(message);
            await db.SaveChangesAsync();
            TempData.Add("Message", "Successfully Added!");
            return RedirectToAction("Index");
        }

        public ActionResult SystemSetting()
        {
            return View();
        }


        public ActionResult BSCSinfo()
        {
            return View();
        }

        public ActionResult MSCSinfo()
        {
            return View();
        }

        public ActionResult PHDInfo()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Gallery()
        {
            return View();
        }
    }
}