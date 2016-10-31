using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Repositories
{
    public class GigRepository
    {
        private readonly ApplicationDbContext _context;

        public GigRepository(ApplicationDbContext context
            )
        {
            _context = context;
        }

        public IEnumerable<Attendance> GetFutureAttendaces(string userId)
        {
            return _context.Attendances
                .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                .ToList();
        }
    }
}