using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Collections.Generic;
using ProjectBank.Core;
using ProjectBank.Infrastructure.ReferenceSystem;
using ProjectBank.Infrastructure.Entities;
using ProjectBank.Infrastructure;
using ProjectBank.Core.EF.DTO;
using ProjectBank.Server.Model;

namespace ProjectBank.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]

    public class ProjectReferenceController : ControllerBase
    {
        private readonly ILogger<ProjectReferenceController> _logger;

        private IProjectRepository _projectRepository;
        private ITagRepository _tagRepository;
        private ICategoryRepository _categoryRepository;



        public ProjectReferenceController(ILogger<ProjectReferenceController> logger, IProjectRepository projectRepository, ITagRepository tagRepository, ICategoryRepository categoryRepository) //IProjectLSH LSH)
        {
            _logger = logger;
            //Console.WriteLine("ProjectReferenceController in tha houuuse");
            if (ProjectReferenceData._LSH == null)
            {
                ProjectReferenceData._LSH = new ProjectLSH(projectRepository, tagRepository, categoryRepository);
                Task.Run(() => ProjectReferenceData._LSH.InsertAll()).Wait();
            }
            _projectRepository = projectRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
        }

        /*  [AllowAnonymous]
          [ProducesResponseType(StatusCodes.Status404NotFound)]
          [ProducesResponseType(typeof(IReadOnlyCollection<ProjectReferenceDTO>), StatusCodes.Status200OK)]
          [HttpGet("InsertAll")]
          public async Task<Response> InsertAll()
          {
              //return await SeedExtensions.LSH.InsertAll(); 
              //return await _LSH.GetSorted(projectID, size);
          }*/

        [Authorize]
        //[Route("InCategory/{projectID},{size}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IReadOnlyCollection<ProjectReferenceDTO>), StatusCodes.Status200OK)]
        [HttpGet("{projectID},{size}")]
        public async Task<IReadOnlyCollection<ProjectReferenceDTO>> GetSortedInCategory(int projectID, int size)
        {
            //if(first) await _LSH.InsertAll(); 
            //first = false;
            var project = await getProjectById(projectID);
            return await ProjectReferenceData._LSH.GetSortedInCategory(project, size);
        }

        [Authorize]
        [Route("Sorted/{projectID},{size}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IReadOnlyCollection<ProjectReferenceDTO>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IReadOnlyCollection<ProjectReferenceDTO>> GetSorted(int projectID, int size)
        {
            //if(first) await _LSH.InsertAll(); 
            //first = false;
            var project = await getProjectById(projectID);
            return await ProjectReferenceData._LSH.GetSorted(project, size);
        }

        private async Task<IProject> getProjectById(int id)
        {
            var dto = (await _projectRepository.ReadByIDAsync(id)).Value;
            var tags = new List<Tag>();
            //await foreach(var tag in dto.TagIDs.ToAsyncEnumerable()) tags.Add(await _context.Tags.Where(c => c.Id == tag).Select(c => c).FirstOrDefaultAsync());
            foreach (var tagid in dto.TagIDs)
            {
                var tagdto = (await _tagRepository.ReadTagByIDAsync(tagid)).Value;
                tags.Add(new Tag(tagdto.Name));
                //tags.Add(await _context.Tags.Where(t => t.Id == tagid).Select(t => t).FirstOrDefaultAsync());
            }
            //await foreach(var category in dto.CategoryID.ToAsyncEnumerable()) tags.Add(await _context.Tags.Where(c => c.Id == tag).Select(c => c).FirstOrDefaultAsync());
            var categoryDTO = (await _categoryRepository.Read(dto.CategoryID));
            return (new ProjectReference() { Id = dto.Id, Tags = tags, Category = new Category() { Id = categoryDTO.Id, Description = categoryDTO.Description, Title = categoryDTO.Title }, Signature = new Signature(tags) });

            /*return await (_context.Projects
                        .Where(p => p.Id == id)
                        .Select(p => p))
                        .FirstOrDefaultAsync();*/
        }

        /* [Authorize]
         [ProducesResponseType(StatusCodes.Status404NotFound)]
         [ProducesResponseType(typeof(IReadOnlyCollection<ProjectReferenceDTO>), StatusCodes.Status200OK)]
         [HttpGet("{projectID},{size}")]
         public async Task<IReadOnlyCollection<ProjectReferenceDTO>> GetSortedByCategory(int projectID, int size)
         {
             await _LSH.InsertAll(); 
             return await _LSH.GetSorted(projectID, size);
         }*/


        //Get x most sorted by category
        //Get x most sorted



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