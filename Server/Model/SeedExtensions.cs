using System;
using Microsoft.EntityFrameworkCore;
using ProjectBank.Infrastructure;
using ProjectBank.Core;
using ProjectBank.Infrastructure.Entities;
using ProjectBank.Infrastructure.ReferenceSystem;


namespace ProjectBank.Server.Model;

public static class SeedExtensions
{
    private static ProjectBankContext _context;
    public static ProjectLSH LSH = new ProjectLSH(_context);
    public static async Task<IHost> SeedAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ProjectBankContext>();

          
            await SeedProjectsAsync(context);
            _context = context;
            //LSH = new ProjectLSH(context);
        }
        return host;
    }

    private static async Task SeedProjectsAsync(ProjectBankContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Projects.AnyAsync())
        {

            //Institutions
            context.Institutions.AddRange(Seeding.Institutions);

            //Faculties
            context.Faculties.AddRange(Seeding.Faculties);

            //TeachingPrograms
            context.Programs.AddRange(Seeding.TeachingPrograms);
 
            //Supervisors
            context.Supervisors.AddRange(Seeding.Supervisors);


            //Students
            context.Students.AddRange(Seeding.Students);

            //Tags
            context.Tags.AddRange(Seeding.Tags);
            
            //Buckets
            //context.LSH.InsertAll();
            
            await context.SaveChangesAsync();

            //Projects
            context.Projects.AddRange(Seeding.Projects);

            await context.SaveChangesAsync();
            
            LSH = new ProjectLSH(context);
            await LSH.InsertAll();
        }
    }
}