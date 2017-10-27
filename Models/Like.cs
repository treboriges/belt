using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace belt.Models
{
    public class Like : BaseEntity
    {
        public int likeid {get;set;}
        public int userid {get;set;}
        public User user {get;set;}
        public int postid {get;set;}
        public Post post {get;set;}
    }
}
