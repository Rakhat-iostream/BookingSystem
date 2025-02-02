﻿using System;
using System.Text;
using System.Threading.Tasks;
using Domain.Core.Helpers;
using Domain.Core.Helpers.Exceptions;
using Domain.Core.Models.Users;
using Domain.Interfaces.Repositories.Users;
using Domain.Interfaces.Services.Users;
using Infrastructure.Services.Utils;

namespace Infrastructure.Services.Users
{
    public class UsersService : BaseService<IUsersRepository, User, Guid>, IUserService
    {
        private readonly Notifier _notificationService;
        private readonly IPasswordEncryptor _encryptor;
        public UsersService(IUsersRepository userRepository, Notifier notificationService, IPasswordEncryptor encryptor) : base(userRepository)
        {
            _notificationService = notificationService;
            _encryptor = encryptor;
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = await _repository.GetByEmailAsync(email).ConfigureAwait(false);

            // check if username exists
            if (user == null) return null;
            // check if password is correct
            if (!_encryptor.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (await _repository.UserExistsAsync(user.Email))
                throw new AppException("Email \"" + user.Email + "\" is already taken");

            _encryptor.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = Roles.USER;
            await Task.WhenAll(/*_notificationService.SendEmailAsync(user.Email, password),*/ _repository.CreateAsync(user)).ConfigureAwait(false);
            return user;
        }

        public async Task<bool> UpdateUserWithPasswordAsync(User userParam, string password = null)
        {
            var user = await GetByIdAsync(userParam.Id);
            if (user == null)
                throw new AppException("User not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Email) && userParam.Email != user.Email)
            {
                // throw error if the new username is already taken
                if ((await _repository.GetByEmailAsync(userParam.Email)) != null)
                    throw new AppException("Email " + userParam.Email + " is already taken");

                user.Email = userParam.Email;
            }
            if (!string.IsNullOrWhiteSpace(userParam.MobilePhone)) user.MobilePhone = userParam.MobilePhone;
            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.Firstname))
                user.Firstname = userParam.Firstname;

            if (!string.IsNullOrWhiteSpace(userParam.Lastname))
                user.Lastname = userParam.Lastname;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            return await _repository.UpdateAsync(user).ConfigureAwait(false);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
