using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Lab7_Many_to_Many_Relationship
{
    class Program
    {
        static void List()
        {
            using (var db = new AppDbContext())
            {
                var everything = db.Courses.Include(c => c.StudentCourses).ThenInclude(sc => sc.Student);

                foreach (var course in everything)
                {
                    Console.WriteLine($"{course.CourseName}: ");
                    foreach (var student in course.StudentCourses)
                    {
                        Console.WriteLine($"\t {student.Student.FirstName} {student.Student.LastName}   GPA: {student.GPA}");
                    }
                    Console.WriteLine();
                }
            }
        }

        static void Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                List<Course> Courses = new List<Course>
                {
                    new Course {CourseName = "CIDM 2315 Object Oriented Programming"},
                    new Course {CourseName = "CIDM 3312 Advanced Business Programming"}
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
                    new StudentCourse {Student = Students[1], Course = Courses[1], GPA = 4.0m},
                    new StudentCourse {Student = Students[2], Course = Courses[1], GPA = 3.75m},
                    new StudentCourse {Student = Students[3], Course = Courses[2], GPA = 3.50m},
                    new StudentCourse {Student = Students[4], Course = Courses[2], GPA = 3.25m}
                };

                db.Add(Courses);
                db.AddRange(Students);
                db.AddRange(joinTable);
                db.SaveChanges();
            }
            List();

            using (var db = new AppDbContext())
            {
                StudentCourse remove1 = db.StudentCourses.Where(sc => sc.Student.FirstName == "Michael").First();
                db.Remove(remove1);
                db.SaveChanges();
            }

            using (var db = new AppDbContext())
            {
                Student Transfer = new Student
                {
                    FirstName = "Aretha", LastName = "Franklin"
                };
                db.Add(Transfer);
                db.SaveChanges();

                StudentCourse transferStudent = new StudentCourse
                {
                    Student = db.Students.Find(5), Course = db.Courses.Find(2), GPA = 3.00m
                };
                db.Add(transferStudent);
                db.SaveChanges();
            }
            List();
        }
    }
}
