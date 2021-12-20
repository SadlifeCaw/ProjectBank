using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Collections.Generic;
using ProjectBank.Core;
using ProjectBank.Infrastructure.ReferenceSystem;
using ProjectBank.Infrastructure.Entities;
using ProjectBank.Infrastructure;
using ProjectBank.Core.EF.DTO;
using ProjectBank.Core.EF.Repository;

namespace ProjectBank.Server.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class TagController : ControllerBase
{
    private readonly ILogger<TagController> _logger;
    private readonly ITagRepository _repository;
    
    public TagController(ILogger<TagController> logger, ITagRepository TagRepository)
    {
        _logger = logger;
        _repository = TagRepository;
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(TagDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<TagDTO>> Get(int id) 
        =>(await _repository.ReadTagByIDAsync(id)).ToActionResult();

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(TagDTO), StatusCodes.Status200OK)]
    [Route("Tagnames")]
    [HttpGet]
    public async Task<ICollection<TagDTO>> GetTagNames(List<int> ids) 
        =>(await _repository.ReadCollectionAsync(ids));

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<TagDTO>> Get()
        => await _repository.ReadAllAsync();


    [AllowAnonymous]
    [ProducesResponseType(typeof(TagDTO), 201)] 
    [HttpPost]
    public async Task<IActionResult> Post(TagCreateDTO tag)
    {
        var created = (await _repository.CreateAsync(tag)).Item2;
        return CreatedAtAction(nameof(Get), new { created.Id }, created);
    }
}
