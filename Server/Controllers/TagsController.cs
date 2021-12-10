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
    [HttpGet("Tags/{TagId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(TagDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<TagDTO>> GetTag(int TagId) =>
        await _repository.ReadTagByIDAsync(TagId);

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<TagDTO>> Get()
        => await _repository.ReadAllAsync();
}