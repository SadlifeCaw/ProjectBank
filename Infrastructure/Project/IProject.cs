using ProjectBank.Infrastructure.Entities;

namespace ProjectBank.Infrastructure;

public interface IProject : ITagable
{
    Category Category {get;}

}