﻿using System.ComponentModel.DataAnnotations;

namespace Dotnet6MvcLogin.Models.DTO
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string UserID { get; set; }
        public string Role { get; set; }
    }
}
