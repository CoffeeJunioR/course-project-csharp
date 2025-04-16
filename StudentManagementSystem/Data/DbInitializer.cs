using StudentManagementSystem.Models;
using System;
using System.Linq;

namespace StudentManagementSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Проверяем, есть ли уже пользователи
            if (context.Users.Any())
            {
                return; // База данных уже заполнена
            }

            // Создаем преподавателей
            var teachers = new User[]
            {
                new User { 
                    FirstName = "Иван", 
                    LastName = "Петров", 
                    Email = "petrov@edu.com", 
                    Password = "teacher123",
                    Role = UserRole.Teacher,
                    Faculty = "Факультет информационных технологий"
                },
                new User { 
                    FirstName = "Елена", 
                    LastName = "Смирнова", 
                    Email = "smirnova@edu.com", 
                    Password = "teacher123",
                    Role = UserRole.Teacher,
                    Faculty = "Факультет информационных технологий"
                },
                new User { 
                    FirstName = "Александр", 
                    LastName = "Иванов", 
                    Email = "ivanov@edu.com", 
                    Password = "teacher123",
                    Role = UserRole.Teacher,
                    Faculty = "Факультет информационных технологий"
                },
                new User { 
                    FirstName = "Ольга", 
                    LastName = "Кузнецова", 
                    Email = "kuznetsova@edu.com", 
                    Password = "teacher123",
                    Role = UserRole.Teacher,
                    Faculty = "Факультет информационных технологий"
                }
            };

            context.Users.AddRange(teachers);
            context.SaveChanges();

            // Создаем студентов
            var students = new User[]
            {
                new User { 
                    FirstName = "Мария", 
                    LastName = "Козлова", 
                    Email = "kozlova@student.edu", 
                    Password = "student123",
                    Role = UserRole.Student,
                    Faculty = "Факультет информационных технологий"
                },
                new User { 
                    FirstName = "Дмитрий", 
                    LastName = "Соколов", 
                    Email = "sokolov@student.edu", 
                    Password = "student123",
                    Role = UserRole.Student,
                    Faculty = "Факультет информационных технологий"
                },
                new User { 
                    FirstName = "Анна", 
                    LastName = "Морозова", 
                    Email = "morozova@student.edu", 
                    Password = "student123",
                    Role = UserRole.Student,
                    Faculty = "Факультет информационных технологий"
                },
                new User { 
                    FirstName = "Павел", 
                    LastName = "Волков", 
                    Email = "volkov@student.edu", 
                    Password = "student123",
                    Role = UserRole.Student,
                    Faculty = "Факультет информационных технологий"
                },
                new User { 
                    FirstName = "Екатерина", 
                    LastName = "Новикова", 
                    Email = "novikova@student.edu", 
                    Password = "student123",
                    Role = UserRole.Student,
                    Faculty = "Факультет информационных технологий"
                }
            };

            context.Users.AddRange(students);
            context.SaveChanges();

            // Создаем администратора
            var admin = new User
            {
                FirstName = "Администратор",
                LastName = "Системы",
                Email = "admin@edu.com",
                Password = "admin123",
                Role = UserRole.Administrator,
                Faculty = "Администрация"
            };

            context.Users.Add(admin);
            context.SaveChanges();

            // Создаем курсы
            var courses = new Course[]
            {
                new Course {
                    CourseName = "Математический анализ",
                    CourseCode = "MATH101",
                    Description = "Основы математического анализа, включая пределы, производные и интегралы. Курс охватывает основные концепции дифференциального и интегрального исчисления.",
                    Credits = 5,
                    TeacherId = teachers[0].Id
                },
                new Course {
                    CourseName = "Программирование на C#",
                    CourseCode = "CS201",
                    Description = "Изучение основ программирования на языке C# и платформе .NET. Включает работу с классами, интерфейсами, коллекциями и LINQ.",
                    Credits = 4,
                    TeacherId = teachers[1].Id
                },
                new Course {
                    CourseName = "Базы данных",
                    CourseCode = "DB101",
                    Description = "Основы проектирования и работы с базами данных. SQL, нормализация, индексы и оптимизация запросов.",
                    Credits = 4,
                    TeacherId = teachers[2].Id
                },
                new Course {
                    CourseName = "Веб-разработка",
                    CourseCode = "WEB201",
                    Description = "Разработка современных веб-приложений. HTML5, CSS3, JavaScript, ASP.NET Core MVC.",
                    Credits = 3,
                    TeacherId = teachers[1].Id
                },
                new Course {
                    CourseName = "Алгоритмы и структуры данных",
                    CourseCode = "ALG101",
                    Description = "Изучение основных алгоритмов и структур данных. Анализ сложности, сортировки, деревья, графы.",
                    Credits = 4,
                    TeacherId = teachers[3].Id
                },
                new Course {
                    CourseName = "Операционные системы",
                    CourseCode = "OS201",
                    Description = "Основы операционных систем. Процессы, потоки, память, файловые системы.",
                    Credits = 4,
                    TeacherId = teachers[2].Id
                }
            };

            context.Courses.AddRange(courses);
            context.SaveChanges();

            // Создаем расписание для курсов
            var schedules = new Schedule[]
            {
                // Математический анализ
                new Schedule {
                    CourseId = courses[0].Id,
                    DayOfWeek = DayOfWeek.Monday,
                    StartTime = new DateTime(2024, 1, 1, 9, 0, 0),
                    EndTime = new DateTime(2024, 1, 1, 10, 30, 0),
                    Location = "Аудитория 101"
                },
                new Schedule {
                    CourseId = courses[0].Id,
                    DayOfWeek = DayOfWeek.Wednesday,
                    StartTime = new DateTime(2024, 1, 1, 9, 0, 0),
                    EndTime = new DateTime(2024, 1, 1, 10, 30, 0),
                    Location = "Аудитория 101"
                },
                // Программирование на C#
                new Schedule {
                    CourseId = courses[1].Id,
                    DayOfWeek = DayOfWeek.Tuesday,
                    StartTime = new DateTime(2024, 1, 1, 11, 0, 0),
                    EndTime = new DateTime(2024, 1, 1, 12, 30, 0),
                    Location = "Компьютерный класс 201"
                },
                new Schedule {
                    CourseId = courses[1].Id,
                    DayOfWeek = DayOfWeek.Thursday,
                    StartTime = new DateTime(2024, 1, 1, 11, 0, 0),
                    EndTime = new DateTime(2024, 1, 1, 12, 30, 0),
                    Location = "Компьютерный класс 201"
                },
                // Базы данных
                new Schedule {
                    CourseId = courses[2].Id,
                    DayOfWeek = DayOfWeek.Monday,
                    StartTime = new DateTime(2024, 1, 1, 13, 0, 0),
                    EndTime = new DateTime(2024, 1, 1, 14, 30, 0),
                    Location = "Компьютерный класс 202"
                },
                new Schedule {
                    CourseId = courses[2].Id,
                    DayOfWeek = DayOfWeek.Friday,
                    StartTime = new DateTime(2024, 1, 1, 13, 0, 0),
                    EndTime = new DateTime(2024, 1, 1, 14, 30, 0),
                    Location = "Компьютерный класс 202"
                },
                // Веб-разработка
                new Schedule {
                    CourseId = courses[3].Id,
                    DayOfWeek = DayOfWeek.Wednesday,
                    StartTime = new DateTime(2024, 1, 1, 15, 0, 0),
                    EndTime = new DateTime(2024, 1, 1, 16, 30, 0),
                    Location = "Компьютерный класс 203"
                },
                new Schedule {
                    CourseId = courses[3].Id,
                    DayOfWeek = DayOfWeek.Friday,
                    StartTime = new DateTime(2024, 1, 1, 15, 0, 0),
                    EndTime = new DateTime(2024, 1, 1, 16, 30, 0),
                    Location = "Компьютерный класс 203"
                },
                // Алгоритмы и структуры данных
                new Schedule {
                    CourseId = courses[4].Id,
                    DayOfWeek = DayOfWeek.Tuesday,
                    StartTime = new DateTime(2024, 1, 1, 13, 0, 0),
                    EndTime = new DateTime(2024, 1, 1, 14, 30, 0),
                    Location = "Аудитория 102"
                },
                new Schedule {
                    CourseId = courses[4].Id,
                    DayOfWeek = DayOfWeek.Thursday,
                    StartTime = new DateTime(2024, 1, 1, 13, 0, 0),
                    EndTime = new DateTime(2024, 1, 1, 14, 30, 0),
                    Location = "Аудитория 102"
                },
                // Операционные системы
                new Schedule {
                    CourseId = courses[5].Id,
                    DayOfWeek = DayOfWeek.Monday,
                    StartTime = new DateTime(2024, 1, 1, 15, 0, 0),
                    EndTime = new DateTime(2024, 1, 1, 16, 30, 0),
                    Location = "Компьютерный класс 204"
                },
                new Schedule {
                    CourseId = courses[5].Id,
                    DayOfWeek = DayOfWeek.Wednesday,
                    StartTime = new DateTime(2024, 1, 1, 15, 0, 0),
                    EndTime = new DateTime(2024, 1, 1, 16, 30, 0),
                    Location = "Компьютерный класс 204"
                }
            };

            context.Schedules.AddRange(schedules);
            context.SaveChanges();

            // Записываем студентов на курсы и выставляем оценки
            var studentCourses = new StudentCourse[]
            {
                // Мария Козлова
                new StudentCourse {
                    StudentId = students[0].Id,
                    CourseId = courses[0].Id,
                    Grade = 85
                },
                new StudentCourse {
                    StudentId = students[0].Id,
                    CourseId = courses[1].Id,
                    Grade = 92
                },
                new StudentCourse {
                    StudentId = students[0].Id,
                    CourseId = courses[4].Id,
                    Grade = 88
                },

                // Дмитрий Соколов
                new StudentCourse {
                    StudentId = students[1].Id,
                    CourseId = courses[0].Id,
                    Grade = 78
                },
                new StudentCourse {
                    StudentId = students[1].Id,
                    CourseId = courses[2].Id,
                    Grade = 88
                },
                new StudentCourse {
                    StudentId = students[1].Id,
                    CourseId = courses[5].Id,
                    Grade = 85
                },

                // Анна Морозова
                new StudentCourse {
                    StudentId = students[2].Id,
                    CourseId = courses[1].Id,
                    Grade = 95
                },
                new StudentCourse {
                    StudentId = students[2].Id,
                    CourseId = courses[3].Id
                },
                new StudentCourse {
                    StudentId = students[2].Id,
                    CourseId = courses[4].Id,
                    Grade = 91
                },

                // Павел Волков
                new StudentCourse {
                    StudentId = students[3].Id,
                    CourseId = courses[2].Id,
                    Grade = 82
                },
                new StudentCourse {
                    StudentId = students[3].Id,
                    CourseId = courses[5].Id,
                    Grade = 89
                },

                // Екатерина Новикова
                new StudentCourse {
                    StudentId = students[4].Id,
                    CourseId = courses[1].Id,
                    Grade = 94
                },
                new StudentCourse {
                    StudentId = students[4].Id,
                    CourseId = courses[3].Id,
                    Grade = 90
                }
            };

            context.StudentCourses.AddRange(studentCourses);
            context.SaveChanges();
        }
    }
} 