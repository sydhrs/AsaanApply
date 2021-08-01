using System;
using System.Collections.Generic;
using System.Data.Entity; // ToListAsync();
using System.Linq;
using System.Threading.Tasks; // perform async task 
using System.Web;
using System.Web.Mvc;
using WebProject_NetFramework.DbContext;
using WebProject_NetFramework.Models;

namespace WebProject_NetFramework.Controllers
{
    public class AdminController : Controller
    {
        private readonly SchoolDbContext _db;
        private SchoolDbContext db = new SchoolDbContext();

        public AdminController()
        {
            _db = new SchoolDbContext();
        }
        // GET: Admin
        [OasAuthorize(true)]
        public ActionResult Index()
        {
            return View();
        }

        [OasAuthorize(true)]
        public async Task<ActionResult> ApplicationsReceived()
        {
            //var stdId = Session.GetSession().ApplicationUser.PersonId;
            var list = await db.Admissions.ToListAsync();
            return View(list);
        }

        [OasAuthorize(true)]
        public ActionResult AdminProfile()
        {
            return View();
        }
    }
}