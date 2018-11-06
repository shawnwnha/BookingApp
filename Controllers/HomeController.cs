using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using bookingApp.Models;

namespace bookingApp.Controllers
{
    public class HomeController : Controller
    {
        private HomeContext _context;
        public HomeController(HomeContext context){
            _context = context;
        }
        private bool UserExists(string email)
        {
            if(_context.users.Where(data => data.Email == email).Count() > 0){
                return true;
            } 
            else{
                return false;
            }
        }
// ########################################################################################################
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegiValidator data)
        {
            if(UserExists(data.Email)){
                ModelState.AddModelError("Email", "Email is in use");
            }
            if(ModelState.IsValid){
                User NewUser = new User {
                    Firstname = data.Firstname,
                    Lastname = data.Lastname,
                    Email = data.Email,
                    Password = data.Password,
                };
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                string x = Hasher.HashPassword(NewUser,NewUser.Password);
                NewUser.Password = x;
                _context.users.Add(NewUser);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else{
                return View("Index");                
            }
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginValidator data)
        {
            if(ModelState.IsValid){
                User thisUser = _context.users.SingleOrDefault(u => u.Email == data.LoginId);
                if(thisUser != null){
                    var Hasher = new PasswordHasher<User>();
                    if(0 != Hasher.VerifyHashedPassword(thisUser, thisUser.Password, data.LoginPw)){
                        HttpContext.Session.SetInt32("ID",thisUser.UserId);
                        ViewData["User"] = thisUser;
                        return RedirectToAction("Main");
                    }
                    else{
                        ViewBag.Error = "Matching Password Failed. ";
                        return View("Index");
                    }
                }
                else{
                    ViewBag.Error = "No User Record Found.";
                    return View("Index");
                }
            }
            else{
                return View("Index");                
            }
        }
// ########################################################################################################
        [HttpGet]
        [Route("main")]
        public IActionResult Main(){
            int? Uid = HttpContext.Session.GetInt32("ID");
            if(HttpContext.Session.GetInt32("ID") == null){
                return RedirectToAction("Index");
            }
            else{
                List<Event> Events = _context.events.Include(data => data.Eventer).Include(data =>data.Attenders).ThenInclude(data => data.User).ToList();
                User thisUser = _context.users.SingleOrDefault(data => data.UserId == (int)Uid);
                ViewBag.events = Events;    
                ViewBag.user = thisUser.UserId;
                return View();
            }
        }
        [HttpGet]
        [Route("new")]
        public IActionResult EventForm(){
            return View();
        }

        [HttpPost]
        [Route("new")]
        public IActionResult SubmitForm(EventValidator data){
            if(ModelState.IsValid){
                if(data.Date < DateTime.Now){
                    ViewBag.Error ="Date has to be in future";
                    return View("EventForm");
                }
                else{
                    int? x = HttpContext.Session.GetInt32("ID");
                    string totalDuration = data.Duration + "" +data.Duration2;
                    DateTime thisDate = data.Date;
                    DateTime g = thisDate.Add(data.Time);
                    Event newEvent = new Event{
                        Title = data.Title,
                        Description = data.Description,
                        Datetime = g,
                        Duration = totalDuration,
                        UserId = (int)x
                    };
                    _context.events.Add(newEvent);
                    _context.SaveChanges();
                    return RedirectToAction("Main");
                }
            }
            else{
                return View("EventForm");
            }
        }

        [HttpGet]
        [Route("events/{id}")]
        public IActionResult EventPage(int id){
            int? Uid = HttpContext.Session.GetInt32("ID");
            Event thisEvent = _context.events.Include(data=> data.Eventer).Include(data => data.Attenders).ThenInclude(data => data.User).SingleOrDefault(data => data.EventId == id);
            ViewBag.thisEvent = thisEvent;
            ViewBag.thisUser = (int)Uid;
            return View();
        }
// ##################################################################################################
        [HttpGet]
        [Route("join/{id}")]
        public IActionResult Join(int id){
            int? x = HttpContext.Session.GetInt32("ID");
            Joint Attend = new Joint{
                UserId = (int)x,
                EventId = id,
            };
            _context.joint.Add(Attend);
            _context.SaveChanges();
            return RedirectToAction("Main");
        }
        [HttpGet]
        [Route("leave/{id}")]
        public IActionResult Leave(int id){
            int? x = HttpContext.Session.GetInt32("ID");
            Joint thisJoint = _context.joint.SingleOrDefault(data => data.UserId == (int)x && data.EventId == id);
            _context.joint.Remove(thisJoint);
            _context.SaveChanges();
            return RedirectToAction("Main");
        }
        [HttpGet]
        [Route("events/{id}/delete")]
        public IActionResult Delete(int id){
            int? Uid = HttpContext.Session.GetInt32("ID");
            Event thisEvent = _context.events.Include(data=> data.Eventer).Include(data => data.Attenders).ThenInclude(data => data.User).SingleOrDefault(data => data.EventId == id);
            _context.events.Remove(thisEvent);
            _context.SaveChanges();
            return RedirectToAction("Main");
        }
// ##################################################################################################
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
