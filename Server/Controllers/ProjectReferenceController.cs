using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Collections.Generic;
using ProjectBank.Core;
using ProjectBank.Infrastructure.ReferenceSystem;
using ProjectBank.Infrastructure.Entities;
using ProjectBank.Infrastructure;
using ProjectBank.Core.EF.DTO;

namespace ProjectBank.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    
    public class ProjectReferenceController : ControllerBase
    {
        private readonly ILogger<ProjectReferenceController> _logger;

        public ProjectReferenceController(ILogger<ProjectReferenceController> logger)
        {
            _logger = logger;
        }

/*
        [HttpGet("Get/{ProjectId}")]
        public async Task<IEnumerable<ProjectDTO>> Get(int ProjectId)
        {
            
            Institution ITU = new Institution{Id = 1};
            Tag Agriculture = new Tag ("Agriculture");
            Tag ComputerScience = new Tag ("Computer Science");
            Tag Security = new Tag ("Security");
            Tag Simulation = new Tag ("Simulation");
            Tag Algorithms = new Tag ("Algorithms");
            Tag Food = new Tag ("Food");
            Tag Farming = new Tag ("Farming");
            var LSH = new ProjectLSH();
            var AgricultureFood = new Project {Category = ITU, Tags = new List<Tag> { Agriculture, Food }, Id = 1, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            var ComputerScienceSimulationAlgorithmsAgriculture = new Project {Category = ITU, Tags = new List<Tag> { ComputerScience, Simulation, Algorithms, Agriculture }, Id = 2, Author = null, Title = "ComputerScienceSimulationAlgorithms", Description = "ComputerScienceSimulationAlgorithmsAgriculture", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            var ComputerScienceAlgorithmsSecurity = new Project {Category = ITU, Tags = new List<Tag> { ComputerScience, Algorithms, Security }, Id = 3, Author = null, Title = "ComputerScienceAlgorithmsSecurity", Description = "ComputerScienceAlgorithmsSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            var AgricultureFarming = new Project {Category = ITU, Tags = new List<Tag> { Agriculture, Farming, Food }, Id = 4, Author = null, Title = "AgricultureFarming", Description = "AgricultureFarming", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            var ComputerScienceAlgorithmsSimulationSecurity = new Project {Category = ITU, Tags = new List<Tag> { ComputerScience, Algorithms, Simulation, Security }, Id = 5, Author = null, Title = "ComputerScienceAlgorithmsSimulationSecurity", Description = "ComputerScienceAlgorithmsSimulationSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            LSH.Insert(AgricultureFarming);
            LSH.Insert(ComputerScienceSimulationAlgorithmsAgriculture);
            LSH.Insert(ComputerScienceAlgorithmsSecurity);
            LSH.Insert(AgricultureFood);
            LSH.Insert(ComputerScienceAlgorithmsSimulationSecurity);

            var projects = LSH.Get(AgricultureFood).ToArray();
            //TagIds.ForEach(async tagid => Tags.Add(await Http.GetFromJsonAsync<ProjectDTO[]>($"Project/GetTag/{tagid}")));
            
            var dtos = new List<ProjectDTO>();
            foreach(var x in projects)
            {
                dtos.Add(new ProjectDTO(x.Id,7, "Hey there", "Hey yo", ProjectStatus.PUBLIC, 3, 19, new List<int>{1, 2, 3}, new List<int>{1,2,3}, new List<int>{1, 2, 3});
            }
            //var dto = new ProjectDTO(6,7, "Hey there", "Hey yo", ProjectStatus.PUBLIC, 3, 19, new List<int>{1, 2, 3}, new List<int>{1,2,3}, new List<int>{1, 2, 3});

            
            return dtos.ToArray();
             
        }
        */
    }
}