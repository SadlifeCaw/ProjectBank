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
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]


public class TagController : ControllerBase
{
    private readonly ILogger<TagController> _logger;
    private readonly ITagRepository _TagRepository;
    
    public TagController(ILogger<TagController> logger, ITagRepository TagRepository)
    {
        _logger = logger;
        _TagRepository = TagRepository;
    }

    [AllowAnonymous]
    [HttpGet("GetTag/{TagId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(TagDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<TagDTO>> GetTag(int TagId) 
    =>(await _TagRepository.ReadTagByIDAsync(TagId)).ToActionResult();
}

