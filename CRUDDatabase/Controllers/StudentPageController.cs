using MySql.Data.MySqlClient;
using CRUDDatabase.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUDDatabase.Controllers
{
  public class StudentPageController : Controller
  {
    private readonly StudentAPIController _studentApiController;

    public StudentPageController(StudentAPIController studentApiController)
    {
      _studentApiController = studentApiController;
    }

    /// <summary>
    /// Displays a list of students.
    /// </summary>
    /// <returns>The view displaying the list of students.</returns>
    // GET: /StudentPage/List
    
    public IActionResult List()
    {
      IEnumerable<Student> students = _studentApiController.ListStudents();
      return View(students);
    }

    /// <summary>
    /// Displays details of a specific student identified by their unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the student to retrieve.</param>
    /// <returns>
    /// The details of the student if the student exists.
    /// </returns>
    /// <example>
    /// Example of a GET request to display details of a student with ID 15:
    /// GET /StudentPage/Show/15
    /// </example>

    public IActionResult Show(int id)
    {
      Student student = _studentApiController.FindStudent(id);
      if (student == null)
      {
          return NotFound();
      }
      return View(student);
    }
  }
}
