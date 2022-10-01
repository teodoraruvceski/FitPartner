using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_Web1_2022.Models
{
    public class FitnessCenter
    {
        string name;
        string address;
        int openingYear;
        string owner;
        int monthlyMembership;
        int annualMembership;
        int trainingPrice;
        int groupTrainingPrice;
        int trainingWithPersonalTrainerPrice;
        bool deleted;
        public FitnessCenter() { }
        public FitnessCenter(string name, string address, int openingYear, string owner, int monthlyMembership, int annualMembership, int trainingPrice, int groupTrainingPrice, int trainingWithPersonalTrainerPrice, bool deleted)
        {
            this.Name = name;
            this.Address = address;
            this.OpeningYear = openingYear;
            this.Owner = owner;
            this.MonthlyMembership = monthlyMembership;
            this.AnnualMembership = annualMembership;
            this.TrainingPrice = trainingPrice;
            this.GroupTrainingPrice = groupTrainingPrice;
            this.TrainingWithPersonalTrainerPrice = trainingWithPersonalTrainerPrice;
            this.Deleted = false;
        }

        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public int OpeningYear { get => openingYear; set => openingYear = value; }
        public string Owner { get => owner; set => owner = value; }
        public int MonthlyMembership { get => monthlyMembership; set => monthlyMembership = value; }
        public int AnnualMembership { get => annualMembership; set => annualMembership = value; }
        public int TrainingPrice { get => trainingPrice; set => trainingPrice = value; }
        public int GroupTrainingPrice { get => groupTrainingPrice; set => groupTrainingPrice = value; }
        public int TrainingWithPersonalTrainerPrice { get => trainingWithPersonalTrainerPrice; set => trainingWithPersonalTrainerPrice = value; }
        public bool Deleted { get => deleted; set => deleted = value; }
    }
}