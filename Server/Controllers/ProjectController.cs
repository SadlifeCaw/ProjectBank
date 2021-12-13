//I'm pretty sure this class is useless when we have ProjectsController
//Outcommented


/* using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Collections.Generic;
using ProjectBank.Core;
using ProjectBank.Infrastructure.ReferenceSystem;
using ProjectBank.Infrastructure.Entities;
using ProjectBank.Infrastructure;
using ProjectBank.Core.EF.Repository;
using ProjectBank.Core.EF.DTO;

namespace ProjectBank.Server.Controllers{


[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ProjectController : ControllerBase
{
    private readonly ILogger<ProjectController> _logger;
    private readonly IProjectRepository _ProjectRepository;
    
    public ProjectController(ILogger<ProjectController> logger, IProjectRepository ProjectRepository)
    {
        _logger = logger;
        _ProjectRepository = ProjectRepository;
    }
    
    [AllowAnonymous]
    [HttpGet("Get/{Projectstr}")]
    public async Task<IEnumerable<ProjectDTO>> Get(int id)
    {
        //Hent project fra databasen
        var project = await _ProjectRepository.ReadByIDAsync(id);
        
        //var TagIds = projectDTO.TagIDs;
        //var Tags = new List<TagDTO>();
        //TagIds.ForEach(async tagid => Tags.Add(await Http.GetFromJsonAsync<ProjectDTO[]>($"Project/GetTag/{tagid}")));
        
        
        //var LSH = new ProjectLSH();

        //return await _ProjectRepository.ReadByIDAsync(ProjectId);

        //new ProjectDTO(6,7, "Hey there", "Hey yo", ProjectStatus.PUBLIC, 3, 19, new List<int>{1, 2, 3}, new List<int>{1,2,3}, new List<int>{1, 2, 3})
        
        //var TagIds = projectDTO.TagIDs;
        //var Tags = new List<TagDTO>();
        //TagIds.ForEach(async tagid => Tags.Add(await Http.GetFromJsonAsync<ProjectDTO[]>($"Project/GetTag/{tagid}")));
        //var AgricultureFood = new Project {Category = null, Tags = null, Id = 6, Author = new Supervisor(), Title = "AgricultureFood", Description = "AgricultureFood" };
        var desc = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like). Where does it come from? Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of de Finibus Bonorum et Malorum (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, Lorem ipsum dolor sit amet.., comes from a line in section 1.10.32.";
        var dto = new ProjectDTO(6,2, "Hey there", desc+desc, ProjectStatus.PUBLIC, 3, 3, new List<int>{1, 2, 3}, new List<int>{1,2,3}, new List<int>{1, 2, 3});
        return Enumerable.Range(1, 5).Select(index => dto).ToArray();
    }
}
} */