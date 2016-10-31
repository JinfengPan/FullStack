using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Repositories
{
    public class AttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

      
    }
}