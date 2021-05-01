﻿using Domain.Interfaces.Services;
using Domain.Models.Users;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure;

namespace Booking.Infrastructure.Tests
{
    public class UsersServiceTest
    {
        private User user;
        private NotificationService _notifyService;
        private EmailConfiguration _conf;
        /*private string _connectionString = "Server=localhost; Port=5432; Database=booking; User Id=postgres; Password=Rubin1!!";
        private DbContextOptionsBuilder<BookingContext> _optionsBuilder = new DbContextOptionsBuilder<BookingContext>();*/
        public UsersServiceTest()
        {
        }
        [Theory]
        [InlineData("vhsboi322")]
        public async Task RegisterAsync_CheckIfThrows_Exception(string password)
        {
            //Arrange
            /*using var context = new BookingContext(_optionsBuilder.UseNpgsql(_connectionString).Options);
            user = new User
            {
                Firstname = "Alemkhan",
                Lastname = "Yergaliev",
                Email = "a.yergaliev@gmail.com",
                MobilePhone = "8(800)555-35-35"
            };*/
            _conf = new EmailConfiguration
            {
                From = "hasenovsultanbek@gmail.com",
                SmtpServer = "smtp.gmail.com",
                Username = "hasenovsultanbek@gmail.com",
                Port = 465,
                Password = "Rubin1!!"
            };
            _notifyService = new NotificationService(_conf);
            //_userService = new UsersService(new UsersRepository(context));
            //Act
            await _notifyService.SendEmailAsync("hasenovsultanbek@gmail.com");
            //var t = await _userService.RegisterAsync(user, password);
            //Assert
            //Assert.Throws>();
        }
    }
}
