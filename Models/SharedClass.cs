using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebProject_NetFramework.DbContext.DbModels;
//using Humanizer;

namespace WebProject_NetFramework.Models
{
    public static class SharedClass
    {

        //public static string NOtoWD(decimal no)
        //{
        //    Humanizer hu =new Humanizer();
        //}
        public static string GetIsActive(this HttpRequestBase request, string urlPart,
            string className = "m-menu__item--active")
        {
            return request.RawUrl.Contains(urlPart) && request.RawUrl.Length <= (urlPart.Length + 1) &&
                   request.RawUrl.Length >= urlPart.Length
                ? className
                : request.RawUrl;
        }

        public static AppSession GetSession(this HttpSessionStateBase session)
        {
            if (session["__Session__"] == null)
            {
                session["__Session__"] = new AppSession();
            }

            return session["__Session__"] as AppSession;
        }

        public static bool IsSessionValid(this HttpSessionStateBase session)
        {
            if (session.GetSession().ApplicationUser == null || session.GetSession().LoginSession == null)
                return false;
            return session.GetSession().LoginSession.ExpiryDate > DateTime.UtcNow;
        }

        public static void SetSession(this HttpSessionStateBase session, ApplicationUser applicationUser,
            LoginSession loginSession)
        {
            session["__Session__"] = new AppSession { ApplicationUser = applicationUser, LoginSession = loginSession };
        }

        public static string GetChallanRefNo(Admission admission, string prefix = "BM", string mask = "00000")
        {
            return prefix + mask.Substring(0, mask.Length - admission.Id.ToString().Length) + admission.Id;
        }
        
         public static string GetMd5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(Encoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            var result = md5.Hash;

            var strBuilder = new StringBuilder();
            foreach (var t in result)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(t.ToString("x2"));
            }

            return strBuilder.ToString();
        }

        //public static string MD5Hash(string input)      
        //{
        //    StringBuilder hash = new StringBuilder();
        //    MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
        //    byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        hash.Append(bytes[i].ToString("x2"));
        //    }
        //    return hash.ToString();
        //}
    }

         public class AppSession
    {
        public ApplicationUser ApplicationUser { get; set; }
        public LoginSession LoginSession { get; set; }
        public string LastValidAddress { get; set; }
    }

         [AttributeUsage(AttributeTargets.Method)]
         public class OasAuthorize : AuthorizeAttribute
         {
           public string LoginViewName { get; set; }

           public bool IsForAdmin { get; set; } = false;
        //        private string CurrentViewName { get; set; }

            public OasAuthorize(bool isAdmin = false)
        {
            LoginViewName = "~/Views/Layout/Login.cshtml";
            IsForAdmin = isAdmin;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var session = filterContext.Controller.ControllerContext.HttpContext.Session;

            if (session.IsSessionValid())
            {
                if ((IsForAdmin && session.GetSession().ApplicationUser.Role != "Student") ||
                    (!IsForAdmin && session.GetSession().ApplicationUser.Role == "Student"))
                {
                    if (string.IsNullOrEmpty(session.GetSession().LastValidAddress) &&
                        !filterContext.RouteData.Values["controller"].Equals("Data"))
                        session.GetSession().LastValidAddress =
                            $"{filterContext.RouteData.Values["controller"]}/{filterContext.RouteData.Values["action"]}.cshtml";
                }
                else
                {
                    var vdd = new ViewDataDictionary
                    {
                        {"MessageContent", "Sorry you don't have the Required permissions to access that View."},
                        {"MessageType", "warning"}
                    };

                    var result = new ViewResult()
                    {
                        ViewName = "~/Views/" +
                                   (session.GetSession().LastValidAddress.Contains("Student") &&
                                    session.GetSession().LastValidAddress.Contains("Index")
                                       ? session.GetSession().LastValidAddress.Replace("Index", "Dashboard")
                                       : session.GetSession().LastValidAddress),
                        ViewData = vdd
                    };
                    filterContext.Result = result;
                }
            }
            else
            {
                var vdd = new ViewDataDictionary
                {
                    {"MessageContent", "Not Logged in. Please Log in First!"}, {"MessageType", "error"}
                };

                var result = new ViewResult() { ViewName = LoginViewName, ViewData = vdd };
                filterContext.Result = result;
            }
        }
    }
}