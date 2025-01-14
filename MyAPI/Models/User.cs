﻿using MyAPI.Enums;

namespace MyAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public UserRole Role { get; set; }        
    }
}

    
