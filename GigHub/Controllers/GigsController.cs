using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var gigs = _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();

            return View("Gigs", new GigsViewModel
            {
                ShowActions = true,
                UpcomingGigs = gigs,
                Heading = "Gigs I'm Attending"

            });
        }

        //@Hi111111
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList()
            };
            return View(viewModel);
        }


        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();

            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);



            var viewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList(),
                Date = gig.DateTime.ToString("yyyy MM dd"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue
            };
            return View("Create", viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("Create", viewModel);
            }

            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = DateTime.Parse($"{viewModel.Date} {viewModel.Time}"),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();
            return RedirectToAction("Mine", "Gigs");
        }


        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Gigs
                .Where(g => g.ArtistId == userId && g.DateTime > DateTime.Now)
                .Include(g => g.Genre)
                .ToList();



            return View(gigs);

        }
    }
}