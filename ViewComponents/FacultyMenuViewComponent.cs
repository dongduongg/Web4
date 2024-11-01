
using QLBN.Models;
using Microsoft.AspNetCore.Mvc;
using QLBN.Repository;
namespace QLBN.ViewComponents
{
    public class FacultyMenuViewComponent:ViewComponent
    {
        private readonly IFacultyRepository _faculty;
        public FacultyMenuViewComponent(IFacultyRepository facultyRepository)
        {
            _faculty = facultyRepository;
        }
        public IViewComponentResult Invoke()
        {
            var faculties = _faculty.GetAllFaculty().OrderBy(x=>x.FacultyName);
            return View("Default",faculties);
        }
    }
}
