namespace ProjectBank.Infrastructure;

 public class CourseRepository : ICourseRepository
    {
        private readonly ProjectBankContext _dbcontext;

        public CourseRepository(ProjectBankContext context)
        {
            _dbcontext = context;
        }

        public async Task<(Response, CourseDTO)> CreateAsync(CourseCreateDTO course)
        {

            //assume courses with the same name from the same faculty are equal
            //maybe also account for Code 
            var conflict =
                    await _dbcontext.Courses
                    .Where(c => c.Title == course.Title)
                    .Where(c => c.Faculty.Id == course.FacultyID)
                    .Select(c => new CourseDTO(c.Id, c.Title, c.Description, c.Faculty.Id, c.Code, c.Programs.ToListOfIDs(), c.Students.ToListOfIDs()))
                    .FirstOrDefaultAsync();

            if (conflict != null)
            {
                return (Response.Conflict, conflict);
            }

            var s = course.Programs.Contains(2);

            //finds the Faculty related to the institution by its id
            //this should (maybe?) not be this class's responsibility
            var Programs = (await _dbcontext.Programs
                              .Where(p => course.Programs.Contains(p.Id))
                              .Select(i => i))
                              .ToListAsync();

            var entity = new Course(course.Title, course.Description, Faculty, course.Code, Programs, course.Students);

            _dbcontext.Programs.Add(entity);

            await _dbcontext.SaveChangesAsync();

            return (Response.Created, new ProgramDTO(entity.Id, entity.Title,entity.Description,entity.Faculty.Id,entity.Code, new List<int>()));
        }

        public Task<CourseDTO> ReadCourseByIDAsync(int courseID) 
        {
            throw new NotImplementedException();
        }
        public Task<IReadOnlyCollection<CourseDTO>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }
}