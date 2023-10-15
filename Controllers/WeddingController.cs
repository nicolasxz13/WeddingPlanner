using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Data;
using WeddingPlanner.Filters;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class WeddingController : Controller
    {
        private readonly LoginContext _context;

        public WeddingController(LoginContext loginContext)
        {
            _context = loginContext;
        }

        [HttpGet("weddings")]
        [SessionCheck]
        public IActionResult Index()
        {
            RemoveOldWedding();
            string username = HttpContext.Session.GetString("UserName");
            int? userid = HttpContext.Session.GetInt32("UserId");
            List<WeddingViewModel> WeddingView = _context.Weddings
                .Include(a => a.Rsvs)
                .ThenInclude(a => a.User)
                .Where(a => a.Rsvs.Any(r => r.User.Id == userid) || (a.Creator.Id == userid))
                .Select(
                    a =>
                        new WeddingViewModel
                        {
                            Id = a.Id,
                            Married = a.Wedder_One + " & " + a.Wedder_Two,
                            GuestCount = a.Rsvs.Count(),
                            Creator = a.Creator.Id == userid,
                            Asist = a.Rsvs.Any(r => r.User.Id == userid && r.Asist),
                            Date = a.Date
                        }
                )
                .ToList();
            return View(WeddingView);
        }

        [HttpGet("weddings/new")]
        [SessionCheck]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("weddings/create")]
        [SessionCheck]
        public IActionResult Create(Wedding wedding)
        {
            int? userid = HttpContext.Session.GetInt32("UserId");
            User? user = _context.Users.FirstOrDefault(a => a.Id == userid);
            if (ModelState.IsValid && user != null)
            {
                wedding.Creator = user;
                wedding.CreatedAt = DateTime.Now;
                wedding.UpdatedAt = DateTime.Now;
                _context.Add(wedding);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("New");
            }
        }

        [HttpGet("weddings/{id}")]
        [SessionCheck]
        public IActionResult Show(int id)
        {
            Wedding? temp = _context.Weddings
                .Include(a => a.Rsvs)
                .ThenInclude(a => a.User)
                .SingleOrDefault(a => a.Id == id);
            if (temp == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(temp);
            }
        }

        [HttpPost("weddings/{id}/rsvp")]
        [SessionCheck]
        public IActionResult Rsvp(int id)
        {
            int? userid = HttpContext.Session.GetInt32("UserId");
            Rsv? rsv = _context.Rsvs.FirstOrDefault(a => a.User.Id == userid && a.wedding.Id == id);
            if (rsv == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                rsv.Asist = !rsv.Asist;
                rsv.UpdatedAt = DateTime.Now;
                _context.Update(rsv);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [HttpPost("weddings/{id}/destroy")]
        [SessionCheck]
        public IActionResult Delete(int id)
        {
            int? userid = HttpContext.Session.GetInt32("UserId");
            Wedding? wedding = _context.Weddings
                .Include(a => a.Rsvs)
                .SingleOrDefault(a => a.Creator.Id == userid && a.Id == id);
            if (wedding != null)
            {
                _context.Rsvs.RemoveRange(wedding.Rsvs);
                _context.Weddings.Remove(wedding);

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        private void RemoveOldWedding()
        {
            List<Wedding>? weddings = _context.Weddings
                .Include(a => a.Rsvs)
                .Where(a => a.Date < DateTime.Now)
                .ToList();
            if (weddings.Any())
            {
                foreach (Wedding result in weddings)
                {
                    _context.Rsvs.RemoveRange(result.Rsvs);
                    _context.Weddings.Remove(result);
                }
                _context.SaveChanges();
            }
        }
    }
}
