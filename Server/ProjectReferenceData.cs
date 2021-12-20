using ProjectBank.Infrastructure.ReferenceSystem;

namespace ProjectBank.Server; 
public static class ProjectReferenceData {
    public static IProjectLSH _LSH;
    public static async void LoadLSH(IProjectRepository projectRepository, ITagRepository tagRepository, ICategoryRepository categoryRepository)
    {
        if(_LSH == null) 
        {
        _LSH = new ProjectLSH(projectRepository, tagRepository, categoryRepository);
        await _LSH.InsertAll();
        }
    }
}