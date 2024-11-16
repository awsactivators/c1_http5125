# cumulative1_http5125: CRUDDatabase - School Management System
This cumulative project involves building a Minimum Viable Product (MVP) on the Teachers table of the provided School Database using ASP.NET Core Web API and MVC.


## Overview

CRUDDatabase is a simple ASP.NET Core MVC application for managing teachers, students, and courses in a school environment. The application provides features for listing, viewing, searching, and filtering information about teachers, students, and courses. Additionally, it supports creating and displaying individual details for each entity, allowing users to interact with the data through a web interface and RESTful APIs.

## Features

- **Teachers**:
  - List all teachers
  - View details of individual teachers
  - Search for teachers by name or hire date
  - Filter by hire date
- **Students**:
  - List all students
  - View details of individual students
- **Courses**:
  - List all courses
  - View details of individual courses

## Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/awsactivators/c1_http5125.git


## Navigate to the project directory:
`cd CRUDDatabase`


## Setup the Database:

- This project uses MySQL as the database. Create a MySQL database named school.

- Update the database connection details in SchoolDbContext.cs with your MySQL credentials.

- Run any database migration scripts if provided, or manually add the required tables (`Teachers`, `Students`, `Courses`).

## Run the application:
`dotnet run`

## Access the application: 

Open a browser and navigate to http://localhost:{port} to view the application.


## Usage

### Teacher Management

- List All Teachers: Navigate to /TeacherPage/List to view all teachers.

- View Teacher Details: Click on a teacher’s details link to see full information, including hire date and associated courses.

- Search and Filter: Use the search form to search by name or filter by hire date.

### Student Management

- List All Students: Navigate to /StudentPage/List to view all students.

- View Student Details: Click on a student’s details link to view individual information.

### Course Management

- List All Courses: Navigate to /CoursePage/List to view all courses.

- View Course Details: Click on a course’s details link to view course information, including associated teacher details.

## API Endpoints

The project includes several RESTful API endpoints to interact with the data programmatically.

### Teacher API

List Teachers:

GET /api/TeacherAPI/ListTeachers

Returns a list of all teachers, optionally filtered by a search key.

Find Teacher by ID:

GET /api/TeacherAPI/FindTeacher/{id}

Returns the details of a specific teacher by ID.

### Student API

List Students:

GET /api/StudentAPI/ListStudents

Returns a list of all students. 

Find Student by ID:

GET /api/StudentAPI/FindStudent/{id}

Returns the details of a specific student by ID.

### Course API

List Courses:

GET /api/CourseAPI/ListCourses

Returns a list of all courses.

Find Course by ID:

GET /api/CourseAPI/FindCourse/{id}

Returns the details of a specific course by ID.

## Project Structure

Controllers: Contains the MVC controllers for handling web requests.

Models: Contains the model classes for Teacher, Student, and Course.

Views: Contains the Razor views for displaying data related to teachers, students, and courses. and also Contains the SchoolDbContext class for database connectivity


## Contributing

Contributions are welcome! Please fork the repository and create a pull request to contribute.

