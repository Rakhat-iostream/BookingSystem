﻿using Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.Users
{
    public interface IOwnersService : IModelService<Owner>
    {
        Task<bool> OwnerExists(Guid ownerId);
    }
}