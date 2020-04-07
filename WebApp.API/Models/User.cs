using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebApp.API.Models
{
    public class User : IdentityUser<int>
    {
        // public int Id { get; set; }
        // public string Username { get; set; }   
        // public byte[] PasswordHash { get; set; }
        // public byte[] PasswordSalt { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        // public string Email { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public ICollection<Ad> Ads { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}