using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_Web1_2022.Models
{
    public enum role { visitor, trainer, owner }
    public enum gen { female,male }
    public class User
    {
        string username;
        string password;
        string name;
        string lastname;
        string email;
        DateTime birthdate;
        gen gender;
        role role;
        bool deleted;
        bool blocked;
        string workplace;
        List<string> attendingTrainings;
        List<string> holdingTrainings;
        List<string> myFitnessCenters;
        

        public User(string username, string password, string name, string lastname, string email, DateTime birthdate, gen gender, role role, bool deleted, string workplace)
        {
            this.Username = username;
            this.Password = password;
            this.Name = name;
            this.Lastname = lastname;
            this.Email = email;
            this.Birthdate = birthdate;
            this.Gender = gender;
            this.Role = role;
            this.Deleted = false;
            blocked = false;
            this.Workplace = workplace;
            AttendingTrainings=new List<string>();
            HoldingTrainings= new List<string>();
            MyFitnessCenters= new List<string>();

        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Name { get => name; set => name = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public string Email { get => email; set => email = value; }
        public DateTime Birthdate { get => birthdate; set => birthdate = value; }
        public gen Gender { get => gender; set => gender = value; }
        public role Role { get => role; set => role = value; }
        public bool Deleted { get => deleted; set => deleted = value; }
        public string Workplace { get => workplace; set => workplace = value; }
        public List<string> AttendingTrainings { get => attendingTrainings; set => attendingTrainings = value; }
        public List<string> HoldingTrainings { get => holdingTrainings; set => holdingTrainings = value; }
        public List<string> MyFitnessCenters { get => myFitnessCenters; set => myFitnessCenters = value; }
        public bool Blocked { get => blocked; set => blocked = value; }
        public User() { }
    }
}