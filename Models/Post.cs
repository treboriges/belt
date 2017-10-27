using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace belt.Models
{
    public class Post : BaseEntity
    {
        public int postid {get;set;}
        [Required]
        public string comment {get;set;}
        public int userid {get;set;}
        public User user {get;set;}
        public List<Like> likedby {get;set;}
        public Post(){
            likedby = new List<Like>();
        }
    }
}
