using ProjectBank.Infrastructure.Entities;

namespace ProjectBank.Infrastructure.ReferenceSystem
{
public class ProjectLSH : LocalitySensitiveHashTable<IProject>
{
     public IReadOnlyCollection<IProject> GetSortedInCategory(IProject tagable)
        {
            var casted = tagable;
            if(casted.Category == null) throw new ArgumentException("The object provided does not have a category");
            var category = casted.Category;
            var allCategories = GetSorted(tagable);
            var sorted = new List<IProject>();

            foreach (var tag in allCategories)
            {
                var project = tag;
                if(project.Category.IsRelated(category)) sorted.Add(project);
            }
            return sorted.AsReadOnly();
        }
}
}
