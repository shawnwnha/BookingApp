using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace bookingApp.Models{
    public class RegiValidator
    {
        [Key]
        public int UserId {get;set;}
        [Required(ErrorMessage = "First Name cannot be empty.")]
        [MinLength(2,ErrorMessage="First name must be more than 2 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "First name must be characters only.")]
        public string Firstname { get;set; }
        [Required(ErrorMessage = "Last Name cannot be empty.")]
        [MinLength(2,ErrorMessage="Last name must be more than 2 characters.")] 
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Last name must be characters only.")]
        public string Lastname  { get;set; }
        [Required(ErrorMessage = "Email Address cannot be empty.")]
        [EmailAddress]
        public string Email { get;set; }
        [Required(ErrorMessage = "Password cannot be empty.")]
        [MinLength(8,ErrorMessage="Password must be more than 8 characters")]
        public string Password { get;set; }
        [Required]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string Confirm { get;set; }
    }
    public class User
    {
        [Key]
        public int UserId {get;set;}
        public string Firstname { get;set; }
        public string Lastname  { get;set; }
        public string Email { get;set; }
        public string Password { get;set; }
        public List<Event> Events {get;set;}
        public List<Joint> Attendings {get;set;}
        public User(){
            Events = new List<Event>();
            Attendings = new List<Joint>();
        }
    }
    public class LoginValidator
    {
        [Required(ErrorMessage="ID REQUIRED.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage="Wrong Email.")] 
        public string LoginId {get;set;}

        [Required(ErrorMessage="Password REQUIRED.")]
        [DataType(DataType.Password)]
        public string LoginPw {get;set;}
    }
}