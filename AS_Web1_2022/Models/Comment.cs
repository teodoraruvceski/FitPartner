using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_Web1_2022.Models
{
    public class Comment
    {
        string commentText;
        int rating;
        bool declined;
        bool accepted;
        string username;
        string fitnessCenter;

        public Comment(string commentText, int rating, bool declined, bool accepted, string username, string fitnessCenter)
        {
            this.CommentText = commentText;
            this.Rating = rating;
            this.Declined = declined;
            this.Accepted = accepted;
            this.Username = username;
            this.FitnessCenter = fitnessCenter;
        }
        public Comment() { }

        public string CommentText { get => commentText; set => commentText = value; }
        public int Rating { get => rating; set => rating = value; }
        public bool Declined { get => declined; set => declined = value; }
        public bool Accepted { get => accepted; set => accepted = value; }
        public string Username { get => username; set => username = value; }
        public string FitnessCenter { get => fitnessCenter; set => fitnessCenter = value; }
    }
}