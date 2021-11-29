namespace ProjectBank.Infrastructure;

 public class ProgramRepository : IProgramRepository
    {
        private readonly ProjectBankContext _dbcontext;

        public ProgramRepository(ProjectBankContext context)
        {
            _dbcontext = context;
        }

        public async Task<(Response, ProgramDTO)> CreateAsync(ProgramCreateDTO program)
        {
            var conflict =
                    await _dbcontext.Programs
                    .Where(p => p.Title == program.Title)
                    .Where(p => p.Description == program.Description)
                    .Where(p => p.Faculty.Title == program.FacultyName)
                    .Where(p => p.Code == program.Code)
                    .Select(p => new ProgramDTO(p.Id, p.Title, p.Description, p.Faculty.Title, p.Code,program.Courses))
                    .FirstOrDefaultAsync();

            if (conflict != null)
            {
                return (Response.Conflict, conflict);
            }

            //finds the Faculty related to the institution by its id
            //this should (maybe?) not be this class's responsibility
            var EntityFaculty =
                await _dbcontext.Faculties
                              .Where(f => f.Title == program.FacultyName)
                              .Select(f => f)
                              .FirstOrDefaultAsync();

            var entity = new TeachingProgram
            {
                Title = program.Title,
                Description = program.Description,
                Faculty = EntityFaculty, 
                Code = program.Code,
                Courses = await GetCoursesAsync(program.Courses).ToListAsync()
            };

            _dbcontext.Programs.Add(entity);

            await _dbcontext.SaveChangesAsync();

            return (Response.Created, new ProgramDTO(entity.Id, entity.Title,entity.Description,entity.Faculty.Title,entity.Code,program.Courses));
        }
        public async Task<ProgramDTO> ReadProgramByIDAsync(int ProgramID)
        {
            var programs = from p in _dbcontext.Programs
                           where p.Id == ProgramID
                           select new ProgramDTO(p.Id, p.Title, p.Description, p.Faculty.Title, p.Code, p.Courses.Select(p=> p.Code).ToList());

            return await programs.FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<ProgramDTO>> ReadAllAsync() =>
            (await _dbcontext.Programs
                           .Select(p => new ProgramDTO(p.Id, p.Title, p.Description, p.Faculty.Title, p.Code, p.Courses.Select(p=> p.Code).ToList()))
                           .ToListAsync())
                           .AsReadOnly();

        //used to get existing courses based on Title and FacultyName given in DTO
        private async IAsyncEnumerable<Course> GetCoursesAsync(ICollection<string> inCourses) 
        {
            var existing = await _dbcontext.Courses
                            .Where(c => inCourses
                                        .Any(inC => inC == c.Code))
                            .Select(c => c)
                            .ToListAsync();

            foreach (var course in existing)
            {
                yield return course;
            }
        }
}