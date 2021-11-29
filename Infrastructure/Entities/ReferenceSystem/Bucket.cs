namespace ProjectBank.Infrastructure.Entities
{
    public class Bucket
    {
        public ISet<ITagable> Projects = new HashSet<ITagable>();
    }
}