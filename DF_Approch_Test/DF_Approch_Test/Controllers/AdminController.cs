using DF_Approch_Test.DB_Connect;
using DF_Approch_Test.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DF_Approch_Test.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            suryaContext dbobj = new suryaContext();
            var resdata = dbobj.Students.ToList();
            List<StudentRegModel> Stumodobj = new List<StudentRegModel>();

            foreach (var item in resdata)
            {
                Stumodobj.Add(new StudentRegModel
                {
                    Stuid=item.Stuid,
                    Name=item.Name,
                    Fname=item.Fname,
                    Email=item.Email,
                    Mobile=item.Mobile,
                    Branch=item.Branch,
                    Address=item.Address,
                });
            }
            return View(Stumodobj);
        }
        public IActionResult Delete(int id)
        {
            suryaContext dbobj = new suryaContext();
            var resdel = dbobj.Students.Where(m => m.Stuid == id).FirstOrDefault();
            dbobj.Students.Remove(resdel);
            dbobj.SaveChanges();
            return RedirectToAction("Index","Admin");
        }
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddStudent(StudentRegModel stuobj)
        {
            suryaContext dbobj = new suryaContext();
            Student tblobj = new Student();
            tblobj.Stuid = stuobj.Stuid;
            tblobj.Name = stuobj.Name;
            tblobj.Fname = stuobj.Fname;
            tblobj.Email = stuobj.Email;
            tblobj.Mobile = stuobj.Mobile;
            tblobj.Branch = stuobj.Branch;
            tblobj.Address = stuobj.Address;
            if (stuobj.Stuid == 0)
            {
                dbobj.Students.Add(tblobj);
                dbobj.SaveChanges();
            }
            else
            {
                dbobj.Entry(tblobj).State = EntityState.Modified;
                dbobj.SaveChanges();

            }
            
            return RedirectToAction("Index", "Admin");
        }
        public IActionResult Edit(int id)
        {
            suryaContext dbobj = new suryaContext();
            var resdata = dbobj.Students.Where(m => m.Stuid == id).FirstOrDefault();
            StudentRegModel stumobj = new StudentRegModel();
               stumobj.Stuid = resdata.Stuid;
               stumobj.Name = resdata.Name;
               stumobj.Fname = resdata.Fname;
               stumobj.Email = resdata.Email;
               stumobj.Mobile = resdata.Mobile;
               stumobj.Branch = resdata.Branch;
               stumobj.Address = resdata.Address;
            return View("AddStudent", stumobj);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Name");
            return RedirectToAction("Index", "Home"); 
        }
    }
}
