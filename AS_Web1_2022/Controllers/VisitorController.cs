using AS_Web1_2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AS_Web1_2022.Controllers
{
    public class VisitorController : Controller
    {
        // GET: Visitor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Join(string trainingName)
        {
            User user = (User)Session["user"];
            Training t = AllData.FindTraining(trainingName);
            AllData.RemoveTraining(t);
            AllData.RemoveUser(user);
            user.AttendingTrainings.Add(trainingName);
            t.Participants.Add(user.Username);
            Session["user"] = user;
            AllData.AddUser(user);
            AllData.AddTraining(t);
            return View("~/Views/Home/More.cshtml",AllData.FindFitnessCenter(t.FitnessCentar));
        }
        public ActionResult ProfileInformation()
        {
            return View((User)Session["user"]);
        }
        public ActionResult ChangeProfileInformation(User user, string userGender)
        {
            User u = (User)Session["user"];
            if(user.Name==null || user.Lastname==null || user.Email==null || !user.Email.Contains("@"))
            {
                ViewBag.ErrorMessage = "Invalid input";
                return View("ProfileInformation", u);
            }
            AllData.RemoveUser(u);
            u.Name = user.Name;
            u.Lastname = user.Lastname;
            u.Email = user.Email;
            if (user.Birthdate != DateTime.MinValue && user.Birthdate < DateTime.Now)
                u.Birthdate = user.Birthdate;
                
            if(userGender!="")
            {
                gen g;
                Enum.TryParse(userGender, out g);
                u.Gender = g;
            }
            AllData.AddUser(u);
            Session["user"] = u;

            return RedirectToAction("AllFitnessCenters", "Home");
        }
        public ActionResult JoinedTrainings()
        {
            return View(AllData.RetrieveAllTrainings());
        }
        public ActionResult SearchTraining(string name,string type,string fitnessCenter)
        {
            List<Training> trainings = AllData.RetrieveAllTrainings();
            List<Training> trainingsName = new List<Training>();
            List<Training> trainingsType = new List<Training>();
            List<Training> trainingsFC = new List<Training>();
            if (name == "" && type== "" && fitnessCenter== "")
                return View("JoinedTrainings", trainings);
            type = type.ToUpper();
            name = name.ToUpper();
            fitnessCenter = fitnessCenter.ToUpper();
            if (name != "")
                for (int i = 0; i < trainings.Count(); i++)
                {
                    if (trainings[i].Name.ToUpper().Contains(name))
                        trainingsName.Add(trainings[i]);
                }
            else
                trainingsName = trainings;
            if (type!="")
                for (int i = 0; i < trainingsName.Count(); i++)
                {
                    if (trainingsName[i].TrainingType.ToString().ToUpper().Contains(type))
                        trainingsType.Add(trainingsName[i]);
                }
            else
                trainingsType = trainingsName;
            if (fitnessCenter != "")
                for (int i = 0; i < trainingsType.Count(); i++)
                {
                    if (trainingsType[i].FitnessCentar.ToUpper().Contains(fitnessCenter))
                        trainingsFC.Add(trainingsType[i]);
                }
            else
                trainingsFC = trainingsType;

            return View("JoinedTrainings", trainingsFC);
        }
        public ActionResult SortTraining(string by,string type)
        {
            List<Training> centers = AllData.RetrieveAllTrainings();
            switch (type)
            {
                case "descending":
                    if (by == "name")
                        centers.Sort(delegate (Training a, Training b) { return b.Name.CompareTo(a.Name); });
                    else if (by == "type")
                        centers.Sort(delegate (Training a, Training b) { return b.TrainingType.CompareTo(a.TrainingType); });
                    else if (by == "datetime")
                        centers.Sort(delegate (Training a, Training b) { return b.Datetime.CompareTo(a.Datetime); });
                    break;
                case "ascending":
                    if (by == "name")
                        centers.Sort(delegate (Training a, Training b) { return a.Name.CompareTo(b.Name); });
                    else if (by == "type")
                        centers.Sort(delegate (Training a, Training b) { return a.TrainingType.CompareTo(b.TrainingType); });
                    else if (by == "datetime")
                        centers.Sort(delegate (Training a, Training b) { return a.Datetime.CompareTo(b.Datetime); });
                    break;
            }
            return View("JoinedTrainings", centers);
        }
        public ActionResult AddComment()
        {
            return View();
        }
        public ActionResult AddCommentRequest(Comment comment)
        {
            User user = (User)Session["user"];
            comment.Username = user.Username;
            List<Comment> comments = AllData.RetrieveAllComments();
            if(comment.CommentText==null || comment.FitnessCenter==null || comment.Rating==0)
            {
                ViewBag.ErrorMessage = "Invalid input";
                return View("AddComment");
            }
            foreach(string s in user.AttendingTrainings)
            {
                Training t = AllData.FindTraining(s);
                if(t.Datetime<DateTime.Now && t!=null && t.FitnessCentar==comment.FitnessCenter)
                {
                    foreach(Comment com in comments)
                    {
                        if (comment.FitnessCenter == com.FitnessCenter && comment.Username == com.Username)
                        {
                            ViewBag.ErrorMessage = "Not able to rate this fitness center";
                            return View("AddComment");
                        }
                     }
                    comment.Username = user.Username;
                    comment.Declined = false;
                    comment.Accepted = false;
                    AllData.AddComment(comment);
                    return View("~/Views/Home/AllFitnessCenters.cshtml", AllData.RetrieveAllFC());
                }
            }
            ViewBag.ErrorMessage = "Not able to rate this fitness center";

            return View("AddComment");
        }
    }
}