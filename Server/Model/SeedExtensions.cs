using Microsoft.EntityFrameworkCore;
using ProjectBank.Infrastructure;
using ProjectBank.Core;
using ProjectBank.Infrastructure.Entities;

namespace ProjectBank.Server.Model;

public static class SeedExtensions
{
    public static async Task<IHost> SeedAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ProjectBankContext>();

          
            await SeedProjectsAsync(context);
        }
        return host;
    }

    private static async Task SeedProjectsAsync(ProjectBankContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Projects.AnyAsync())
        {

            //Institutions
            var Institution_ITU = new Institution("ITU", "IT-Universitetet i København"); 
            var Institution_KU = new Institution("KU", "Københavns Universitet"); 
            var Institution_NK = new Institution("Kim Il-Sung University", "The Korean people made the first university of new Korea bear the august name of President Kim Il Sung, the hero of the nation and peerless patriot."); 

            context.Institutions.AddRange(
                Institution_ITU,
                Institution_KU,
                Institution_NK
            );

            //Faculties
            var Faculty_COMPSCI_ITU = new Faculty("Computer Science", "Computers and Science", Institution_ITU);
            var Faculty_BUSI_ITU = new Faculty ("Digital Business","Business and digital stuff", Institution_ITU);
            var Faculty_DESI_ITU = new Faculty("Digital Design","Design and its digital!",Institution_ITU);
            var Faculty_HIS_KU = new Faculty("History", "Boring history stuff",Institution_KU);
            var Faculty_COMPSCI_KU = new Faculty("Computer Science","Computers and Science",Institution_KU);
            var Faculty_MEDISCI_KU = new Faculty("Medical Science","Medical science is important",Institution_KU);
            var Faculty_MILSCI_NK = new Faculty("Millitary Science","The mission is to educate the people on the most acclaimed millitary power in the world.", Institution_NK);
            var Faculty_HIS_NK = new Faculty("History", "The mission is to train specialists in the history of the revolutionary activities of the peerless great persons of Mt Paektu, Korean history, world history, archeology, and folklore", Institution_NK); 

            context.Faculties.AddRange(
                Faculty_COMPSCI_ITU,
                Faculty_BUSI_ITU,
                Faculty_DESI_ITU,
                Faculty_HIS_KU,
                Faculty_COMPSCI_KU,
                Faculty_MEDISCI_KU,
                Faculty_MILSCI_NK,
                Faculty_HIS_NK
            );

            //TeachingPrograms
            var TP_SWU_ITU = new TeachingProgram("Software Development", "The development of software", Faculty_COMPSCI_ITU, "SWU2021", new List<Course>());
            var TP_DATA_ITU = new TeachingProgram("Data Science", "The science of data", Faculty_COMPSCI_ITU,"DATA2021",new List<Course>());
            var TP_DDS_ITU = new TeachingProgram("Digital Design and Interactive Technologies","Teachings in the digital and interactive field",Faculty_DESI_ITU,"DDS2021",new List<Course>());
            var TP_SWU_KU = new TeachingProgram("Software Development", "The development of software", Faculty_COMPSCI_KU, "SWU2021", new List<Course>());
            var TP_HIS_KU = new TeachingProgram("Ancient Greek Studies", "Study of the ancient Greeks", Faculty_HIS_KU, "GRK2019", new List<Course>());
            var TP_TRUTH_NK = new TeachingProgram("Truth Studies", "Teachings in the truth of the history of the peerless new Korean nation", Faculty_HIS_NK, "TRU2022", new List<Course>());
            var TP_LOG_NK = new TeachingProgram("Logistics", "How to supply the glorious army of new Korea", Faculty_MILSCI_NK, "LOG2020", new List<Course>());
            
            context.Programs.AddRange(
                TP_SWU_ITU,
                TP_DATA_ITU,
                TP_DDS_ITU,
                TP_SWU_KU,
                TP_HIS_KU,
                TP_TRUTH_NK,
                TP_LOG_NK
            );

            //Supervisors
            var SP_1 = new Supervisor("troe@itu.dk", Institution_ITU, "Troels", "Jyde", new List<Project>(), Faculty_COMPSCI_ITU, new List<Project>());
            var SP_2 = new Supervisor("kim@nk.nk",Institution_NK,"Kim","Jong-Il", new List<Project>(), Faculty_MILSCI_NK, new List<Project>());
            var SP_3 = new Supervisor("augu@ku.dk",Institution_KU,"August","Beck", new List<Project>(), Faculty_HIS_KU, new List<Project>());

            context.Supervisors.AddRange(
                SP_1,
                SP_2,
                SP_3
            );

            //Students
            var ST_1 = new Student("jens@itu.dk", Institution_ITU, "Jens", "Jensen", new List<Project>(), TP_SWU_ITU, new List<Course>());
            var ST_2 = new Student("bob@itu.dk", Institution_ITU, "Bob", "Bobsen", new List<Project>(), TP_SWU_ITU, new List<Course>());
            var ST_3 = new Student("hans@itu.dk", Institution_ITU, "Hans", "Hansen", new List<Project>(), TP_SWU_ITU, new List<Course>());
            var ST_4 = new Student("hyun@nk.nk", Institution_NK, "Dong-hyun", "Gwan", new List<Project>(), TP_LOG_NK, new List<Course>()); 
            var ST_5 = new Student("kiml@nk.nk", Institution_NK, "Kim", "Lee", new List<Project>(), TP_LOG_NK, new List<Course>()); 
            var ST_6 = new Student("jens@ku.dk", Institution_KU, "Jens", "Jensen", new List<Project>(), TP_SWU_ITU, new List<Course>());
            var ST_7 = new Student("bob@ku.dk", Institution_KU, "Bob", "Bobsen", new List<Project>(), TP_SWU_ITU, new List<Course>());
            var ST_8 = new Student("hans@ku.dk", Institution_KU, "Hans", "Jensen", new List<Project>(), TP_SWU_ITU, new List<Course>());

            context.Students.AddRange(
                ST_1,
                ST_2,
                ST_3,
                ST_4,
                ST_5,
                ST_6,
                ST_7,
                ST_8
            );


            //Tags
            var CARTOGRAPHY = new Tag("Cartography");
            var ENCR = new Tag("Encryption");
            var NAV = new Tag("Navigation");
            var CODE = new Tag("Programming");
            var MATH = new Tag("Math");
            var GREECE = new Tag("Ancient Greece");
            var ROME = new Tag("Ancient Rome");
            var LIT = new Tag("Literature");
            var PHIL = new Tag("Philosophy");
            
            var ALGORITHM = new Tag("Algorithm");
            var JAVA = new Tag("Java");
            var DOT = new Tag("Dotnet");
            var NETWORK = new Tag("Network");
            var HEURISTIC = new Tag("Heuristic");
            var UML = new Tag("Uml");
            var DOCKER = new Tag("Docker");
            var CSHARP = new Tag("C#");
            var GOLANG = new Tag("Golang");
            var PYTHON = new Tag("Python");

            var WARFARE = new Tag("Warfare");
            var TECHNOLOGY = new Tag("Technology");

            var ECON = new Tag("Economics");
            var EAST = new Tag("East Asia");
            var USA = new Tag("USA");
            var SURVEILLANCE = new Tag("Surveillance");
            
            context.Tags.AddRange(
                WARFARE,
                TECHNOLOGY,

                ECON,
                EAST,
                USA,
                SURVEILLANCE,
                
                ALGORITHM,
                JAVA,
                DOT,
                NETWORK,
                HEURISTIC,
                UML,
                DOCKER,
                CSHARP,
                GOLANG,
                PYTHON,
                
                CARTOGRAPHY,
                ENCR,
                NAV,
                CODE,
                MATH,
                GREECE,
                ROME,
                LIT,
                PHIL
            );
            


            //Buckets
            
            
            //Projects
            var PR_1_desc = "The First Year Project is the first large project on this education. The students will plan and conduct a project in small groups, where they will get to apply the techniques they have learned throughout the first two semesters of";
            var PR_1 = new Project(SP_1, "First Year Project of SWU: Map of Denmark of DK", PR_1_desc, ProjectStatus.PUBLIC,Faculty_COMPSCI_ITU, new List<Tag>(){JAVA,ALGORITHM}, new List<User>() {ST_1, ST_7, ST_2}, new List<ProjectBucket>(),20);
            var PR_2 = new Project(SP_1, "Programmeringshoest or how to teach Programming","Welcome to the great ProgrammeringsHoest project, where you will be forced to solve difficult programming exercises every day",ProjectStatus.PUBLIC,Faculty_COMPSCI_ITU,new List<Tag>(){HEURISTIC, JAVA, ALGORITHM}, new List<User>(){ST_4,ST_2,ST_8}, new List<ProjectBucket>(),11);
            var PR_3 = new Project(SP_3, "Ancient Greece and Eros", "This thesis would focus on how the ancient Greeks saw love, eros, and which different interpretations existed during the time of Platon", ProjectStatus.PUBLIC, Faculty_HIS_KU, new List<Tag>() {LIT, GREECE, PHIL}, new List<User>() {ST_6, ST_7}, new List<ProjectBucket>(), 5);
            var PR_4 = new Project(SP_2, "Decadence of the West", "This will focus on the study of the decadent, capitalist West compared to the honorable, liberated North Korean lifestyle", ProjectStatus.PUBLIC, Faculty_HIS_NK, new List<Tag>() {ECON, EAST, LIT}, new List<User>() {ST_4, ST_5}, new List<ProjectBucket>(), 3);
            var PR_5 = new Project(SP_1, "The Role of Software in Government Surveillance", "This thesis is concerned with the growing use of technology by governments to control their populations", ProjectStatus.PUBLIC, Faculty_COMPSCI_ITU, new List<Tag>() {PHIL, EAST, NETWORK, TECHNOLOGY, ENCR, USA, SURVEILLANCE}, new List<User>() {ST_1, ST_2, ST_3, ST_7}, new List<ProjectBucket>(), 7);

            context.Projects.AddRange(
                PR_1,
                PR_2,
                PR_3,
                PR_4,
                PR_5
            );

            await context.SaveChangesAsync();
        }
    }
}