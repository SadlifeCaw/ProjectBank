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

            var conflict =
                    await _dbcontext.Programs
                    .Where(c => c.Title == course.Title)
                    .Where(c => c.Description == course.Description)
                    .Where(c => c.Faculty.Title == course.FacultyName)
                    .Where(c => c.Code == course.Code)
                    .Select(c => new CourseDTO(c.Id, c.Title, c.Description,c.Faculty.Title,c.Code))
                    .FirstOrDefaultAsync();

            if (conflict != null)
            {
                return (Response.Conflict, conflict);
            }

            //finds the Faculty related to the institution by its id
            //this should (maybe?) not be this class's responsibility
            var EntityFaculty =
                await _dbcontext.Faculties
                              .Where(f => f.Title == course.FacultyName)
                              .Select(f => f)
                              .FirstOrDefaultAsync();

            var entity = new Course
            {
                Title = course.Title,
                Description = course.Description,
                Faculty = EntityFaculty,
                Code = course.Code,
                Programs = await GetProgramsAsync(course.Programs).ToListAsync()
            };

            _dbcontext.Courses.Add(entity);

            await _dbcontext.SaveChangesAsync();

            return (Response.Created, new CourseDTO(entity.Id, entity.Title,entity.Description,entity.Faculty.Title,entity.Code));
        }
        public async Task<CourseDTO> ReadCourseByIDAsync(int CourseID)
        {
            var courses = from c in _dbcontext.Courses
                           where c.Id == CourseID
                           select new CourseDTO(c.Id, c.Title, c.Description, c.Faculty.Title, c.Code);

            return await courses.FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<CourseDTO>> ReadAllAsync() =>
            (await _dbcontext.Programs
                           .Select(c => new CourseDTO(c.Id, c.Title, c.Description, c.Faculty.Title, c.Code))
                           .ToListAsync())
                           .AsReadOnly();

        //used to get existing Programs based on Title and FacultyName given in DTO
        private async IAsyncEnumerable<Course> GetProgramsAsync(ICollection<(int id)> inPrograms)
        {
            var existing = await _dbcontext.Programs
                            .Where(p => inPrograms
                                        .Any(inP => inP.id == p.Id))
                            .Select(p => p)
                            .ToListAsync();

            foreach (var program in existing)
            {
                yield return program;
            }
        }
}