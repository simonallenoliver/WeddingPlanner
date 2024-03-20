using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;

 
namespace WeddingPlanner.Controllers;

public class WeddingController : Controller
{
    private readonly ILogger<WeddingController> _logger;

    private MyContext _context;

    public WeddingController(ILogger<WeddingController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }
    [HttpGet("weddings")]
    public IActionResult Weddings()
    {
        List<Wedding> WeddingsFromDB = _context.Weddings.Include(w => w.Users).ThenInclude(g => g.User).ToList();
        return View("Weddings", WeddingsFromDB);
    }

    [HttpGet("weddings/planwedding")]
    public IActionResult PlanWedding()
    {
        return View("PlanWedding");
    }

    [HttpPost("weddings/submit")]
    public IActionResult SubmitWedding(Wedding newWedding)
    {
        // validations checked
        if (!ModelState.IsValid)
        {
            return View("PlanWedding");
        }
        // if the validations go through, then we add our form data to the db
        // in our context file, it knows to associate our Dish model with the Dishes table in our db
        _context.Add(newWedding);
        // ALWAYS SAVE CHANGES
        _context.SaveChanges();
        // then we redirect to our index page, in our Home controller
        return RedirectToAction("Weddings");

    }

    // get one wedding
    [HttpGet("weddings/{weddingId}")]
    public IActionResult ViewWedding(int weddingId)
    {
        Wedding? OneWedding = _context.Weddings.Include(w => w.Users).ThenInclude(ww => ww.User).FirstOrDefault(g => g.WeddingId == weddingId);
        
        if (OneWedding == null)
        {
            return RedirectToAction("Weddings");
        }

        return View("ViewWedding", OneWedding);
    }

    // delete wedding post route
    [HttpPost("weddings/{weddingId}/delete")]
    public IActionResult DeleteWedding(int weddingId)
    {

        // first we check to make sure this exists in our database before we try to delete it
        Wedding? WeddingToDelete = _context.Weddings.SingleOrDefault(w => w.WeddingId == weddingId);

        if (WeddingToDelete != null && WeddingToDelete.UserId == (int)HttpContext.Session.GetInt32("UserId"))
        {
            _context.Remove(WeddingToDelete);
            _context.SaveChanges();
        }
        return RedirectToAction("Weddings");
    }

    // toggle accept reject wedding
    [HttpPost("weddings/{weddingId}/toggle")]
    public IActionResult ToggleWedding(int weddingId)
    {
        int userId = (int)HttpContext.Session.GetInt32("UserId");

        GuestList? AcceptedGuest = _context.GuestLists.SingleOrDefault(u => u.UserId == userId && u.WeddingId == weddingId);
        
        if(AcceptedGuest == null)
        {
            GuestList newGuest = new(){UserId = userId, WeddingId =weddingId};
            _context.Add(newGuest);
        }
        else
        {
            _context.Remove(AcceptedGuest);
        }
        _context.SaveChanges();

        // redirects back to same page
        return Redirect(HttpContext.Request.Headers.Referer);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}