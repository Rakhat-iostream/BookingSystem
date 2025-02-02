﻿using Domain.Core.Models.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Utils
{
    class FilterBuilder
    {
        private IQueryable<LeisureService> _services;
        public FilterBuilder(IQueryable<LeisureService> services)
        {
            _services = services;
        }
        public FilterBuilder WithCategory(Guid categoryId)
        {
            if (categoryId != default)
            {
                _services = _services.Where(s => s.CategoryId == categoryId);
            }
            return this;
        }

        public FilterBuilder WithRating(int rating)
        {
            if (rating != default)
            {
                _services = _services.Where(s => Math.Round(s.Rating) == rating);
            }
            return this;
        }
        public FilterBuilder WithWorkingTime(string workingTime)
        {
            if (!string.IsNullOrEmpty(workingTime))
            {
                _services = _services.Where(s => s.WorkingTime == workingTime);
            }
            return this;
        }

        public IQueryable<LeisureService> Build()
        {
            return _services;
        }
    }
}
