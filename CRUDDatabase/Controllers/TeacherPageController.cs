using CRUDDatabase.Models;
using Microsoft.AspNetCore.Mvc;


namespace CRUDDatabase.Controllers
{
  public class TeacherPageController : Controller
  {
    private readonly TeacherAPIController _teacherApiController;
    private readonly CourseAPIController _courseApiController;

    public TeacherPageController(TeacherAPIController teacherApiController, CourseAPIController courseApiController)
    {
      _teacherApiController = teacherApiController;
      _courseApiController = courseApiController;
    }

    /// <summary>
    /// Retrieves a filtered list of teachers with optional search key and hire date filter.
    /// Displays an error message if no teachers match the search criteria.
    /// </summary>
    /// <param name="SearchKey">An optional search term to filter teachers by name, or hire date.</param>
    /// <param name="HireDate">An optional date to further filter teachers based on their hire date.</param>
    /// <returns>
    /// - A filtered list of teachers matching the search criteria.
    /// - An empty list with an error message if no teachers match the criteria.
    /// </returns>
    /// <example>
    /// Example of calling this method with a search key and hire date:
    /// GET /TeacherPage/List?SearchKey=John
    /// GET /TeacherPage/List?SearchKey=2022-01-01
    /// </example>

    public IActionResult List(string SearchKey = null, DateTime? HireDate = null)
    {
      IActionResult actionResult = _teacherApiController.ListTeachers(SearchKey);

    // Check if the result is NotFound and handle accordingly
    if (actionResult is NotFoundObjectResult)
    {
      ViewBag.Message = "Teacher doesn't exist.";
      return View(Enumerable.Empty<Teacher>());
    }

    // If the result is Ok, cast it to OkObjectResult and extract the data
    var okResult = actionResult as OkObjectResult;
    IEnumerable<Teacher> teachers = okResult?.Value as IEnumerable<Teacher> ?? Enumerable.Empty<Teacher>();

    // Apply date filtering if HireDate is provided
    if (HireDate.HasValue)
    {
      teachers = teachers.Where(t => t.HireDate.Date == HireDate.Value.Date);
    }

    return View(teachers);
    }

    /// <summary>
    /// Retrieves and displays details of a specific teacher identified by their unique ID, including their associated courses.
    /// </summary>
    /// <param name="id">The unique identifier of the teacher to retrieve.</param>
    /// <returns>
    /// - The details of the teacher, including associated courses, if the teacher is found.
    /// - A "Teacher not found." message if the teacher with the specified ID does not exist.
    /// </returns>
    /// <example>
    /// Example of a GET request to display a teacher’s details with ID 5:
    /// GET /TeacherPage/Show/5
    /// </example>
    /// 
    public ActionResult Show(int id)
    {
      // Call the API to find the teacher by ID
      IActionResult actionResult = _teacherApiController.FindTeacher(id);
      var okResult = actionResult as OkObjectResult;

      if (okResult == null || okResult.Value == null)
      {
        return NotFound("Teacher not found.");
      }

      Teacher selectedTeacher = okResult.Value as Teacher;

      if (selectedTeacher == null)
      {
        return NotFound("Teacher not found.");
      }

      // Fetch courses for the teacher
      selectedTeacher.Courses = _courseApiController.ListCoursesByTeacher(id);

      return View(selectedTeacher);
    }
  }
}



