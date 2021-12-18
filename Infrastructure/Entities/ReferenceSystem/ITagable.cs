namespace ProjectBank.Infrastructure.Entities
{
    public interface ITagable
    {
        int Id {get; set;}
        IReadOnlyCollection<Tag> Tags {get; set;}
        Signature Signature {get;}
    }
}
