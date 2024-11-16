using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using CRUDDatabase.Models;

namespace CRUDDatabase.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class TeacherAPIController : ControllerBase
  {
    private readonly SchoolDbContext _context;
    // dependency injection of database context
    public TeacherAPIController(SchoolDbContext context)
    {
      _context = context;
    }

    /// <summary>
    /// Retrieves a list of teachers from the database, optionally filtered by a search key.
    /// The search key can match the teacher's first name, last name, full name, or hire date.
    /// </summary>
    /// <param name="SearchKey">An optional parameter to filter teachers by first name, last name, full name, or hire date.</param>
    /// <returns>
    /// - A list of <see cref="Teacher"/> objects that match the search criteria, returned with an HTTP 200 status code if found.
    /// - An HTTP 404 status code with a custom message if no teachers match the search criteria.
    /// </returns>
    /// <example>
    /// Example of a GET request to search for teachers by name or hire date:
    /// GET /api/TeacherAPI/ListTeachers?SearchKey=John
    /// GET /api/TeacherAPI/ListTeachers?SearchKey=2022-01-01
    /// </example>

    [HttpGet]
    [Route(template: "ListTeachers/{SearchKey?}")]
    public IActionResult ListTeachers(string SearchKey = null)
    {
      // Create a connection to the database
      MySqlConnection Connection = _context.AccessDatabase();
      Connection.Open();

      // Prepare SQL query with optional search key
      MySqlCommand Command = Connection.CreateCommand();

      Command.CommandText = @"SELECT * FROM Teachers 
                            WHERE LOWER(teacherfname) LIKE LOWER(@Key) 
                            OR LOWER(teacherlname) LIKE LOWER(@Key) 
                            OR LOWER(CONCAT(teacherfname, ' ', teacherlname)) LIKE LOWER(@Key) or hiredate Like @Key or DATE_FORMAT(hiredate, '%d-%m-%Y') Like @Key or salary LIKE @Key ";
      Command.Parameters.AddWithValue("@Key", "%" + SearchKey + "%");

      Command.Prepare();

      // Execute the query
      MySqlDataReader ResultSet = Command.ExecuteReader();

      // Create a list to hold teacher objects
      List<Teacher> Teachers = new List<Teacher>();

      // Loop through each row in the result set
      while (ResultSet.Read())
      {
        // Retrieve column information
        int Id = Convert.ToInt32(ResultSet["teacherId"]);
        string Firstname = ResultSet["teacherFname"].ToString();
        string Lastname = ResultSet["teacherLname"].ToString();
        string EmployeeNo = ResultSet["employeenumber"].ToString();
        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

        // Create a new Teacher object and populate its properties
        Teacher NewTeacher = new Teacher
        {
          TeacherId = Id,
          TeacherFname = Firstname,
          TeacherLname = Lastname,
          EmployeeNumber = EmployeeNo,
          HireDate = HireDate,
          Salary = Salary
        };

        // Add the teacher to the list
        Teachers.Add(NewTeacher);
      }

      // Close the database connection
      Connection.Close();

      if (Teachers.Count == 0)
      {
        return NotFound(new { Message = "Teacher doesn't exist." });
      }

      // Return the list of teachers
      return Ok(Teachers);
    }

    /// <summary>
    /// Retrieves the details of a specific teacher identified by their unique teacher ID.
    /// </summary>
    /// <param name="id">The unique identifier of the teacher to retrieve.</param>
    /// <returns>
    /// - A <see cref="Teacher"/> object with the details of the specified teacher, returned with an HTTP 200 status code if found.
    /// - An HTTP 404 status code with a "Teacher not found." message if no teacher with the specified ID exists.
    /// </returns>
    /// <example>
    /// Example of a GET request to retrieve details of a teacher with ID 3:
    /// GET /api/TeacherAPI/FindTeacher/3
    /// </example>

    [HttpGet]
    [Route(template: "FindTeacher/{id}")]
    public IActionResult FindTeacher(int id)
    {
      // Create a new Teacher object
      Teacher SelectedTeacher = new Teacher();

      // Create a connection to the database
      MySqlConnection Connection = _context.AccessDatabase();
      Connection.Open();

      // Prepare SQL query to retrieve teacher information
      MySqlCommand Command = Connection.CreateCommand();
      Command.CommandText = "SELECT * FROM Teachers WHERE teacherid = @id";
      Command.Parameters.AddWithValue("@id", id);
      Command.Prepare();

      // Execute the query
      MySqlDataReader ResultSet = Command.ExecuteReader();

      if (!ResultSet.HasRows) 
      {
        Connection.Close();
        return NotFound("Teacher not found.");
      }

      // Populate the teacher object with information from the result set
      while (ResultSet.Read())
      {
        SelectedTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherId"]);
        SelectedTeacher.TeacherFname = ResultSet["teacherFname"].ToString();
        SelectedTeacher.TeacherLname = ResultSet["teacherLname"].ToString();
        SelectedTeacher.EmployeeNumber = ResultSet["employeenumber"].ToString();
        SelectedTeacher.HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
        SelectedTeacher.Salary = Convert.ToDecimal(ResultSet["salary"]);
      }

      // Close the result set
      ResultSet.Close(); 

      // Close the database connection
      Connection.Close();

      // Return the teacher object
      return Ok(SelectedTeacher);;
    }

  }
}
