using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Time_Clock.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Identity;

namespace Time_Clock.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Dashboard()
        {
            //show the most recent stamps
            ViewBag.Stamps = _context.TimeStamps
                .Include(t => t.User)
                .ToList().OrderByDescending(a => a.CreatedAt);
            return View();
        }

        [HttpGet("admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [HttpPost("admin/login")]
        public IActionResult AdminLogin(User attempt)
        {
            Console.WriteLine(attempt.Pin);
            if(attempt.Pin != "1234")
            {
                ModelState.AddModelError("Pin", "Incorrect Password.");
                return View("Admin");
            }
            return RedirectToAction("Index");
        }
        [HttpGet("/create")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/user/create")]
        public IActionResult CreateUser(User user)
        {
            if(ModelState.IsValid)
            {
                Console.WriteLine("Create the user");
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Pin = Hasher.HashPassword(user, user.Pin);
                _context.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }

        [HttpPost("/stamp")]
        public IActionResult CreateStamp(TimeStamp stamp)
        {
            ViewBag.Stamps = _context.TimeStamps
                .Include(t => t.User)
                .ToList().OrderByDescending(a => a.CreatedAt);
            if(stamp.Pin == null || stamp.ID == null)
            {
                ModelState.AddModelError("Pin", "Invalid ID/Pin");
                return View("Dashboard");
            }
            //get the user from the DB, check the password.
            int a;
            bool res = int.TryParse(stamp.ID, out a);
            var userInDB = _context.Users.FirstOrDefault(u => u.UserId == a);
            if(userInDB == null)
            {
                ModelState.AddModelError("Pin", "Invalid ID/Password");
                Console.WriteLine("User not found.");
                return View("Dashboard");
            }
            //check PW match
            var hasher = new PasswordHasher<TimeStamp>();
            var result = hasher.VerifyHashedPassword(stamp, userInDB.Pin, stamp.Pin);
            if(result == 0)
            {
                ModelState.AddModelError("Pin", "Invalid ID/Password");
                Console.WriteLine("Wrong pin.");
                return View("Dashboard");
            }

            if(stamp.Job == null)
            {
                ModelState.AddModelError("Job", "Job Name Required");
                return View("Dashboard");
            }

            stamp.UserId = userInDB.UserId;
            _context.Add(stamp);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpPost("/logout")]
        public IActionResult Logout(TimeStamp stamp)
        {
            Console.WriteLine(stamp.ID);
            Console.WriteLine(stamp.Pin);
            ViewBag.Stamps = _context.TimeStamps
                .Include(t => t.User)
                .ToList().OrderByDescending(a => a.CreatedAt);
            //get the user with that ID, check PW match
            if(stamp.Pin == null || stamp.ID == null)
            {
                ModelState.AddModelError("Status", "ID and Pin Fields Required");
                return View("Dashboard");
            }

            int a;
            bool res = int.TryParse(stamp.ID, out a);
            var userInDB = _context.Users.FirstOrDefault(u => u.UserId == a);
            if(userInDB == null)
            {
                ModelState.AddModelError("Status", "Invalid ID/Password");
                Console.WriteLine("User not found.");
                return View("Dashboard");
            }
            //check PW match
            var hasher = new PasswordHasher<TimeStamp>();
            var result = hasher.VerifyHashedPassword(stamp, userInDB.Pin, stamp.Pin);
            if(result == 0)
            {
                ModelState.AddModelError("Status", "Invalid ID/Password");
                Console.WriteLine("Wrong pin.");
                return View("Dashboard");
            }

            //get the last job user was working on. IF he wasn't logged in, don't create stamp
            List<TimeStamp> lastStamp = _context.TimeStamps.Where(t => t.UserId == userInDB.UserId).ToList();
            foreach(TimeStamp s in lastStamp)
                Console.WriteLine(s.Job);

            if(lastStamp[lastStamp.Count - 1].Job == null)
            {
                Console.WriteLine("Employee not logged on.");
                ModelState.AddModelError("Status", "Employee Not on Job");
                return View("Dashboard");
            }

            //log the user off the job (create new stamp for LoggedOff)
            Console.WriteLine("Log off " + userInDB.Name);
            TimeStamp newStamp = new TimeStamp();
            newStamp.UserId = userInDB.UserId;
            newStamp.Job = null;

            //add the stamp to the database
            _context.Add(newStamp);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}
