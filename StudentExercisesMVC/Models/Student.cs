using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentExercises.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name="Slack Handle")]
        public string SlackHandle { get; set; }
        public Cohort Cohort { get; set; }
        public int CohortId { get; set; }
        public List<Exercise> CurrentExercises = new List<Exercise>();



    }




}