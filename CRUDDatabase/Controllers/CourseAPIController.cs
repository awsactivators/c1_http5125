using CRUDDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;


namespace CRUDDatabase.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class CourseAPIController : ControllerBase
  {
    // Database context class for accessing the MySQL Database.
    private readonly SchoolDbContext _context;
    // dependency injection of database context
    public CourseAPIController(SchoolDbContext context)
    {
      _context = context;
    }

    /// <summary>
    /// Retrieves a list of all courses from the database.
    /// </summary>
    /// <returns>
    /// A list containing all courses in the system.
    /// </returns>
    /// <example>
    /// Example of a GET request to retrieve all courses:
    /// GET /api/CourseAPI/ListCourses
    /// </example>

    [HttpGet]
    [Route(template:"ListCourses")]
    public IEnumerable<Course> ListCourses()
    {
      // Create a list to store course objects
      List<Course> Courses = new List<Course>();

      // Establish a connection to the database
      MySqlConnection Connection = _context.AccessDatabase();
      Connection.Open();

      // Prepare SQL query
      MySqlCommand Command = Connection.CreateCommand();
      Command.CommandText = "SELECT * FROM courses";

      // Execute the query
      MySqlDataReader ResultSet = Command.ExecuteReader();
      

      // Iterate through each row in the result set
      while (ResultSet.Read())
      {
        Course course = new Course
        {
          CourseId = Convert.ToInt32(ResultSet["CourseId"]),
          CourseCode = ResultSet["CourseCode"].ToString(),
          TeacherId = Convert.ToInt32(ResultSet["TeacherId"]),
          StartDate = Convert.ToDateTime(ResultSet["StartDate"]),
          FinishDate = Convert.ToDateTime(ResultSet["FinishDate"]),
          CourseName = ResultSet["CourseName"].ToString()
        };
        Courses.Add(course);
      }

      // Close the database connection
      Connection.Close();

      // Return the list of Course
      return Courses;
    }

    /// <summary>
    /// Retrieves a list of courses taught by a specific teacher, identified by their unique teacher ID.
    /// </summary>
    /// <param name="teacherId">The unique identifier of the teacher whose courses are to be listed.</param>
    /// <returns>A list of course objects associated with the specified teacher. 
    /// </returns>
    /// <example>
    /// Example of a GET request to retrieve courses taught by a teacher with ID 5:
    /// GET /api/CourseAPI/ListCoursesByTeacher/5
    /// </example>

    [HttpGet]
    [Route(template: "ListCoursesByTeacher/{teacherId}")]
    public List<Course> ListCoursesByTeacher(int teacherId)
    {
      List<Course> courses = new List<Course>();

      MySqlConnection Connection = _context.AccessDatabase();
      Connection.Open();

      MySqlCommand Command = Connection.CreateCommand();
      Command.CommandText = "SELECT * FROM Courses WHERE TeacherId = @teacherId";
      Command.Parameters.AddWithValue("@teacherId", teacherId);
      Command.Prepare();

      MySqlDataReader ResultSet = Command.ExecuteReader();

      while (ResultSet.Read())
      {
        Course course = new Course
        {
          CourseId = Convert.ToInt32(ResultSet["CourseId"]),
          CourseCode = ResultSet["CourseCode"].ToString(),
          TeacherId = teacherId,
          StartDate = Convert.ToDateTime(ResultSet["StartDate"]),
          FinishDate = Convert.ToDateTime(ResultSet["FinishDate"]),
          CourseName = ResultSet["CourseName"].ToString()
        };

        courses.Add(course);
      }

      Connection.Close();
      return courses;
    }

    /// <summary>
    /// Retrieves the details of a specific course identified by its unique course ID.
    /// </summary>
    /// <param name="id">The unique identifier of the course to retrieve.</param>
    /// <returns>A course object containing the details of the specified course. 
    /// </returns>
    /// <example>
    /// Example of a GET request to retrieve details of a course with ID 10:
    /// GET /api/CourseAPI/FindCourse/10
    /// </example>

    [HttpGet]
    [Route(template: "FindCourse/{id}")]
    public Course FindCourse(int id)
    {
      MySqlConnection connection = _context.AccessDatabase();
      connection.Open();

      MySqlCommand command = connection.CreateCommand();
      command.CommandText = "SELECT * FROM Courses WHERE CourseId = @id";
      command.Parameters.AddWithValue("@id", id);

      MySqlDataReader resultSet = command.ExecuteReader();
      Course course = null;

      if (resultSet.Read())
      {
        course = new Course
        {
          CourseId = Convert.ToInt32(resultSet["CourseId"]),
          CourseCode = resultSet["CourseCode"].ToString(),
          TeacherId = Convert.ToInt32(resultSet["TeacherId"]),
          StartDate = Convert.ToDateTime(resultSet["StartDate"]),
          FinishDate = Convert.ToDateTime(resultSet["FinishDate"]),
          CourseName = resultSet["CourseName"].ToString()
        };
      }

      connection.Close();
      return course;
    }

  }
}