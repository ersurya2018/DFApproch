using DF_Approch_Test.DB_Connect;
using DF_Approch_Test.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DF_Approch_Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            suryaContext dbobj = new suryaContext();

            return View();
        }
        [HttpPost]
        public IActionResult Index(UserInfoModel userobj)
        {
            suryaContext dbobj = new suryaContext();
            var res = dbobj.Userinfos.Where(m => m.Email == userobj.Email).FirstOrDefault();
            if(res==null)
            {
                TempData["invalid"] = "Invalid Email";
            }
            else
            {
                if(res.Email==userobj.Email && res.Password==userobj.Password)
                {
                    //here we write the code for Authentication
                    var claims = new[] { new Claim(ClaimTypes.Name, res.Name),
                                        new Claim(ClaimTypes.Email, res.Email) };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    authProperties);

                    //this code for session set
                    HttpContext.Session.SetString("Name", res.Email);
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    TempData["invalidpass"] = "Invalid password";
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddNewUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNewUser(UserInfoModel userobj)
        {
            suryaContext dbobj = new suryaContext();
            Userinfo tblobj = new Userinfo();
            tblobj.Name = userobj.Name;
            tblobj.Email = userobj.Email;
            tblobj.Mobile = userobj.Mobile;
            tblobj.Password = userobj.Password;
            dbobj.Userinfos.Add(tblobj);
            dbobj.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
