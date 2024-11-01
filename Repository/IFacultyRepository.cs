using QLBN.Models;
namespace QLBN.Repository
{
    public interface IFacultyRepository
    {
        Faculty Add ( Faculty faculty );
        Faculty Update ( Faculty faculty );
        Faculty Delete (String facultyId );
        Faculty GetFaculty (String facultyId);
        IEnumerable<Faculty> GetAllFaculty();
    }
}
