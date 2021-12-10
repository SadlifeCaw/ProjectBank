namespace ProjectBank.Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ProjectsController : ControllerBase
{
    private readonly ILogger<ProjectsController> _logger;
    private readonly IProjectRepository _repository;

    public ProjectsController(ILogger<ProjectsController> logger)//, IProjectRepository repository)
    {
        _logger = logger;
        _repository = new TestProjectRepository().TestRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<ProjectKeyDTO>> Get()
        //=> await _repository.ReadAllAsync();
        {
            var meh = await _repository.ReadAllAsync();
            Console.WriteLine(meh.Count());
            return meh;
        }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDTO>> Get(int id)
        => (await _repository.ReadByIDAsync(id)).ToActionResult();

    [Authorize]
    [HttpPost("Post")]
    [ProducesResponseType(typeof(ProjectDTO), 201)]
    public async Task<IActionResult> Post(ProjectCreateDTO project)
    {
        var created = await _repository.CreateAsync(project);

        return CreatedAtAction(nameof(Get), new { created.Item2.Id }, created);
    }

    [Authorize(Roles = $"{Member},{Administrator}")]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] ProjectUpdateDTO project)
            => (await _repository.UpdateAsync(project)).ToActionResult();

    
    /*[Authorize(Roles = Administrator)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
        => (await _repository.DeleteAsync(id)).ToActionResult();*/
}