using AS_Web1_2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Type = AS_Web1_2022.Models.Type;

namespace AS_Web1_2022.Controllers
{
    public class TrainerController : Controller
    {
        // GET: Trainer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyTrainings()
        {
            return View(RetrieveTrainingsByTrainer());
        }
        public ActionResult AddTraining()
        {
            return View();
        }
        public ActionResult AddTrainingRequest(Training training,string typeOfTrainingSession)
        {
            Type t;
            if(training.Name!="" && training.Duration!=0 && training.MaxParticipants!=0 && training.Datetime!=DateTime.MinValue && typeOfTrainingSession!="" && Enum.TryParse(typeOfTrainingSession, out t) && training.Datetime>DateTime.Now)
            {
                User trainer = ((User)Session["user"]);
                training.TrainingType = t;
                training.Deleted = false;
                training.FitnessCentar =trainer.Workplace;
                training.Participants = new List<string>();
                AllData.AddTraining(training);
                AllData.RemoveUser(trainer);
                trainer.HoldingTrainings.Add(training.Name);
                AllData.AddUser(trainer);
                return RedirectToAction("MyTrainings");


            }
            ViewBag.ErrorMessage = "Invalid input.";
            return RedirectToAction("AddTraining");
        }
        public ActionResult ModifyTraining(string name)
        {
            return View(AllData.FindTraining(name));
        }
        public ActionResult ModifyTrainingRequest(Training training, string typeOfTrainingSession)
        {
            Training tr = AllData.FindTraining(training.Name);
            Type t;
            if (typeOfTrainingSession != "" && Enum.TryParse(typeOfTrainingSession, out t))
                tr.TrainingType = t;
            if (training.Duration != 0 && training.MaxParticipants != 0 )
            {
                AllData.RemoveTraining(tr);
                tr.Duration = training.Duration;
                if(training.Datetime!=DateTime.MinValue)
                    tr.Datetime = training.Datetime;
                tr.MaxParticipants = training.MaxParticipants;
                AllData.AddTraining(tr);
                return RedirectToAction("MyTrainings");
            }
            ViewBag.ErrorMessage = "Invalid input.";
            return View("ModifyTraining",tr);
        }
        public ActionResult DeleteTraining(string name)
        {
            Training training = AllData.FindTraining(name);
            AllData.RemoveTraining(training);
            training.Deleted = true;
            AllData.AddTraining(training);
            return RedirectToAction("MyTrainings");
        }
        public ActionResult SearchTraining(string name, string type, string fitnessCenter)
        {
            List<Training> trainings = RetrieveTrainingsByTrainer();
            List<Training> trainingsName = new List<Training>();
            List<Training> trainingsType = new List<Training>();
            List<Training> trainingsFC = new List<Training>();
            if (name == "" && type == "" && fitnessCenter == "")
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
            if (type != "")
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

            return View("MyTrainings", trainingsFC);
        }
        public ActionResult SortTraining(string by, string type)
        {
            List<Training> centers = RetrieveTrainingsByTrainer();
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
            return View("MyTrainings", centers);
        }
        public ActionResult Participants(string name)
        {
            List<User> participants = new List<User>();
            List<User> allUsers = AllData.RetrieveAllUsers();
            Training training = AllData.FindTraining(name);
            for(int i=0;i< allUsers.Count();i++)
            {
                if (allUsers[i].AttendingTrainings.Contains(training.Name))
                    participants.Add(allUsers[i]);
            }
            return View(participants);
        }
        private List<Training> RetrieveTrainingsByTrainer()
        {
            List<Training> trainings = new List<Training>();
            User trainer = (User)Session["user"];
            foreach(Training t in AllData.RetrieveAllTrainings())
            {
                if (trainer.HoldingTrainings.Contains(t.Name))
                    trainings.Add(t);
            }
            return trainings;

        }
    }
}