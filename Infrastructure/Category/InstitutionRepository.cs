namespace ProjectBank.Infrastructure;

 public class InstitutionRepository : IInstitutionRepository
    {
        private readonly ProjectBankContext _dbcontext;

        public InstitutionRepository(ProjectBankContext context)
        {
            _dbcontext = context;
        }

        public async Task<(Response, InstitutionDTO)> CreateAsync(InstitutionCreateDTO institution)
        {
            //assume that all institions with the same name are equal
            var conflict =
                await _dbcontext.Institutions
                              .Where(i => i.Title == institution.Title)
                              .Select(i => new InstitutionDTO(i.Id, i.Title, i.Description, i.Faculties.ToListOfIDs()))
                              .FirstOrDefaultAsync();

            if (conflict != null)
            {
                return (Response.Conflict, conflict);
            }

            //get all faculties in the database corresponding to faculty IDs in InstitutionCreateObject
            var Faculties = 
                await _dbcontext.Faculties
                                .Where(f => institution.FacultyIDs.Contains(f.Id))
                                .Select(f => f)
                                .ToListAsync();

            var entity = new Institution(institution.Title, institution.Description, Faculties);

            _dbcontext.Institutions.Add(entity);

            await _dbcontext.SaveChangesAsync();

            return (Response.Created, new InstitutionDTO(entity.Id, entity.Title, entity.Description, institution.FacultyIDs));
        }
        public async Task<InstitutionDTO> ReadByIDAsync(int insitutionID)
        {
            var institutions = from i in _dbcontext.Institutions
                               where i.Id == insitutionID
                               select new InstitutionDTO(i.Id, i.Title, i.Description, i.Faculties.ToListOfIDs());

            return await institutions.FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<InstitutionDTO>> ReadAllAsync() =>
            (await _dbcontext.Institutions
                           .Select(i => new InstitutionDTO(i.Id, i.Title, i.Description, i.Faculties.ToListOfIDs()))
                           .ToListAsync())
                           .AsReadOnly();
}