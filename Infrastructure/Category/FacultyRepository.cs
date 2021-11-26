
namespace ProjectBank.Infrastructure;

 public class FacultyRepository : IFacultyRepository
    {
        private readonly ProjectBankContext _dbcontext;

        public FacultyRepository(ProjectBankContext context)
        {
            _dbcontext = context;
        }

        public async Task<(Response, FacultyDTO)> CreateAsync(FacultyCreateDTO faculty)
        {
            var conflict =
                await _dbcontext.Faculties
                              .Where(f => f.Title == faculty.Title)
                              .Select(f => new FacultyDTO(f.Id, f.Title, f.Description, f.Institution.Id))
                              .FirstOrDefaultAsync();

            if (conflict != null)
            {
                return (Response.Conflict, conflict);
            }

            //optimize later. necesarry since DTO's cant hold object references
            //finds the institution related to the faculty by its id
            //this should not be this class's responsibility
            var institution =
                await _dbcontext.Institutions
                              .Where(i => i.Id == faculty.InstitutionID)
                              .Select(i => i)
                              .FirstOrDefaultAsync();

            var entity = new Faculty(faculty.Title, faculty.Description, institution);

            _dbcontext.Faculties.Add(entity);

            await _dbcontext.SaveChangesAsync();

            return (Response.Created, new FacultyDTO(entity.Id, entity.Title, entity.Description, entity.Institution.Id));
        }
        public async Task<FacultyDTO> ReadByIDAsync(int facultyID)
        {
            var faculties = from f in _dbcontext.Faculties
                         where f.Id == facultyID
                         select new FacultyDTO(f.Id, f.Title, f.Description, f.Institution.Id);

            return await faculties.FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<FacultyDTO>> ReadAllAsync() =>
            (await _dbcontext.Faculties
                           .Select(f => new FacultyDTO(f.Id, f.Title, f.Description, f.Institution.Id))
                           .ToListAsync())
                           .AsReadOnly();
}