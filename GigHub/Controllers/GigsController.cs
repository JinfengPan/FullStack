using GigHub.Models;
using GigHub.Repositories;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttendanceRepository _attendanceRepository;
        private readonly GigRepository _gigRepository;
        private readonly FollowingRepository _followingRepository;
        private readonly GenreRepository _genreRepository;

        public GigsController()
        {
            _context = new ApplicationDbContext();
            _attendanceRepository = new AttendanceRepository(_context);
            _gigRepository = new GigRepository(_context);
            _followingRepository = new FollowingRepository(_context);
            _genreRepository = new GenreRepository(_context);
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            return View("Gigs", new GigsViewModel
            {
                ShowActions = true,
                UpcomingGigs = _gigRepository.GetGigsUserAttending(userId),
                Heading = "Gigs I'm Attending",
                Attendances = _attendanceRepository.GetFutureAttendaces(userId).ToLookup(a => a.GigId)
            });
        }


        //@Hi111111
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = _genreRepository.GetGenres(),
                Heading = "Add a Gig"
            };
            return View("GigForm", viewModel);
        }


        [Authorize]
        public ActionResult Edit(int id)
        {

            var gig = _gigRepository.GetGig(id);

            if (gig == null)
                return HttpNotFound();

            if(gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();



            var viewModel = new GigFormViewModel
            {
                Id = gig.Id,
                Genres = _context.Genres.ToList(),
                Date = gig.DateTime.ToString("yyyy/MM/dd"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue,
                Heading = "Edit a Gig"
            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }
            

            var gig = _gigRepository.GetGigWithAttendees(viewModel.Id);
            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();


            gig.Modify(viewModel.DateTime, viewModel.Venue, viewModel.Genre);
            
            _context.SaveChanges();
            return RedirectToAction("Mine", "Gigs");
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
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
            var gigs = _gigRepository.GetUpcomingGigsByArtist(userId);

            return View(gigs);

        }

        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new {query = viewModel.SearchTerm});
        }


        public ActionResult Details(int id)
        {
            var gig = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .SingleOrDefault(g => g.Id == id);


            if(gig == null)
                return HttpNotFound();

            var viewModel = new GigDetailsViewModel {Gig = gig};

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                viewModel.IsAttending = 
                _attendanceRepository.GetAttendace(gig.Id, userId) != null;

                viewModel.IsFollowing =
                    _followingRepository.GetFollowing(userId, gig.ArtistId) != null;

            }

            return View("Details", viewModel);
        }


    }

    internal class GenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Genre> GetGenres()
        {
            return _context.Genres.ToList();
        }
    }

    internal class FollowingRepository
    {
        private readonly ApplicationDbContext _context;

        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Following GetFollowing(string userId, string artistId)
        {
            return _context.Followings.SingleOrDefault(f => f.FolloweeId == artistId && f.FollowerId == userId);
        }
    }
}