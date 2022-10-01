using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace AS_Web1_2022.Models
{
    public class AllData
    {
        static string fitnessCentersLocation = "C:/Users/elitebook/Desktop/New folder/AS_Web1_2022/fitnessCenterDB.txt";
        static string trainingsLocation = "C:/Users/elitebook/Desktop/New folder/AS_Web1_2022/trainingDB.txt";
        static string commentsLocation = "C:/Users/elitebook/Desktop/New folder/AS_Web1_2022/commentDB.txt";
        static string usersLocation = "C:/Users/elitebook/Desktop/New folder/AS_Web1_2022/userDB.txt";
        #region users
        public static List<User> RetrieveAllUsers()
        {
            XmlSerializer xmlSerializ = new XmlSerializer(typeof(List<User>));
            using (var str = new StreamReader(usersLocation))
            {
                return (List<User>)xmlSerializ.Deserialize(str);
            }
        }
        public static void AddAllUsers(List<User> list)
        {
            User bob = new User("bob","bob","bob","bob","bob",DateTime.MaxValue,gen.male,role.visitor,false,"");
            User sam = new User("sam", "sam", "sam", "sam", "sam", DateTime.MaxValue, gen.female, role.owner, false, "fitStar");


            //list.Add(bob);
            //list.Add(sam);

            XmlSerializer xmlSerializ = new XmlSerializer(typeof(List<User>));
            using (var str = File.Create(usersLocation))
            {
                xmlSerializ.Serialize(str, list);
            }
        }
        public static User FindUser(string user)
        {
            foreach (User u in RetrieveAllUsers())
                if (u.Username == user)
                    return u;
            return null;
        }
        public static bool UserExists(User user)
        {
            foreach (User u in RetrieveAllUsers())
                if (u.Username == user.Username && u.Password == user.Password)
                    return true;
            return false;
        }
        public static bool AddUser(User user)
        {
            List<User> users = RetrieveAllUsers();
            foreach(User u in users)
                if(u.Username==user.Username)
                {
                    return false;
                }
            
            users.Add(user);
            AddAllUsers(users);
            return true;
        }
        public static bool RemoveUser(User user)
        {
            List<User> users = RetrieveAllUsers();
            foreach (User u in users)
                if (u.Username == user.Username)
                {
                    users.Remove(u);
                    AddAllUsers(users);
                    return true;
                }
            return false;
        }
        public static bool ChangeUser(User user)
        {
            List<User> users = RetrieveAllUsers();
            foreach(User u in users)
                if(u.Username==user.Username)
                {
                    RemoveUser(u);
                    AddUser(user);
                    return true;
                }
                return false;
        }
        #endregion
        #region comments

        public static List<Comment> RetrieveAllComments()
        {
            XmlSerializer xmlSerializ = new XmlSerializer(typeof(List<Comment>));
            using (var str = new StreamReader(commentsLocation))
            {
                return (List<Comment>)xmlSerializ.Deserialize(str);
            }
        }
        public static void AddAllComments(List<Comment> comments)
        {
            XmlSerializer xmlSerializ = new XmlSerializer(typeof(List<Comment>));
            using (var str = File.Create(commentsLocation))
            {
                xmlSerializ.Serialize(str, comments);
            }
        }
        public static bool AddComment(Comment comment)
        {
            List<Comment> comments = RetrieveAllComments();

            comments.Add(comment);
            AddAllComments(comments);
            return true;
        }
        public static bool RemoveComment(Comment comment)
        {
            List<Comment> comments = RetrieveAllComments();
            foreach (Comment c in comments)
                if (c.FitnessCenter == comment.FitnessCenter && c.Username==comment.Username && c.CommentText==comment.CommentText)
                {
                    comments.Remove(c);
                    AddAllComments(comments);
                    return true;
                }
            return false;
        }
        public static bool ChangeComment(Comment comment)
        {
            List<Comment> comments = RetrieveAllComments();
            foreach (Comment c in comments)
                if (c.FitnessCenter == comment.FitnessCenter && c.Username == comment.FitnessCenter && c.CommentText == comment.CommentText)
                {
                    RemoveComment(c);
                    AddComment(comment);
                    return true;
                }
            return false;
        }
        #endregion
        #region trainings
        public static List<Training> RetrieveAllTrainings()
        {
            XmlSerializer xmlSerializ = new XmlSerializer(typeof(List<Training>));
            using (var str = new StreamReader(trainingsLocation))
            {
                return (List<Training>)xmlSerializ.Deserialize(str);
            }
        }
        public static void AddAllTrainings(List<Training> trainings)
        {
            Training box1 = new Training("Box1", Type.Box, "fitStar", 70, 10, DateTime.MaxValue, false);


            //trainings.Add(box1);

            XmlSerializer xmlSerializ = new XmlSerializer(typeof(List<Training>));
            using (var str = File.Create(trainingsLocation))
            {
                xmlSerializ.Serialize(str, trainings);
            }
        }
        public static Training FindTraining(string training)
        {
            foreach (Training t in RetrieveAllTrainings())
                if (t.Name == training)
                    return t;
            return null;
        }
        public static bool AddTraining(Training training)
        {
            List<Training> trainings = RetrieveAllTrainings();
            foreach (Training t in trainings)
                if (t.Name == training.Name)
                {
                    return false;
                }

            trainings.Add(training);
            AddAllTrainings(trainings);
            return true;
        }
        public static bool RemoveTraining(Training training)
        {
            List<Training> trainings = RetrieveAllTrainings();
            foreach (Training u in trainings)
                if (u.Name == training.Name)
                {
                    trainings.Remove(u);
                    AddAllTrainings(trainings);
                    return true;
                }
            return false;
        }
        public static bool ChangeTraining(Training training)
        {
            List<Training> trainings = RetrieveAllTrainings();
            foreach (Training u in trainings)
                if (u.Name == training.Name)
                {
                    RemoveTraining(u);
                    AddTraining(training);
                    return true;
                }
            return false;
        }
        #endregion 
        #region fitnessCenters
        public static List<FitnessCenter> RetrieveAllFC()
        {
            XmlSerializer xmlSerializ = new XmlSerializer(typeof(List<FitnessCenter>));
            using (var str = new StreamReader(fitnessCentersLocation))
            {
                return (List<FitnessCenter>)xmlSerializ.Deserialize(str);
            }
        }
        public static void AddAllFC(List<FitnessCenter> fcs)
        {
            FitnessCenter fitStar = new FitnessCenter("fitStar", "addr", 2002,"sam", 50, 500, 10, 9, 15, false);

            //fcs.Add(fitStar);

            XmlSerializer xmlSerializ = new XmlSerializer(typeof(List<FitnessCenter>));
            using (var str = File.Create(fitnessCentersLocation))
            {
                xmlSerializ.Serialize(str, fcs);
            }
        }
        public static FitnessCenter FindFitnessCenter(string fitnessCenter)
        {
            foreach (FitnessCenter f in RetrieveAllFC())
                if (f.Name== fitnessCenter)
                    return f;
            return null;
        }
        public static bool AddFC(FitnessCenter fc)
        {
            List<FitnessCenter> fcs = RetrieveAllFC();
            foreach (FitnessCenter fcc in fcs)
                if (fc.Name == fcc.Name)
                {
                    return false;
                }

            fcs.Add(fc);
            AddAllFC(fcs);
            return true;
        }
        public static bool RemoveFC(FitnessCenter fc)
        {
            List<FitnessCenter> fcs = RetrieveAllFC();
            foreach (FitnessCenter fcc in fcs)
                if (fc.Name == fcc.Name)
                {
                    fcs.Remove(fcc);
                    AddAllFC(fcs);
                    return true;
                }
            return false;
        }
        public static bool ChangeFC(FitnessCenter fc)
        {
            List<FitnessCenter> fcs = RetrieveAllFC();
            foreach (FitnessCenter fcc in fcs)
                if (fc.Name == fcc.Name)
                {
                    RemoveFC(fcc);
                    AddFC(fc);
                    return true;
                }
            return false;
        }
        #endregion
    }
}