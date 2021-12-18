namespace ProjectBank.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserRepository _repository;

    public UsersController(ILogger<UsersController> logger, IUserRepository repository)
    {
        _logger = logger;
        //_repository = new TestProjectRepository().TestRepository;
        _repository = repository;

    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<UserDTO>> Get()
        => await _repository.ReadAllAsync();


    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> Get(int id)
        => (await _repository.ReadByID(id)).ToActionResult();

    [AllowAnonymous]
    [Route("Supervisors/{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<UserDTO>> GetSupervisor(int id)
        => (await _repository.ReadSupervisor(id)).ToActionResult();

    [AllowAnonymous]
    [Route("Students/{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<UserDTO>> GetStudent(int id)
        => (await _repository.ReadStudent(id)).ToActionResult();
}