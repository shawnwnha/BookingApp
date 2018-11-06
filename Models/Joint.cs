using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace bookingApp.Models{
    public class Joint{
        [Key]
        public int JointId {get;set;}
        public int UserId {get;set;}
        public User User {get;set;}
        public int EventId {get;set;}
        public Event Event {get;set;}
    } 
}