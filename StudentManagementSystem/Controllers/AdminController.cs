using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace StudentManagementSystem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ApplicationDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Users()
        {
            try
            {
                var users = await _context.Users
                    .OrderBy(u => u.Role)
                    .ThenBy(u => u.LastName)
                    .ToListAsync();

                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке списка пользователей");
                TempData["Error"] = "Произошла ошибка при загрузке списка пользователей.";
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                    if (existingUser != null)
                    {
                        TempData["Error"] = "Пользователь с таким email уже существует.";
                        return RedirectToAction(nameof(Users));
                    }

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Пользователь успешно создан.";
                    return RedirectToAction(nameof(Users));
                }

                TempData["Error"] = "Пожалуйста, проверьте правильность заполнения всех полей.";
                return RedirectToAction(nameof(Users));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании пользователя");
                TempData["Error"] = "Произошла ошибка при создании пользователя.";
                return RedirectToAction(nameof(Users));
            }
        }

        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Users));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> ManageCourses()
        {
            try
            {
                var courses = await _context.Courses
                    .Include(c => c.Teacher)
                    .Include(c => c.Schedules)
                    .ToListAsync();
                return View(courses);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Произошла ошибка при загрузке списка курсов.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> EditCourse(int id)
        {
            try
            {
                var course = await _context.Courses
                    .Include(c => c.Teacher)
                    .Include(c => c.Schedules)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (course == null)
                {
                    TempData["Error"] = "Курс не найден.";
                    return RedirectToAction(nameof(ManageCourses));
                }

                return View(course);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Произошла ошибка при загрузке курса.";
                return RedirectToAction(nameof(ManageCourses));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                TempData["Error"] = "Некорректный идентификатор курса.";
                return RedirectToAction(nameof(ManageCourses));
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Курс успешно обновлен.";
                    return RedirectToAction(nameof(ManageCourses));
                }
                return View(course);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(course.Id))
                {
                    TempData["Error"] = "Курс не найден.";
                    return RedirectToAction(nameof(ManageCourses));
                }
                else
                {
                    TempData["Error"] = "Произошла ошибка при обновлении курса.";
                    return View(course);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Произошла ошибка при сохранении изменений.";
                return View(course);
            }
        }

        public async Task<IActionResult> Reports()
        {
            try
            {
                var viewModel = new ReportsViewModel
                {
                    TotalStudents = await _context.Users.CountAsync(u => u.Role == UserRole.Student),
                    TotalTeachers = await _context.Users.CountAsync(u => u.Role == UserRole.Teacher),
                    TotalCourses = await _context.Courses.CountAsync(),
                    CourseEnrollments = await _context.Courses
                        .Include(c => c.StudentCourses)
                        .Select(c => new CourseEnrollmentViewModel
                        {
                            CourseName = c.CourseName,
                            EnrolledStudents = c.StudentCourses.Count
                        })
                        .ToListAsync(),
                    AverageGrades = await _context.Courses
                        .Include(c => c.StudentCourses)
                        .Select(c => new CourseGradeViewModel
                        {
                            CourseName = c.CourseName,
                            AverageGrade = c.StudentCourses.Any() ? 
                                c.StudentCourses.Average(sc => sc.Grade ?? 0) : 0
                        })
                        .ToListAsync()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Произошла ошибка при формировании отчетов.";
                return RedirectToAction("Index", "Home");
            }
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(c => c.Id == id);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }

    public class ReportsViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalCourses { get; set; }
        public List<CourseEnrollmentViewModel> CourseEnrollments { get; set; }
        public List<CourseGradeViewModel> AverageGrades { get; set; }
    }

    public class CourseEnrollmentViewModel
    {
        public string CourseName { get; set; }
        public int EnrolledStudents { get; set; }
    }

    public class CourseGradeViewModel
    {
        public string CourseName { get; set; }
        public double AverageGrade { get; set; }
    }
} 