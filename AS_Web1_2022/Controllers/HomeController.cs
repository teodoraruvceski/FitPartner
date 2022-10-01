using AS_Web1_2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AS_Web1_2022.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //AllData.AddAllUsers(new List<User>());
            //AllData.AddAllFC(new List<FitnessCenter>());
            //AllData.AddAllTrainings(new List<Training>());

            return View();
        }
        public ActionResult Registration()
        {
            return View();
        }
        public ActionResult RegistrationRequest(User user,string usersGender)
        {
            if(user.Name==null || user.Name.Length<5
                || user.Lastname==null || user.Lastname.Length<5
                || user.Username==null || user.Username.Length<5
                || user.Password==null || user.Password.Length<5
                || user.Email==null || !user.Email.Contains("@") || !user.Email.Contains(".") || user.Email.Length<5
                || user.Birthdate>DateTime.Now 
                || usersGender==""
                )
            {
                ViewBag.ErrorMessage = "Input invalid.";
                return View("Registration");
            }
            gen g;
            Enum.TryParse(usersGender, out g);
            user.Gender = g;
            user.HoldingTrainings = new List<string>();
            user.AttendingTrainings = new List<string>();
            user.Blocked = false;
            user.MyFitnessCenters = new List<string>();
            user.Deleted = false;
            if(!AllData.AddUser(user))
            {
                ViewBag.ErrorMessage = "User exists.";
                return View("Registration");
            }
            return RedirectToAction("Index");
        }
        public ActionResult LoginRequest(User user)
        {
            User u= AllData.FindUser(user.Username);
            if (user.Username!=null && user.Password!=null && AllData.UserExists(user) &&  !u.Blocked)
            {
                Session["user"] = u;
                return RedirectToAction("AllFitnessCenters");
            }
            ViewBag.ErrorMessage = "Invalid";
            return View("Index");
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Index");
        }
        public ActionResult AllFitnessCenters()
        {
            List<FitnessCenter> list = AllData.RetrieveAllFC();
            list.Sort(delegate (FitnessCenter a, FitnessCenter b) { return a.Name.CompareTo(b.Name); });
            return View(list);
        }
        public ActionResult SortFitnessCenters(string type, string by)
        {
            List<FitnessCenter> centers = AllData.RetrieveAllFC();
            switch(type)
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
            return View("AllFitnessCenters", centers);
        }
        public ActionResult SearchFitnessCenters(string openingYearFrom,string openingYearTo, string address, string name)
        {
            List<FitnessCenter> centers = AllData.RetrieveAllFC();
            if (address == "" && name == "" && openingYearTo == "" && openingYearFrom == "")
                return View("AllFitnessCenters", centers);
            List<FitnessCenter> centersName = new List<FitnessCenter>();
            List<FitnessCenter> centersYear = new List<FitnessCenter>();
            List<FitnessCenter> centersAddress = new List<FitnessCenter>();
            int year1=-1;
            int year2=-1;
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
            
            return View("AllFitnessCenters", centersAddress);
        }
        public ActionResult More(string fitnessCenter)
        {
            return View(AllData.FindFitnessCenter(fitnessCenter));
        }
    }
}