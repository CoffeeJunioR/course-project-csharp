using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string CourseName { get; set; }

        [Required]
        [StringLength(20)]
        public string CourseCode { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(1, 10)]
        public int Credits { get; set; }

        [Required]
        public int TeacherId { get; set; }
        public User Teacher { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }

    public class Schedule
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }

    public class StudentCourse
    {
        public int StudentId { get; set; }
        public User Student { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int? Grade { get; set; }
        public DateTime? EnrollmentDate { get; set; }
    }
} 