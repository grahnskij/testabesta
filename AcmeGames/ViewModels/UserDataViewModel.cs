﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AcmeGames.Models;

namespace AcmeGames.ViewModels
{
    public class UserDataViewModel
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Birth { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Password { get; set; }

        public UserDataViewModel(User user)
        {
            Firstname = user.FirstName;
            Lastname = user.LastName;
            Birth = user.DateOfBirth.ToString("yyyy-MM-dd");
            Email = user.EmailAddress;
            Role = user.IsAdmin ? "Admin" : "User";
            Password = user.Password;
        }
    }
}


