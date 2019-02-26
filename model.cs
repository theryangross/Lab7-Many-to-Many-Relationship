using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lab7_Many_to_Many_Relationship
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(e => new {e.CourseId, e.StudentId});
        }
        public DbSet<Course> Courses {get; set;}
        public DbSet<Student> Students {get; set;}
        public DbSet<StudentCourse> StudentCourses {get; set;}
    }

    public class Course
    {
        public int CourseId {get; set;} //PK
        public string CourseName {get; set;}
        public List<StudentCourse> StudentCourses {get; set;} // Nav Property
    }

    public class Student
    {
        public int StudentId {get; set;} //PK
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public List<StudentCourse> StudentCourses {get; set;} // Nav Property
    }

    public class StudentCourse
    {
        public int CourseId {get; set;} //Composit PK, FK 1
        public int StudentId {get; set;} //Composite PK, FK 2
        public Course Course {get; set;} // Course Nav property
        public Student Student {get; set;} // Student Nav roperty
        public decimal GPA {get; set;}
    }
}