namespace ProjectBank.Infrastructure.Entities
{
    public interface ITagable
    {
        IReadOnlyCollection<Tag> Tags {get; set;}
        Signature Signature {get;}
    }
}
