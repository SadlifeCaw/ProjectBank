using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Collections.Generic;
using ProjectBank.Core;
using ProjectBank.Infrastructure.ReferenceSystem;
using ProjectBank.Infrastructure.Entities;
using ProjectBank.Infrastructure;

namespace ProjectBank.Server.Controllers;
[Authorize]
    [ApiController]
[Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]

    
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ILogger<ProjectController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Tag> Get()
        {
            Institution ITU = new Institution{Id = 1};
            Tag Agriculture = new Tag ("Agriculture");
            Tag Food = new Tag ("Food");
        /*Tag ComputerScience = new Tag ("Computer Science");
        Tag Security = new Tag ("Security");
        Tag Algorithms = new Tag ("Algorithms");
        Tag Simulation = new Tag ("Simulation");
        Tag Farming = new Tag ("Farming");*/
            //var LSH = new ProjectLSH();
            var AgricultureFood = new Project { Category = ITU, Tags = new List<Tag> { Agriculture, Food }, Id = 6, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            //LSH.Insert(AgricultureFood);
            var projects = new Project[1];
            projects[0] = AgricultureFood;

            return Enumerable.Range(1, 5).Select(index => new Tag("Hey"))
        .ToArray();
        }
    
}