
using System;
using ProjectBank.Infrastructure.Entities;
using Microsoft.Data.Sqlite;

namespace ProjectBank.Server.Controllers
 {
public class TestProjectRepository
{
    private readonly HashSet<ProjectKeyDTO> _dbcontext;
    //private readonly DbSet<Category> _categories;
    //private readonly DbSet<User> _users;
    //private readonly DbSet<Bucket> _buckets;

    public ProjectBankContext _context{get; set;}
    public IProjectRepository TestRepository{get; set;}
    private ITagRepository _tagRepository{get; set;}
    private IUserRepository _userRepository{get; set;}
    private IBucketRepository _bucketRepository{get; set;}

    public TestProjectRepository()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        builder.EnableSensitiveDataLogging();

        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();

        context.SaveChanges();
        _context = context;
        TestRepository = new ProjectRepository(_context);
        _tagRepository = new TagRepository(_context);
        _userRepository = new UserRepository(_context);
        _bucketRepository = new BucketRepository(_context);
        //add();
    }
    private async void add()
    {
        /*await _tagRepository.CreateAsync(new TagCreateDTO{Name = "agriculture"});
        await _userRepository.CreateAsync(new SupervisorCreateDTO{});
        await _userRepository.CreateAsync(new StudentCreateDTO{});
        await _bucketRepository.CreateAsync(new BucketCreateDTO{Key = "96c842e9da8fd2e3d76890658ce7e43d08de90c23f52a423f8ebd4ccad56c627fa1287d3286e15e20968d590bfbbbb6d53162a1c"});
        await _bucketRepository.CreateAsync(new BucketCreateDTO{Key = "f8ea9e07f542c0cdf0805e5f30c76cb6ff5facdc5d5ee3e511b93375d0f73aab14d0b4afce62d38007c76a089fcf103ef8da82b360a1e42ae654a49ac270ba65"});
        await _bucketRepository.CreateAsync(new BucketCreateDTO{Key = "8e92a5e09ab90a391102756ea6496af289b5eefd9c6ffa3453b1987f725381953ffa3b43fc63d7dd5b6af3859e40e3bf654da02b722392f10d204f354e2d9e1eagriculture"});
*/


        for(int i = 0; i < 5; i++)
        {
            await TestRepository.CreateAsync(new ProjectCreateDTO{AuthorID = 1, Title = "Project: " + i, Description = "Description", Status = ProjectStatus.PUBLIC, MaxStudents = 3, CategoryID = 1, TagIDs = new List<int>{1}, UserIDs = new List<int>{2}, BucketIDs = new List<int>{1, 2, 3}});
        }
    }
 }
 }
