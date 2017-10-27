using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace belt.Models
{
    public abstract class BaseEntity
    {
    }
    public class User
    {
        public int userid { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string alias { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "password needs to be at least 8 characters")]
        public string password { get; set; }
        [Required]
        [NotMapped]
        [Compare("password",ErrorMessage = "Passwords do not match.")]
        public string c_password {get;set;}
        public List<Post> createdpost {get;set;}
        public List<Like> likedpost {get;set;}

        public User()
        {
            createdpost = new List<Post>();
            likedpost = new List<Like>();
        }
    }
}