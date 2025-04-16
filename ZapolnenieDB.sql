-- Создание таблицы "Преподаватели"
CREATE TABLE Преподаватели (
    ID_преподавателя INT PRIMARY KEY IDENTITY(1,1),
    ФИО NVARCHAR(255) NOT NULL
);

-- Создание таблицы "Группы"
CREATE TABLE Группы (
    ID_группы INT PRIMARY KEY IDENTITY(1,1),
    Название_группы NVARCHAR(255) NOT NULL
);

-- Создание таблицы "Курсы"
CREATE TABLE Курсы (
    ID_курса INT PRIMARY KEY IDENTITY(1,1),
    Название_курса NVARCHAR(255) NOT NULL,
    Описание TEXT,
    Преподаватель_ID INT,
    FOREIGN KEY (Преподаватель_ID) REFERENCES Преподаватели(ID_преподавателя)
);

-- Создание таблицы "Студенты"
CREATE TABLE Студенты (
    ID_студента INT PRIMARY KEY IDENTITY(1,1),
    ФИО NVARCHAR(255) NOT NULL,
    Дата_рождения DATE NOT NULL,
    Группа_ID INT,
    Курс INT,
    FOREIGN KEY (Группа_ID) REFERENCES Группы(ID_группы)
);

-- Создание таблицы "Оценки"
CREATE TABLE Оценки (
    ID_оценки INT PRIMARY KEY IDENTITY(1,1),
    Студент_ID INT,
    Курс_ID INT,
    Оценка CHAR(1),
    Дата DATE,
    FOREIGN KEY (Студент_ID) REFERENCES Студенты(ID_студента),
    FOREIGN KEY (Курс_ID) REFERENCES Курсы(ID_курса)
);

-- Создание таблицы "Экзамены"
CREATE TABLE Экзамены (
    ID_экзамена INT PRIMARY KEY IDENTITY(1,1),
    Курс_ID INT,
    Дата_экзамена DATE,
    FOREIGN KEY (Курс_ID) REFERENCES Курсы(ID_курса)
);

-- Создание таблицы "Расписание"
CREATE TABLE Расписание (
    ID_расписания INT PRIMARY KEY IDENTITY(1,1),
    Курс_ID INT,
    Время TIME,
    Место NVARCHAR(255),
    FOREIGN KEY (Курс_ID) REFERENCES Курсы(ID_курса)
);

-- Создание таблицы "Пользователи"
CREATE TABLE Пользователи (
    ID_пользователя INT PRIMARY KEY IDENTITY(1,1),
    Логин NVARCHAR(255) NOT NULL,
    Пароль NVARCHAR(255) NOT NULL,
    Роль NVARCHAR(50) NOT NULL
);
