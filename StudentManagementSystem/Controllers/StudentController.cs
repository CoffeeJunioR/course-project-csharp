using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Logging;

namespace StudentManagementSystem.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StudentController> _logger;

        public StudentController(ApplicationDbContext context, ILogger<StudentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Schedule()
        {
            try
            {
                var studentId = GetCurrentUserId();
                _logger.LogInformation($"Loading schedule for student ID: {studentId}");

                var studentCourses = await _context.StudentCourses
                    .Include(sc => sc.Course)
                        .ThenInclude(c => c.Schedules)
                    .Include(sc => sc.Course.Teacher)
                    .Where(sc => sc.StudentId == studentId)
                    .ToListAsync();

                _logger.LogInformation($"Found {studentCourses.Count} courses for student");
                return View(studentCourses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading schedule");
                TempData["Error"] = "Произошла ошибка при загрузке расписания: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Grades()
        {
            try
            {
                var studentId = GetCurrentUserId();
                var studentCourses = await _context.StudentCourses
                    .Include(sc => sc.Course)
                    .Include(sc => sc.Course.Teacher)
                    .Where(sc => sc.StudentId == studentId)
                    .ToListAsync();

                return View(studentCourses);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Произошла ошибка при загрузке оценок.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> AvailableCourses()
        {
            try
            {
                var studentId = GetCurrentUserId();

                var enrolledCourseIds = await _context.StudentCourses
                    .Where(sc => sc.StudentId == studentId)
                    .Select(sc => sc.CourseId)
                    .ToListAsync();

                var availableCourses = await _context.Courses
                    .Include(c => c.Schedules)
                    .Include(c => c.Teacher)
                    .Where(c => !enrolledCourseIds.Contains(c.Id))
                    .ToListAsync();

                return View(availableCourses);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Произошла ошибка при загрузке доступных курсов.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnrollCourse(int courseId)
        {
            try
            {
                var studentId = GetCurrentUserId();

                var course = await _context.Courses
                    .FirstOrDefaultAsync(c => c.Id == courseId);

                if (course == null)
                {
                    TempData["Error"] = "Курс не найден.";
                    return RedirectToAction(nameof(AvailableCourses));
                }

                var existingEnrollment = await _context.StudentCourses
                    .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

                if (existingEnrollment != null)
                {
                    TempData["Error"] = "Вы уже записаны на этот курс.";
                    return RedirectToAction(nameof(AvailableCourses));
                }

                var enrollment = new StudentCourse
                {
                    StudentId = studentId,
                    CourseId = courseId,
                    EnrollmentDate = DateTime.UtcNow
                };

                _context.StudentCourses.Add(enrollment);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Вы успешно записались на курс!";
                return RedirectToAction(nameof(Schedule));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Произошла ошибка при записи на курс. Пожалуйста, попробуйте позже.";
                return RedirectToAction(nameof(AvailableCourses));
            }
        }

        private int GetCurrentUserId()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current user ID");
                throw;
            }
        }
    }
} 