using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_Web1_2022.Models
{
    public enum Type { Box, Judo, BodyBuilding, CardioKickBox, WomenBodyBuilding, MMA}
    public class Training
    {
        string name;
        Type trainingType;
        string fitnessCentar;
        int duration;
        int maxParticipants;
        DateTime datetime;
        bool deleted;
        List<string> participants;
        public Training() { }
        public Training(string name, Type type, string fitnessCentar, int duration, int maxParticipants, DateTime datetime, bool deleted)
        {
            this.Name = name;
            this.TrainingType = type;
            this.FitnessCentar = fitnessCentar;
            this.Duration = duration;
            this.MaxParticipants = maxParticipants;
            this.Datetime = datetime;
            this.Deleted = false;
            Participants = new List<string>();
        }

        public string Name { get => name; set => name = value; }
        public string FitnessCentar { get => fitnessCentar; set => fitnessCentar = value; }
        public int Duration { get => duration; set => duration = value; }
        public int MaxParticipants { get => maxParticipants; set => maxParticipants = value; }
        public DateTime Datetime { get => datetime; set => datetime = value; }
        public bool Deleted { get => deleted; set => deleted = value; }
        public List<string> Participants { get => participants; set => participants = value; }
        public Type TrainingType { get => trainingType; set => trainingType = value; }
    }
}