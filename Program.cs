using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Lab7_Many_to_Many_Relationship
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }

            Course CIDM2315 = new Course
            {
                CourseName = "CIDM 2315 Object Oriented Programming"
            };

            Course CIDM3312 = new Course
            {
                CourseName = "CIDM 3312 Advanced Business Programming"
            };

            List<Student> Students = new List<Student>
            {
                new Student {FirstName = "Michael", LastName = "Jackson"},
                new Student {FirstName = "Whitney", LastName = "Houston"},
                new Student {FirstName = "Amy", LastName = "Whinehouse"},
                new Student {FirstName = "George", LastName = "Michael"}
            };

            List<StudentCourse> joinTable = new List<StudentCourse>()
            {
                new StudentCourse {Student = Students[1], Course = CIDM2315, GPA = 4.0m},
                new StudentCourse {Student = Students[2], Course = CIDM3312, GPA = 3.75m},
                new StudentCourse {Student = Students[3], Course = CIDM2315, GPA = 3.50m},
                new StudentCourse {Student = Students[4], Course = CIDM3312, GPA = 3.25m}
            };

            db.Add(CIDM2315);
            db.Add(CIDM3312);
            db.AddRange(Students);
            db.AddRange(joinTable);
            db.SaveChanges();
        }
    }
}
