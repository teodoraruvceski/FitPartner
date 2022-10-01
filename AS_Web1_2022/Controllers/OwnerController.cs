using AS_Web1_2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AS_Web1_2022.Controllers
{
    public class OwnerController : Controller
    {
        // GET: Owner
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MyFitnessCenters()
        {
            return View(OwnersFitnessCenters());
        }
        public ActionResult MyWorkers()
        {
            User owner = (User)Session["user"];
            List<User> users = AllData.RetrieveAllUsers();
            List<User> myWorkers = new List<User>();
            foreach(User u in users)
            {
                if (u.Role == role.trainer && owner.MyFitnessCenters.Contains(u.Workplace))
                    myWorkers.Add(u);
            }
            return View(myWorkers);
        }
        public ActionResult BlockTrainer(string username)
        {
            User user = AllData.FindUser(username);
            AllData.RemoveUser(user);
            user.Blocked = true;
            AllData.AddUser(user);
            return RedirectToAction("MyWorkers");
        }
        public ActionResult AddTrainer()
        {
            return View();
        }
        public ActionResult AddTrainerRequest(User user, string usersGender)
        {
            User owner = (User)Session["user"];
            if (user.Name == null || user.Name.Length < 5
                || user.Lastname == null || user.Lastname.Length < 5
                || user.Username == null || user.Username.Length < 5
                || user.Password == null || user.Password.Length < 5
                || user.Email == null || !user.Email.Contains("@") || !user.Email.Contains(".") || user.Email.Length < 5
                || user.Birthdate > DateTime.Now
                || usersGender == "" || user.Workplace==null || !owner.MyFitnessCenters.Contains(user.Workplace)
                )
            {
                ViewBag.ErrorMessage = "Input invalid.";
                return View("AddTrainer");
            }
            gen g;
            Enum.TryParse(usersGender, out g);
            user.Gender = g;
            user.HoldingTrainings = new List<string>();
            user.AttendingTrainings = new List<string>();
            user.Blocked = false;
            user.MyFitnessCenters = new List<string>();
            user.Deleted = false;
            user.Role = role.trainer;
            if (!AllData.AddUser(user))
            {
                ViewBag.ErrorMessage = "User exists.";
                return View("AddTrainer");
            }
            return RedirectToAction("MyWorkers");
        }
        public ActionResult DeleteFitnessCenter(string name)
        {
            List<Training> trainings = AllData.RetrieveAllTrainings();
            FitnessCenter fc = AllData.FindFitnessCenter(name);
            foreach (Training t in trainings)
                if (t.FitnessCentar == name && t.Datetime > DateTime.Now)
                {
                    ViewBag.ErrorMessage = "Error! There are trainings in the future.";
                    return View("MyFitnessCenters", OwnersFitnessCenters());
                }
            AllData.RemoveFC(fc);
            fc.Deleted = true;
            AllData.AddFC(fc);
            List<User> workers = AllData.RetrieveAllUsers();
            foreach (User u in workers)
                if (u.Role==role.trainer && u.Workplace == name)
                    u.Blocked = true;
            AllData.AddAllUsers(workers);
            return RedirectToAction("MyFitnessCenters");
        }
        public ActionResult ModifyTrainer(string username)
        {
            return View(AllData.FindUser(username));
        }
        public ActionResult ModifyTrainerRequest(User user,string usersGender)
        {
            User owner = (User)Session["user"];
            User trainer = AllData.FindUser(user.Username);
            AllData.RemoveUser(trainer);
            if (user.Name == null || user.Name.Length < 5
                || user.Lastname == null || user.Lastname.Length < 5
                || user.Username == null || user.Username.Length < 5
                || user.Password == null || user.Password.Length < 5
                || user.Email == null || !user.Email.Contains("@") || !user.Email.Contains(".") || user.Email.Length < 5
                
                || usersGender == "" || user.Workplace == null || !owner.MyFitnessCenters.Contains(user.Workplace)
                )
            {
                ViewBag.ErrorMessage = "Input invalid.";
                return View("ModifyTrainer",trainer);
            }
            gen g;
            Enum.TryParse(usersGender, out g);
            trainer.Gender = g;
            trainer.Name = user.Name;
            trainer.Lastname = user.Lastname;
            trainer.Workplace = user.Workplace;
            trainer.Email = user.Email;
            trainer.Birthdate = user.Birthdate;
            AllData.AddUser(trainer);
            return RedirectToAction("MyWorkers");
        }
        public ActionResult AddFitnessCenter()
        {
            return View();
        }
        public ActionResult AddFitnessCenterRequest(FitnessCenter fc)
        {
            User owner = (User)Session["user"];
            
            if(fc.Name!=null && fc.Address!=null
                && fc.OpeningYear!=0 && fc.MonthlyMembership!=0
                && fc.AnnualMembership!=0 && fc.TrainingPrice!=0 && fc.TrainingWithPersonalTrainerPrice!=0
                && fc.GroupTrainingPrice!=0 )
            {
                AllData.RemoveUser(owner);
                owner.MyFitnessCenters.Add(fc.Name);
                AllData.AddUser(owner);
                fc.Deleted = false;
                fc.Owner = owner.Username;
                AllData.AddFC(fc);
                return RedirectToAction("MyFitnessCenters");
            }
            ViewBag.ErrorMessage = "Invalid input";
            return View("AddFitnessCenter");
        }
        public ActionResult ModifyFitnessCenter()
        {
            return View();
        }
        public ActionResult ModifyFitnessCenterRequest(FitnessCenter fc)
        {
            User owner = (User)Session["user"];
            FitnessCenter fitnessCenter=AllData.FindFitnessCenter(fc.Name);
            if (fc.Address != null
                && fc.OpeningYear != 0 && fc.MonthlyMembership != 0
                && fc.AnnualMembership != 0 && fc.TrainingPrice != 0 && fc.TrainingWithPersonalTrainerPrice != 0
                && fc.GroupTrainingPrice != 0)
            {
                AllData.RemoveFC(fitnessCenter);
                fc.Deleted = false;
                fc.Owner = owner.Username;
                AllData.AddFC(fc);
                return RedirectToAction("MyFitnessCenters");
            }
            ViewBag.ErrorMessage = "Invalid input";
            return View("AddFitnessCenter");
        }
        public ActionResult MyComments()
        {
            User owner = (User)Session["user"];
            List<Comment> allComments = AllData.RetrieveAllComments();
            List<Comment> myComments = new List<Comment>();
            for (int i = 0; i < allComments.Count; i++)
                if (owner.MyFitnessCenters.Contains(allComments[i].FitnessCenter))
                    myComments.Add(allComments[i]);
            return View(myComments);
        }
        public ActionResult Accept(string c)
        {
            foreach(Comment com in AllData.RetrieveAllComments())
                if((com.Username+com.FitnessCenter)==c)
                {
                    AllData.RemoveComment(com);
                    com.Accepted = true;
                    com.Declined = false;
                    AllData.AddComment(com);
                    return RedirectToAction("MyComments");
                }
            return RedirectToAction("MyComments");
        }
        public ActionResult Decline(string c)
        {
            foreach (Comment com in AllData.RetrieveAllComments())
                if ((com.Username + com.FitnessCenter) == c)
                {
                    AllData.RemoveComment(com);
                    com.Accepted = false;
                    com.Declined = true;
                    AllData.AddComment(com);
                    return RedirectToAction("MyComments");
                }
            return RedirectToAction("MyComments");
        }
        public ActionResult SortFitnessCenters(string type, string by)
        {
            List<FitnessCenter> centers = OwnersFitnessCenters();
            switch (type)
            {
                case "descending":
                    if (by == "name")
                        centers.Sort(delegate (FitnessCenter a, FitnessCenter b) { return b.Name.CompareTo(a.Name); });
                    else if (by == "openingYear")
                        centers.Sort(delegate (FitnessCenter a, FitnessCenter b) { return b.OpeningYear.CompareTo(a.OpeningYear); });
                    else if (by == "address")
                        centers.Sort(delegate (FitnessCenter a, FitnessCenter b) { return b.Address.CompareTo(a.Address); });
                    break;
                case "ascending":
                    if (by == "name")
                        centers.Sort(delegate (FitnessCenter a, FitnessCenter b) { return a.Name.CompareTo(b.Name); });
                    else if (by == "openingYear")
                        centers.Sort(delegate (FitnessCenter a, FitnessCenter b) { return a.OpeningYear.CompareTo(b.OpeningYear); });
                    else if (by == "address")
                        centers.Sort(delegate (FitnessCenter a, FitnessCenter b) { return a.Address.CompareTo(b.Address); });
                    break;
            }
            return View("MyFitnessCenters", centers);
        }
        public ActionResult SearchFitnessCenters(string openingYearFrom, string openingYearTo, string address, string name)
        {
            List<FitnessCenter> centers = OwnersFitnessCenters();
            if (address == "" && name == "" && openingYearTo == "" && openingYearFrom == "")
                return View("AllFitnessCenters", centers);
            List<FitnessCenter> centersName = new List<FitnessCenter>();
            List<FitnessCenter> centersYear = new List<FitnessCenter>();
            List<FitnessCenter> centersAddress = new List<FitnessCenter>();
            int year1 = -1;
            int year2 = -1;
            int.TryParse(openingYearFrom, out year1);
            int.TryParse(openingYearTo, out year2);
            address = address.ToUpper();
            name = name.ToUpper();
            if (name != "")
                for (int i = 0; i < centers.Count(); i++)
                {
                    if (centers[i].Name.ToUpper().Contains(name))
                        centersName.Add(centers[i]);
                }
            else
                centersName = centers;
            if (year1 != 0 && year2 != 0)
                for (int i = 0; i < centersName.Count(); i++)
                {
                    if (centersName[i].OpeningYear >= year1 && centersName[i].OpeningYear <= year2)
                        centersYear.Add(centersName[i]);
                }
            else
                centersYear = centersName;
            if (address != "")
                for (int i = 0; i < centersYear.Count(); i++)
                {
                    if (centersYear[i].Address.ToUpper().Contains(address))
                        centersAddress.Add(centersYear[i]);
                }
            else
                centersAddress = centersYear;

            return View("MyFitnessCenters", centersAddress);
        }
        private List<FitnessCenter> OwnersFitnessCenters()
        {
            List<FitnessCenter> fitnessCenters = new List<FitnessCenter>();
            foreach (FitnessCenter fc in AllData.RetrieveAllFC())
                if (((User)Session["user"]).MyFitnessCenters.Contains(fc.Name))
                    fitnessCenters.Add(fc);
            return fitnessCenters;
        }
    }
}