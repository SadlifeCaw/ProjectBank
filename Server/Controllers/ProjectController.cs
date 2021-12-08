using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Collections.Generic;
using ProjectBank.Core;
using ProjectBank.Infrastructure.ReferenceSystem;
using ProjectBank.Infrastructure.Entities;
using ProjectBank.Infrastructure;
using ProjectBank.Core.EF.DTO;

namespace ProjectBank.Server.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]


public class ProjectController : ControllerBase
{
    private readonly ILogger<ProjectController> _logger;
    //private readonly IProjectRepository _ProjectRepository;

    /*
    public ProjectController(ILogger<ProjectController> logger, IProjectRepository ProjectRepository)
    {
        _logger = logger;
        _ProjectRepository = ProjectRepository;
    }
    */


    [AllowAnonymous]
    [HttpGet("GetTag/{TagId}")]
    public IEnumerable<ProjectDTO> Get(int ProjectId)
    {
        //Hent project fra databasen
        var projectDTO = new ProjectDTO(0,"","",ProjectStatus.PUBLIC, new List<int>());//await _ProjectRepository.ReadProjectByIDAsync(ProjectId);
        
        var TagIds = projectDTO.TagIDs;
        var Tags = new List<TagDTO>();
        //TagIds.ForEach(async tagid => Tags.Add(await Http.GetFromJsonAsync<ProjectDTO[]>($"Project/GetTag/{tagid}")));
        
        var LSH = new ProjectLSH();
        //var AgricultureFood = new Project {Category = null, Tags = null, Id = 6, Author = new Supervisor(), Title = "AgricultureFood", Description = "AgricultureFood" };
        //LSH.Insert(AgricultureFood);
        var projects = new Project[1];
        //projects[0] = AgricultureFood;
        //Console.WriteLine(hey);
        
        var dto = new ProjectDTO(6, "Hey there", "Hey yo", ProjectStatus.PUBLIC, new List<int>{1, 2, 3});

        return Enumerable.Range(1, 5).Select(index => dto).ToArray();
        
    }
    

    


}