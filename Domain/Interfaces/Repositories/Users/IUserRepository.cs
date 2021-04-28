﻿using Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories.Users
{
    public interface IUserRepository : IModelRepository<User>
    {
        Task<User> GetByEmailAsync(string Email);
        Task<bool> GetAnyEmail(string Email);
        Task<bool> UpdateUser(User userParam, string password = null);
        
    }
}
