using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Lab7_Many_to_Many_Relationship
{
    class Program
    {
    // 4. List out each course, the students enrolled in that course, and their GPA.
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
                        Console.WriteLine($"\t {student.Student.FirstName} {student.Student.LastName} - GPA: {student.GPA}");
                    }
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

// 1. Create 2 courses and add them to the database.
                List<Course> Courses = new List<Course>
                {
                    new Course {CourseName = "CIDM 2315 Object Oriented Programming"},
                    new Course {CourseName = "CIDM 3312 Advanced Business Programming"}
                };
                db.AddRange(Courses);
                db.SaveChanges();

// 2. Create 4 students and add them to the database.
                List<Student> Students = new List<Student>
                {
                    new Student {FirstName = "Michael", LastName = "Jackson"},
                    new Student {FirstName = "Whitney", LastName = "Houston"},
                    new Student {FirstName = "Amy", LastName = "Whinehouse"},
                    new Student {FirstName = "George", LastName = "Michael"}
                };
                db.AddRange(Students);
                db.SaveChanges();

// 3. Create a list of enrollments where each student is enrolled in at least one course and each course has at least a few students. Add this list to the database as your association table.
                List<StudentCourse> joinTable = new List<StudentCourse>()
                {
                    new StudentCourse {Student = Students[0], Course = Courses[0], GPA = 4.0m},
                    new StudentCourse {Student = Students[1], Course = Courses[0], GPA = 3.75m},
                    new StudentCourse {Student = Students[2], Course = Courses[1], GPA = 3.50m},
                    new StudentCourse {Student = Students[3], Course = Courses[1], GPA = 3.25m}
                };
                db.AddRange(joinTable);
                db.SaveChanges();
            }
// 4. List out each course, the students enrolled in that course, and their GPA.
            List();

// 5. Remove a student from one of the courses.
            using (var db = new AppDbContext())
            {
                StudentCourse remove1 = db.StudentCourses.Where(sc => sc.Student.FirstName == "Michael").First();
                db.Remove(remove1);
                db.SaveChanges();
            }

// 6. Add a new transfer student and enroll that student in one course.
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
                    Student = db.Students.Find(5), Course = db.Courses.Find(1), GPA = 3.00m
                };
                db.Add(transferStudent);
                db.SaveChanges();
            }
// 7. List your data again.
            List();
        }
    }
}
