using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace bookingApp.Models{    
    public class EventValidator{
        [Key]
        public int EventId {get;set;}
        [Required(ErrorMessage = "Title cannot be empty.")]
        [MinLength(2,ErrorMessage="Must be more than 2 characters.")]
        public string Title {get;set;}
        [Required(ErrorMessage = "Description cannot be empty.")]
        [MinLength(10,ErrorMessage="Must be more than 10 characters.")]
        public string Description {get;set;}
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date cannot be empty.")]
        public DateTime Date {get;set;}
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Time cannot be empty.")]
        public TimeSpan Time {get;set;}
        [Required(ErrorMessage = "Duration cannot be empty.")]
        public string Duration {get;set;}
        public string Duration2 {get;set;}
    }
    public class Event{
        [Key]
        public int EventId {get;set;}
        [Required(ErrorMessage = "Title cannot be empty.")]
        [MinLength(2,ErrorMessage="Must be more than 2 characters.")]
        public string Title {get;set;}
        [Required(ErrorMessage = "Description cannot be empty.")]
        [MinLength(10,ErrorMessage="Must be more than 10 characters.")]
        public string Description {get;set;}
        [Required(ErrorMessage = "Date Time cannot be empty.")]
        public DateTime Datetime {get;set;}
        [Required(ErrorMessage = "Duration cannot be empty.")]
        public string Duration {get;set;}
        public int UserId {get;set;}
        public User Eventer {get;set;}
        public List<Joint> Attenders {get;set;}
        public Event(){
            Attenders = new List<Joint>();
        }
    }
}