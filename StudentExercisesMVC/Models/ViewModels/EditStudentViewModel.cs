using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercises.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class EditStudentViewModel
    {
        public Student student { get; set; }
        public List<SelectListItem> exercises { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> cohorts { get; set; } = new List<SelectListItem>();
        protected string _connectionString;
        protected SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public EditStudentViewModel() { }
        public EditStudentViewModel(string connectionString, int id)
        {
            _connectionString = connectionString;
            {

                using (SqlConnection conn = Connection)

                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                        SELECT
                            Id, FirstName, LastName, SlackHandle, CohortId
                        FROM Student
                        WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        SqlDataReader reader = cmd.ExecuteReader();

                       

                        if (reader.Read())
                        {
                            student = new Student
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                                CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))

                            };
                        }
                        reader.Close();

                        //ViewData["Title"] = Student.FirstName + Student.LastName;

                    }

                    cohorts = GetAllCohorts()
                .Select(cohort => new SelectListItem()
                {
                    Text = cohort.Name,
                    Value = cohort.Id.ToString()
                }).ToList();

                    cohorts.Insert(0, new SelectListItem
                    {
                        Text = "Choose a cohort",
                        Value = "0"
                    });

                    exercises = GetAllExercises().Select(exercise => new SelectListItem()
                    {
                        Text = exercise.Name,
                        Value = exercise.Id.ToString()
                    }).ToList();

                    
                }
            }
        }


        //public Student GetSingleStudent()
        //{

        //}

        protected List<Cohort> GetAllCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Cohort";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();
                    while (reader.Read())
                    {
                        cohorts.Add(new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        });
                    }
                    reader.Close();
                    return cohorts;
                }
            }
        }


        protected List<Exercise> GetAllExercises()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT e.Id, e.Name, e.Language FROM Exercise e";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Exercise> exercises = new List<Exercise>();
                    while (reader.Read())
                    {
                        exercises.Add(new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        });
                    };
                    reader.Close();
                    return exercises;
                }
            }
        }





    }





}
    



