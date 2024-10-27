using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Technico.Data;
using Technico.IRepositories;
using Technico.Iservices;
using Technico.Models;
using Technico.Repositories;
using Technico.ServicesImpl;
using Technico.ServicesImpl;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<TechnicoDbContext>(options =>
                    options.UseSqlServer("Data Source=(local)\\SQLEXPRESS;Initial Catalog=Technico;Integrated Security=True;TrustServerCertificate=True;"));
                
                services.AddScoped<IOwnerRepository, OwnerRepository>();
                services.AddScoped<IOwnerService, OwnerServiceImpl>();
            })
            .Build();
        
        using (var scope = host.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TechnicoDbContext>();
            await dbContext.Database.EnsureDeletedAsync(); 
            await dbContext.Database.MigrateAsync(); 
        }
        
        var ownerService = host.Services.GetRequiredService<IOwnerService>();
        
        var ownerCreated = new Owner()
        {
            VatNumber = "123456789",
            FirstName = "John",
            LastName = "Doe",
            Address = "123 Main St",
            PhoneNumber = "123-456-7890",
            Email = "john.doe@example.com"
        };

        var result = await ownerService.CreateOwner(ownerCreated);
        if (result.IsSuccess)
        {
            Console.WriteLine($"Owner created: {result.Value.FirstName} {result.Value.LastName} {ownerCreated.Id}");
        }
        else
        {
            Console.WriteLine($"Error: {result.Error}");
        }

        var ownerFound = await ownerService.GetOwner(1);
        Console.WriteLine($"Owner found: {ownerFound.Value.FirstName} {ownerFound.Value.LastName} {ownerFound.Value.VatNumber}");

        var updatedOwner = ownerService.UpdateOwner(ownerCreated, new Owner()
        {
            VatNumber = "12345678910",
            FirstName = "George",
            LastName = "Doe",
            Address = "123 Main St",
            PhoneNumber = "123-456-7890",
            Email = "john.doe@example.com"
        });
        Console.WriteLine($"updated owner: {updatedOwner.Result.Value.FirstName} {updatedOwner.Result.Value.LastName} {updatedOwner.Result.Value.VatNumber}");
        
        var ownerFoundAgain = await ownerService.GetOwner(1);
        Console.WriteLine($"Owner found: {ownerFoundAgain.Value.FirstName} {ownerFoundAgain.Value.LastName} {ownerFoundAgain.Value.VatNumber}");

        var deleteOwner = await ownerService.DeleteOwner(ownerCreated);
        var owner = await ownerService.GetOwner(1);
        if (owner.IsFailure)
        {
            Console.WriteLine("Owner deleted");
        }
        else
        {
            Console.WriteLine("lathos");
        }
        
        
        Console.ReadLine();
    }
}
