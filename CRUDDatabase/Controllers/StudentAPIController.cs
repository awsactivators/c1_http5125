using CRUDDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;


namespace CRUDDatabase.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class StudentAPIController : ControllerBase
  {
    // Database context class for accessing the MySQL Database.
    private readonly SchoolDbContext _context;
    // dependency injection of database context
    public StudentAPIController(SchoolDbContext context)
    {
      _context = context;
    }

    /// <summary>
    /// Retrieves a list of all students from the database.
    /// </summary>
    /// <returns>
    /// A list containing all students in the system.
    /// </returns>
    /// <example>
    /// Example of a GET request to retrieve all students:
    /// GET /api/StudentAPI/ListStudents
    /// </example>
    
    [HttpGet]
    [Route(template: "ListStudents")]
    public IEnumerable<Student> ListStudents()
    {
      // Create a list to store student objects
      List<Student> Students = new List<Student>();

      // Establish a connection to the database
      MySqlConnection Connection = _context.AccessDatabase();
      Connection.Open();

      // Prepare SQL query
      MySqlCommand Command = Connection.CreateCommand();
      Command.CommandText = "SELECT * FROM students";

      // Execute the query
      MySqlDataReader ResultSet = Command.ExecuteReader();
      

      // Iterate through each row in the result set
      while (ResultSet.Read())
      {
        Student student = new Student
        {
          StudentId = Convert.ToInt32(ResultSet["StudentId"]),
          StudentFname = ResultSet["StudentFname"].ToString(),
          StudentLname = ResultSet["StudentLname"].ToString(),
          StudentNumber = ResultSet["StudentNumber"].ToString(),
          EnrolDate = Convert.ToDateTime(ResultSet["EnrolDate"])
        };
        Students.Add(student);
      }
      // Close the database connection
      Connection.Close();

      // Return the list of students
      return Students;
    }

    /// <summary>
    /// Retrieves the details of a specific student identified by their unique student ID.
    /// </summary>
    /// <param name="id">The unique identifier of the student to retrieve.</param>
    /// <returns>
    /// A student object containing the details of the specified student.
    /// </returns>
    /// <example>
    /// Example of a GET request to retrieve details of a student with ID 5:
    /// GET /api/StudentAPI/FindStudent/5
    /// </example>

    [HttpGet]
    [Route(template: "FindStudent/{id}")]
    public Student FindStudent(int id)
    {
      MySqlConnection connection = _context.AccessDatabase();
      connection.Open();

      MySqlCommand command = connection.CreateCommand();
      command.CommandText = "SELECT * FROM Students WHERE StudentId = @id";
      command.Parameters.AddWithValue("@id", id);

      MySqlDataReader resultSet = command.ExecuteReader();
      Student student = null;

      if (resultSet.Read())
      {
        student = new Student
        {
          StudentId = Convert.ToInt32(resultSet["StudentId"]),
          StudentFname = resultSet["StudentFname"].ToString(),
          StudentLname = resultSet["StudentLname"].ToString(),
          StudentNumber = resultSet["StudentNumber"].ToString(),
          EnrolDate = Convert.ToDateTime(resultSet["EnrolDate"])
        };
      }

      connection.Close();
      return student;
    }
  }
}