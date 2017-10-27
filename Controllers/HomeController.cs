using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using belt.Models;

namespace belt.Controllers
{
    public class HomeController : Controller
    {
        private beltContext _context;
        public HomeController(beltContext context)
        {
            _context = context;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.errors = ModelState.Values;
            ViewBag.success = TempData["success"];
            ViewBag.fail = TempData["fail"];
            return View("index");
        }
        [HttpPost]
        [Route("addUser")]
        public IActionResult addUser(User user)
        {
            if (TryValidateModel(user) == false)
            {
                ViewBag.errors = ModelState.Values;
                return View("index");
            }
            else
            {
                _context.users.Add(user);
                _context.SaveChanges();
                TempData["success"] = "User has been created.";
                return RedirectToAction("index");

            }
        }
        [HttpPost]
        [Route("loginUser")]
        public IActionResult loginUser(string email, string password)
        {
            // int? sesh = HttpContext.Session.GetInt32("userid");
            User aUser = _context.users.SingleOrDefault(p => p.email == email);
            if (email == null || password == null)
            {
                TempData["fail"] = "Email or Password is invalid";
                return RedirectToAction("index");

            }
            else if (email != aUser.email || password != aUser.password)
            {
                TempData["fail"] = "Email or Password is invalid";
                return RedirectToAction("index");
            }
            else
            {
                // User aUser = _context.Users.SingleOrDefault(p => p.username == username);
                HttpContext.Session.SetString(key: "email", value: aUser.email);
                HttpContext.Session.SetInt32(key: "userid", value: aUser.userid);
                return RedirectToAction("dashboard");
            }
        }
        [HttpGet]
        [Route("dashboard")]
        public IActionResult dashboard()
        {
                     
            int? sesh = HttpContext.Session.GetInt32("userid");
            int sesh2 = sesh ?? default(int);
            User user = _context.users.SingleOrDefault(s => s.userid == sesh2);
            ViewBag.user = user;
            ViewBag.post = TempData["addPost"];

            List<Post> allPost = _context.posts.Where(i => i.postid > 0).Include(u => u.user).ThenInclude(e => e.likedpost).ToList();
            ViewBag.allPost = allPost;
            
            List<Like> allLikes = _context.likes.Include(i => i.user).Include(e => e.post).ToList();
            foreach(var i in allLikes)
            {
                ViewBag.like = i;
            }
            ViewBag.sesh = sesh2;
            return View("dashboard");
        }
        [HttpPost]
        [Route("addPost")]
        public IActionResult addPost(string comment)
        {
            int? sesh = HttpContext.Session.GetInt32("userid");
            int sesh2 = sesh ?? default(int);
            Post aPost = new Post()
            {
                comment = comment,
                userid = sesh2,
            };
            _context.posts.Add(aPost);
            _context.SaveChanges();
            TempData["addPost"] = "Post has been added.";


            return RedirectToAction("dashboard");
        }
        [HttpGet]
        [Route("displayPost/{postid}")]
        public IActionResult Display (int postid)
        {
            List<Post> allPost = _context.posts.Where(i => i.postid > 0).Include(u => u.user).ThenInclude(e => e.likedpost).ToList();
            List<Post> aPost = _context.posts.Where(i => i.postid == postid).Include(u => u.user).ThenInclude(e => e.likedpost).ToList();
            // List<Post> posts = _context.posts.Where()
            ViewBag.allPost = aPost;
            List<User> user = aPost.Select(e => e.user).Distinct().ToList();
            foreach(var i in user)
            {
                ViewBag.hi = i;
            }
            foreach(var q in aPost)
            {
                ViewBag.like = q;
            } 
            // List<Post> post = _context.posts.Where(e => e.postid == postid).ToList();
            // ViewBag.post = post;
            return View("display");
        }
        [HttpGet]
        [Route("user/{userid}")]
        public IActionResult showUser(int userid)
        {
            
            List<User> join = _context.users.Include(w => w.createdpost).ToList();
            User aUser = _context.users.SingleOrDefault(e => e.userid == userid);
            List<User> touser= _context.users.Where(e => e.userid == userid).Include(w => w.createdpost).ToList();
            ViewBag.touser = touser;
            foreach(var i in touser)
            {
                    ViewBag.poop = i;
            }
            ViewBag.aUser = aUser;
          
            ViewBag.join = join;
         
            List<Like> likes = _context.likes.Where(e => e.userid == userid).Include(w => w.user).ToList();
            ViewBag.count = likes;
            List<Like> allLikes = _context.likes.Where(e => e.userid == userid).ToList();
            ViewBag.like = allLikes.Count;
            
            return View("user");
        }
        [HttpGet]
        [Route("like/{postid}")]
        public IActionResult Like(int postid)
        {
            int? sesh = HttpContext.Session.GetInt32("userid");
            int sesh2 = sesh ?? default(int);
            List<Post> likedposts = _context.posts.Include(e => e.likedby).ThenInclude(y => y.user).ToList();
            Like aLike = new Like()
            {
                userid = sesh2,
                postid = postid,
            };
            _context.Add(aLike);
            _context.SaveChanges();
            return RedirectToAction("dashboard");
        }
        [HttpGet]
        [Route("delete/{postid}")]
        public IActionResult delete(int postid)
        {
            int? sesh = HttpContext.Session.GetInt32("studentid");
            int sesh2 = sesh ?? default(int);
            Post post = _context.posts.SingleOrDefault(b => b.postid == postid);
            List<Like> postit = _context.likes.Where(b => b.postid == post.postid).ToList();
            foreach(var poop in postit)
            {
                _context.likes.Remove(poop);
                _context.SaveChanges();
            }
            _context.posts.Remove(post);
            _context.SaveChanges();
            


            return RedirectToAction("dashboard");
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index");
        }
    }
    }


