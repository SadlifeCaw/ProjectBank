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
            var courseList =
                    await _dbcontext.Courses
                    .Where(c => program.CourseIDs.Contains(c.Id))
                    .Select(c => c.Id)
                    .ToListAsync();  

            var conflict =
                    await _dbcontext.Programs
                    .Where(p => p.Title == program.Title)
                    .Where(p => p.Description == program.Description)
                    .Where(p => p.Faculty.Id == program.FacultyID)
                    .Where(p => p.Code == program.Code)
                    .Select(p => new ProgramDTO(p.Id, p.Title, p.Description,p.Faculty.Id,p.Code,courseList))
                    .FirstOrDefaultAsync();

            if (conflict != null)
            {
                return (Response.Conflict, conflict);
            }

            var entity = new Program(program.Title);

            _dbcontext.Programs.Add(entity);

            await _dbcontext.SaveChangesAsync();

            return (Response.Created, new ProgramDTO(entity.Id, entity.Title,entity.Description,entity.Faculty.Id,entity.Code,courseList));
        }
        public async Task<ProgramDTO> ReadProgramByIDAsync(int ProgramID)
        {
            var tags = from p in _dbcontext.Programs
                         where p.Id == ProgramID
                         select new ProgramDTO(p.Id, p.Name);

            return await tags.FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<ProgramDTO>> ReadAllAsync() =>
            (await _dbcontext.Programs
                           .Select(p => new ProgramDTO(p.Id, p.Name))
                           .ToListAsync())
                           .AsReadOnly();
}