﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces.Repositories.Booking;
using Domain.Models.Booking;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Booking
{
    public class LeisureServicesRepository : BaseRepository<LeisureService>,ILeisureServicesRepository
    {
        public LeisureServicesRepository(BookingContext context) : base(context)
        {
        }
        public override async Task<LeisureService> GetByIdAsync(Guid id)
        {
            return await _context.LeisureServices.Include(s => s.Category).Include(s => s.Images).FirstOrDefaultAsync(s => s.Id == id);
        }
        public override async Task<IEnumerable<LeisureService>> GetAllAsync()
        {
            var services = await _context.LeisureServices.Include(s => s.Category).Include(s => s.Images).ToListAsync();
            return services;
        }

        public async Task<IEnumerable<LeisureService>> GetByRating(int rating)
        {
            return await _context.LeisureServices.Include(s => s.Images).Include(s => s.Category).
                Where(s => s.Rating == rating).ToListAsync();
        }

        public async Task<IEnumerable<LeisureService>> GetByWorkingTime(string workingTime)
        {
            return await _context.LeisureServices.Include(s => s.Images).Include(s => s.Category).
                Where(s => s.WorkingTime == workingTime).ToListAsync();
        }

        public async Task<IEnumerable<LeisureService>> GetByCategoryId(Guid categoryId)
        {
            return await _context.LeisureServices.Where(s => s.CategoryId == categoryId).Include(s => s.Images).Include(s => s.Category).ToListAsync();
        }
    }
}