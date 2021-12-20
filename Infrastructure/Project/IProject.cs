using ProjectBank.Infrastructure.Entities;

namespace ProjectBank.Infrastructure;

public interface IProject : ITagable
{
    //int Id { get; set; }

    Category Category {get;}

}