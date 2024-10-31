using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Technico.Data;
using Technico.IRepositories;
using Technico.Iservices;
using Technico.Models;
using Technico.Repositories;
using Technico.ServicesImpl;
using Technico.Validators;
using FluentValidation;

//Scroll down until you see the test examples
namespace Technico;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                // Load the configuration from appsettings.json
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                // Configure TechnicoDbContext using the connection string from appsettings.json
                services.AddDbContext<TechnicoDbContext>(options =>
                {
                    options.UseSqlServer(context.Configuration.GetConnectionString("TechnicoDb"));

                    
                    options.LogTo(Console.WriteLine, LogLevel.Warning);
                });
                
                services.AddScoped<IOwnerRepository, OwnerRepository>();
                services.AddScoped<IOwnerService, OwnerServiceImpl>();
                services.AddScoped<IPropertyRepository, PropertyRepository>();
                services.AddScoped<IPropertyService, PropertyServiceImpl>();
                services.AddScoped<IRepairRepository, RepairRepository>();
                services.AddScoped<IRepairService, RepairServiceImpl>();
                
                services.AddValidatorsFromAssemblyContaining<RepairValidator>();
                services.AddValidatorsFromAssemblyContaining<OwnerValidator>();
                services.AddValidatorsFromAssemblyContaining<PropertyValidator>();
            })
            .Build();

        using (var scope = host.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TechnicoDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();
        }

        var ownerService = host.Services.GetRequiredService<IOwnerService>();
        var propertyService = host.Services.GetRequiredService<IPropertyService>();
        var repairService = host.Services.GetRequiredService<IRepairService>();

        //------------------------------Test Cases Implementations------------------------------//
        await CreatePropertyExample();// it will fail because we dont have owners yet
        await CreateRepairExample(); // it will fail because we dont have owners yet
        
        await CreateOwnerExample();                     //success
        await GetOwnerExample();                        //success
        await UpdateOwnerExample();                     //success
        await GetOwnerExample();// we see the user is updated
        //await DeleteOwnerExample(); //If this method runs we will get failure (as we should) in return from the others due to dependencies. Remove the comment in order to see it works.

        await CreatePropertyExample();
        //await SoftDeletePropertyExample(); Remove the comment to softdelete and the property will not be visible
        await GetPropertyExample();                     //success
        await UpdatePropertyExample();                  //success
        //await DeleteOwnerExample();                   
        await GetOwnerExample();
        await GetPropertyExample();                     //success
        await DeletePropertyExample();
        await GetPropertyExample();                     //Not found

        await CreateRepairExample();                    //success
        await GetOwnerExample();                        //success: Now owner display repairs as well
        //await SoftDeleteRepairExample();
        await GetRepairExample();                       //success
        await UpdateRepairExample();                    //success
        await GetPropertyExample();
        await GetRepairExample();                       //success
        await DeleteRepairExample();                    //success
        await GetRepairExample();
        // Repair not found
        await GetOwnerExample();                        // now owner does not have repairs
       
        

        //------------------------------Test Cases Implementations------------------------------//
        async Task CreateOwnerExample()
        {
            var owner1 = new Owner
            {
                VatNumber = "12345678910",
                FirstName = "Alice",
                LastName = "Smith",
                Address = "123 Elm St",
                PhoneNumber = "123-456-7890",
                Email = "alice.smith@example.com",
                Type = OwnerType.House, // Replace with a valid type
                Password = "A1!abcde"
            };
            
            var owner2 = new Owner
            {
                VatNumber = "VAT123456789",
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St, Anytown, USA",
                PhoneNumber = "555-1234",
                Email = "john.doe@example.com"
            };

            var result = await ownerService.CreateOwner(owner1);
            var result1 = await ownerService.CreateOwner(owner2);// invalid owner will not be created
            Console.WriteLine(
                $"Create Owner: {(result.IsSuccess ? $"Success: {result.Value}" : result.Error)} \n");

        }

        async Task GetOwnerExample()
        {
            var result = await ownerService.GetOwner(1); // Assuming ID 1 exists
            Console.WriteLine(
                $"Get Owner: {(result.IsSuccess ? $"Success: {result.Value} " : result.Error)}\n");
        }

        async Task UpdateOwnerExample()
        { 
            var newOwner = new Owner
            {   Id = 1,
                VatNumber = "55566677788", // Unique VAT number
                FirstName = "Robert",
                LastName = "Brown",
                Address = "321 Pine Rd",
                PhoneNumber = "456-123-7890", // Valid format
                Email = "robert.brown@example.com", // Valid email
                Type = OwnerType.House, // Ensure this is a valid enum value
                Password = "Rob3rtB!@n" // Valid password
            };

            var result = await ownerService.UpdateOwner(1, newOwner);
            Console.WriteLine($"Update Owner: {(result.IsSuccess ? $"Success {result.Value}" : result.Error)}\n");
        }

        async Task DeleteOwnerExample()
        {
            var result = await ownerService.DeleteOwner(1);
            Console.WriteLine($"Delete Owner: {(result.IsSuccess ? result.IsSuccess : result.Error)}\n");
        }

        async Task CreatePropertyExample()
        {
            var property = new Property
            {
                IdentificationNumber = "ID123456",
                Address = "456 Elm St",
                YearOfConstruction = 2005,
                Type = PropertyType.DetachedHouse
            };

            var result = await propertyService.CreateProperty(property, new List<string> { "55566677788", "VAT123456789" });
            Console.WriteLine($"Create Property: {(result.IsSuccess ? result.Value : result.Error)}\n");
        }

        async Task GetPropertyExample()
        {
            var result = await propertyService.GetProperty(1); // Assuming ID 1 exists
            if (result.IsSuccess)
            {
                Console.WriteLine($"Get Property: Found - {result.Value}\n");
            }
            else
            {
                Console.WriteLine($"Get Property: {result.Error}\n");
            }
        }

        async Task UpdatePropertyExample()
        {
            var newPropertyData = new Property
            {   Id = 1,
                IdentificationNumber = "ID123456",
                Address = "789 Oak St",
                YearOfConstruction = 2010,
                Type = PropertyType.Maisonet,
            };

            var result = await propertyService.UpdateProperty(1, newPropertyData); // Assuming ID 1 exists
            Console.WriteLine($"Update Property: {(result.IsSuccess ? result.Value : result.Error)}\n");
        }

        async Task DeletePropertyExample()
        {
            var result = await propertyService.DeleteProperty(1); // Assuming ID 1 exists
            Console.WriteLine($"Delete Property: {(result.IsSuccess ? result : result.Error)}\n");
        }

        async Task CreateRepairExample()
        {
            // ONLY FOR THE TEST CASE
            var ownerRepository = host.Services.GetRequiredService<IOwnerRepository>();
            // Assuming you have a valid owner already
            var owner = await ownerRepository.GetOwner(1);
            if (owner != null)
            {
                var repair = new Repair
                {
                    Type = RepairType.Plumbing, // Valid enum value
                    DateTime = DateTime.Now.AddDays(-1), // Valid past date
                    Description = "Fixed a leaking faucet.", // Valid description
                    Address = "123 Elm St, Springfield", // Valid address
                    Status = Status.Complete, // Valid enum value
                    Cost = 150.00m 

                };


                // Test: Create Repair
                var createResult = await repairService.CreateRepair(repair, owner);
                Console.WriteLine(createResult.IsSuccess
                    ? $"Repair created: {createResult.Value}\n"
                    : $"Failed to create repair: {createResult.Error}\n");
            }
            else
            {
                Console.WriteLine($"Owner not found\n");
            }
        }


        async Task GetRepairExample()
        {
            var repairId = 1;
            var getResult = await repairService.GetRepair(repairId);
            Console.WriteLine(getResult.IsSuccess
                ? $"Repair retrieved: {getResult.Value}\n"
                : $"Failed to get repair: {getResult.Error}\n");
        }

        async Task UpdateRepairExample()
        {   var newRepair = new Repair
            {
                Type = RepairType.Plumbing,
                DateTime = DateTime.Now,
                Description = "Fixing a leaking faucet in the bathroom.",
                Address = "123 Water St, Springfield, USA",
                Status = Status.Pending,
                Cost = 150.75m
            };
                var updateResult = await repairService.UpdateRepair(1, newRepair);
                Console.WriteLine(updateResult.IsSuccess
                    ? $"Repair updated: {updateResult.Value}\n"
                    : $"Failed to update repair: {updateResult.Error}");
            }
            
        
        
        async Task DeleteRepairExample()
        {
            var repairId = 1; 
            var repairToDeleteResult = await repairService.GetRepair(repairId);
            if (repairToDeleteResult.IsSuccess)
            {
                // Test: Delete Repair
                var deleteResult = await repairService.DeleteRepair(1);
                Console.WriteLine(deleteResult.IsSuccess ? 
                    "Repair deleted successfully.\n" : 
                    $"Failed to delete repair: {deleteResult.Error}\n");
            }
            else
            {
                Console.WriteLine($"Repair not found for deletion: {repairToDeleteResult.Error}\n");
            }
        }
         
        async Task SoftDeletePropertyExample()
        {
            
            var deactivateResult = await propertyService.DeactivateProperty(1);
            Console.WriteLine(deactivateResult.IsSuccess 
                ? "Property deactivated successfully!" 
                : deactivateResult.Error);
            
            var propertyResult = await propertyService.GetProperty(1);
            if (propertyResult.IsSuccess)
            {
                Console.WriteLine($"Property {propertyResult.Value} is " + 
                                  (propertyResult.IsSuccess ? "active" : "inactive"));
            }
        }
          
        async Task SoftDeleteRepairExample()
        {
            var result = await repairService.DeactivateRepair(1);
            Console.WriteLine(result.IsSuccess
                ? "Repair soft deleted successfully."
                : "Failed to soft delete repair.");
        }
    }
}