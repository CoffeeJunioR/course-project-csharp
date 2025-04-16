using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace StudentManagementSystem.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TeacherController> _logger;

        public TeacherController(ApplicationDbContext context, ILogger<TeacherController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Courses()
        {
            var teacherId = GetCurrentUserId();
            var courses = await _context.Courses
                .Include(c => c.Schedules)
                .Include(c => c.StudentCourses)
                .Where(c => c.TeacherId == teacherId)
                .ToListAsync();

            return View(courses);
        }

        public async Task<IActionResult> Course(int id)
        {
            var course = await _context.Courses
                .Include(c => c.StudentCourses)
                    .ThenInclude(sc => sc.Student)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        public async Task<IActionResult> ManageGrades(int courseId)
        {
            var teacherId = GetCurrentUserId();
            var course = await _context.Courses
                .Include(c => c.StudentCourses)
                    .ThenInclude(sc => sc.Student)
                .FirstOrDefaultAsync(c => c.Id == courseId && c.TeacherId == teacherId);

            if (course == null)
            {
                return NotFound();
            }

            ViewBag.CourseId = courseId;
            ViewBag.CourseName = course.CourseName;

            return View(course.StudentCourses);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateGrade(int studentId, int courseId, int grade)
        {
            var teacherId = GetCurrentUserId();

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == courseId && c.TeacherId == teacherId);

            if (course == null)
            {
                return NotFound();
            }

            var studentCourse = await _context.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

            if (studentCourse == null)
            {
                return NotFound();
            }

            if (grade < 0 || grade > 100)
            {
                TempData["Error"] = "Оценка должна быть от 0 до 100.";
                return RedirectToAction(nameof(ManageGrades), new { courseId });
            }

            studentCourse.Grade = grade;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Оценка успешно обновлена.";
            return RedirectToAction(nameof(ManageGrades), new { courseId });
        }

        public IActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(Course course, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime, string location)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    course.TeacherId = GetCurrentUserId();
                    
                    _logger.LogInformation($"Creating course: {course.CourseName}, Code: {course.CourseCode}");
                    
                    var schedule = new Schedule
                    {
                        Course = course,
                        DayOfWeek = dayOfWeek,
                        StartTime = DateTime.Today.Add(startTime),
                        EndTime = DateTime.Today.Add(endTime),
                        Location = location
                    };

                    _logger.LogInformation($"Created schedule: {schedule.DayOfWeek} at {schedule.StartTime:HH:mm}");

                    course.Schedules = new List<Schedule> { schedule };

                    _context.Add(course);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Course saved successfully with ID: {course.Id}");

                    return RedirectToAction(nameof(Courses));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating course");
                    ModelState.AddModelError("", "Error creating course: " + ex.Message);
                }
            }
            else
            {
                _logger.LogWarning("ModelState is invalid:");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogWarning(error.ErrorMessage);
                    }
                }
            }
            return View(course);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                _logger.LogError("User is not authenticated or NameIdentifier claim is missing");
                throw new InvalidOperationException("Пользователь не аутентифицирован");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                _logger.LogError($"Failed to parse user ID from claim value: {userIdClaim.Value}");
                throw new InvalidOperationException("Некорректный идентификатор пользователя");
            }

            return userId;
        }
    }
} 