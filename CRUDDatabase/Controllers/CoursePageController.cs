using CRUDDatabase.Models;
using Microsoft.AspNetCore.Mvc;


namespace CRUDDatabase.Controllers
{

  public class CoursePageController : Controller
  {
    private readonly CourseAPIController _courseApiController;

      public CoursePageController(CourseAPIController courseApiController)
      {
        _courseApiController = courseApiController;
      }

      /// <summary>
      /// Displays a list of courses.
      /// </summary>
      /// <returns>The view displaying the list of courses.</returns>
      // GET: /CoursePage/List
      public IActionResult List()
      {
        IEnumerable<Course> courses = _courseApiController.ListCourses();
        return View(courses);
      }

      /// <summary>
      /// Displays details of a specific course identified by its unique ID.
      /// </summary>
      /// <param name="id">The unique identifier of the course to retrieve.</param>
      /// <returns>
      /// - The details of the course if it exists.
      /// </returns>
      /// <example>
      /// Example of a GET request to display details of a course with ID 7:
      /// GET /CoursePage/Show/7
      /// </example>

      public IActionResult Show(int id)
      {
        Course course = _courseApiController.FindCourse(id);
        if (course == null)
        {
          return NotFound();
        }
        return View(course);
      }

  }
}