namespace ProjectBank.Infrastructure.Entities
{
    public class Bucket<Tagable>
    where Tagable : ITagable
    {
        [Required]
        public int Id {get; set;}
        public ISet<Tagable> Projects = new HashSet<Tagable>(); 


        public Bucket() {}

    }

    public class ProjectBucket : Bucket<Project>
    {
        public ISet<Project> Projects = new HashSet<Project>();

        [Required]
        public string Key {get; set;}

        public ProjectBucket(ISet<Project> Projects, string Key)
        {
               this.Projects = Projects;
               this.Key = Key;
        }

         public ProjectBucket() {}
    }
        
}