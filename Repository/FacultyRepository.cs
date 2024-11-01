 using QLBN.Models;

namespace QLBN.Repository
{
    public class FacultyRepository : IFacultyRepository
    {
        public readonly QLBNContext _context;
        public FacultyRepository( QLBNContext context )
        {
            _context = context;
        }
        public Faculty Add(Faculty faculty)
        {
            _context.Faculties.Add( faculty );
            _context.SaveChanges();
            return faculty;
        }

        public Faculty Delete(string facultyId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Faculty> GetAllFaculty()
        {
            return _context.Faculties;
        }

        public Faculty GetFaculty(string facultyId)
        {
            return _context.Faculties.Find(facultyId);
        }

        public Faculty Update(Faculty faculty)
        {
            _context.Update(faculty);
            _context.SaveChanges();
            return faculty;
        }
    }
}
