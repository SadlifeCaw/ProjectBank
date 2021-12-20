using ProjectBank.Infrastructure.ReferenceSystem;

namespace ProjectBank.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]

    public class ProjectReferenceController : ControllerBase
    {
        private readonly ILogger<ProjectReferenceController> _logger;

        private IProjectRepository _projectRepository;
        private ITagRepository _tagRepository;
        private ICategoryRepository _categoryRepository;



        public ProjectReferenceController(ILogger<ProjectReferenceController> logger, IProjectRepository projectRepository, ITagRepository tagRepository, ICategoryRepository categoryRepository) //IProjectLSH LSH)
        {
            _logger = logger;
            if (ProjectReferenceData._LSH == null)
            {
                ProjectReferenceData._LSH = new ProjectLSH(projectRepository, tagRepository, categoryRepository);
                Task.Run(() => ProjectReferenceData._LSH.InsertAll()).Wait();
            }
            _projectRepository = projectRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IReadOnlyCollection<ProjectReferenceDTO>), StatusCodes.Status200OK)]
        [HttpGet("{projectID},{size}")]
        public async Task<IReadOnlyCollection<ProjectReferenceDTO>> GetSortedInCategory(int projectID, int size)
        {
            var project = await getProjectById(projectID);
            return await ProjectReferenceData._LSH.GetSortedInCategory(project, size);
        }

        [Authorize]
        [Route("Sorted/{projectID},{size}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IReadOnlyCollection<ProjectReferenceDTO>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IReadOnlyCollection<ProjectReferenceDTO>> GetSorted(int projectID, int size)
        {
            var project = await getProjectById(projectID);
            return await ProjectReferenceData._LSH.GetSorted(project, size);
        }

        private async Task<IProject> getProjectById(int id)
        {
            var dto = (await _projectRepository.ReadByIDAsync(id)).Value;
            var tags = new List<Tag>();
            foreach (var tagid in dto.TagIDs)
            {
                var tagdto = (await _tagRepository.ReadTagByIDAsync(tagid)).Value;
                tags.Add(new Tag(tagdto.Name));
            }
            var categoryDTO = (await _categoryRepository.Read(dto.CategoryID));
            return (new ProjectReference() { Id = dto.Id, Tags = tags, Category = new Category() { Id = categoryDTO.Id, Description = categoryDTO.Description, Title = categoryDTO.Title }, Signature = new Signature(tags) });
        }
    }
}