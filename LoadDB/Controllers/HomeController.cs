using Azure.Messaging;
using LoadDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace LoadDB.Controllers
{
    public class HomeController : Controller
    {
        private readonly Prn2111Context _context;

        public HomeController(Prn2111Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var st = _context.Students.Include(s => s.Depart).ToList();
            ViewBag.st = st;
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string user, string pass)
        {
            ViewBag.u = user;
            ViewBag.p = pass;
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["dept"] = _context.Departments.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormCollection f)
        {
            try
            {
                int id = int.Parse(f["id"]);
                var x = _context.Students.Find(id);
                if (x != null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Student st = new Student
                    {
                        Id = id,
                        Name = f["name"],
                        Gender = string.IsNullOrEmpty(f["gender"]),
                        DepartId = f["dept"],
                        Dob = DateOnly.Parse(f["dob"]),
                        Gpa = double.Parse(f["gpa"])
                    };
                    _context.Students.Add(st);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
            }
            return RedirectToAction("Index");
        }
    }

}
