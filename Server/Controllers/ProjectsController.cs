namespace ProjectBank.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ProjectsController : ControllerBase
{
    private readonly ILogger<ProjectsController> _logger;
    private readonly IProjectRepository _repository;

    public ProjectsController(ILogger<ProjectsController> logger, IProjectRepository repository)
    {
        _logger = logger;
        _repository = repository;

    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<ProjectDTO>> Get()
        => await _repository.ReadAllAsync();


    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDTO>> Get(int id)
        => (await _repository.ReadByIDAsync(id)).ToActionResult();

    [Authorize]
    [ProducesResponseType(typeof(ProjectDTO), 201)]
    [HttpPost]
    public async Task<IActionResult> Post(ProjectCreateDTO project)
    {
        var created = (await _repository.CreateAsync(project)).Item2;
        return CreatedAtAction(nameof(Get), created.Id, created); //Changed: new {created.Item2.Id}
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProjectUpdateDTO project)
    {
        if(id != project.Id)
        {
            return BadRequest("Id mismatch");
        }

        var projectToReturn = await _repository.UpdateAsync(id, project);

        if(projectToReturn == Core.Response.NotFound)
        {
            return BadRequest("project doesnt fucking exist");
        } 
        /* if(projectToReturn == Core.Response.Updated)
        {
            return NotFound("project does fucking exist");
        }  */

        return projectToReturn.ToActionResult();
    }

    
    /*[Authorize(Roles = Administrator)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
        => (await _repository.DeleteAsync(id)).ToActionResult();*/
}