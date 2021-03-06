﻿using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercises.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class CreateStudentViewModel
    {
        public List<SelectListItem> Cohorts { get; set; }

        public Student student { get; set; }

        protected string _connectionString;

        protected SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public CreateStudentViewModel() { }

        public CreateStudentViewModel(string connectionString)
        {
            _connectionString = connectionString;

            Cohorts = GetAllCohorts()
                .Select(cohort => new SelectListItem()
                {
                    Text = cohort.Name,
                    Value = cohort.Id.ToString()

                })
                .ToList();

            Cohorts.Insert(0, new SelectListItem
            {
                Text = "Choose a cohort",
                Value = "0"
            });

        }

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
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        });
                    }

                    reader.Close();

                    return cohorts;
                }
            }
        }
    }
}
    