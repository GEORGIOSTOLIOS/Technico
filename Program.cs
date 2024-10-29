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
using Technico.ServicesImpl;

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

        //------------------------------Test Cases Run------------------------------//
        
        await CreateOwnerExample();
        await GetOwnerExample();
        await UpdateOwnerExample();
        //await DeleteOwnerExample(); //If this method runs we will get failure (as we should) in return from the others due to dependencies. Remove the comment in order to see it works.

        await CreatePropertyExample();
        //await SoftDeletePropertyExample();
        await GetPropertyExample();
        await UpdatePropertyExample();
        await DeletePropertyExample();

        await CreateRepairExample();
        //await SoftDeleteRepairExample();
        await GetRepairExample();
        await UpdateRepairExample();
        await DeleteRepairExample();

//------------------------------Test Cases Implementations------------------------------//
        async Task CreateOwnerExample()
        {
            var owner = new Owner
            {
                VatNumber = "12345678910",
                FirstName = "Alice",
                LastName = "Smith",
                Address = "123 Elm St",
                PhoneNumber = "123-456-7890",
                Email = "alice.smith@example.com"
            };

            var result = await ownerService.CreateOwner(owner);
            Console.WriteLine(
                $"Create Owner: {(result.IsSuccess ? $"Success: {result.Value.ToString()}" : result.Error)} \n");

        }

        async Task GetOwnerExample()
        {
            var result = await ownerService.GetOwner(1); // Assuming ID 1 exists
            Console.WriteLine(
                $"Get Owner: {(result.IsSuccess ? $"Success: {result.Value.FirstName} {result.Value.LastName}" : result.Error)}\n");
        }

        async Task UpdateOwnerExample()
        {
            var oldOwner = new Owner
            {
                Id = 1, // Existing owner ID
                VatNumber = "12345678910",
                FirstName = "Alice",
                LastName = "Smith",
                Address = "123 Elm St",
                PhoneNumber = "123-456-7890",
                Email = "alice.smith@example.com"
            };

            var newOwner = new Owner
            {
                Id = 1,
                VatNumber = "12345678910",
                FirstName = "Alice",
                LastName = "Johnson", // Changed last name for update
                Address = "456 Oak St", // Changed address
                PhoneNumber = "321-654-0987",
                Email = "alice.johnson@example.com"
            };

            var result = await ownerService.UpdateOwner(oldOwner, newOwner);
            Console.WriteLine($"Update Owner: {(result.IsSuccess ? $"Success {result.Value}" : result.Error)}\n");
        }

        async Task DeleteOwnerExample()
        {
            var owner = new Owner
            {
                Id = 1,
                VatNumber = "12345678910"
            };

            var result = await ownerService.DeleteOwner(owner);
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

            var result = await propertyService.CreateProperty(property, new List<string> { "12345678910" });
            Console.WriteLine($"Create Property: {(result.IsSuccess ? result.Value : result.Error)}\n");
        }

        async Task GetPropertyExample()
        {
            var result = await propertyService.GetProperty(1); // Assuming ID 1 exists
            if (result.IsSuccess)
            {
                Console.WriteLine($"Get Property: Found - {result.Value.IdentNum}\n");
            }
            else
            {
                Console.WriteLine($"Get Property: {result.Error}");
            }
        }

        async Task UpdatePropertyExample()
        {
            var newPropertyData = new Property
            {
                IdentificationNumber = "ID123456",
                Address = "789 Oak St",
                YearOfConstruction = 2010,
                Type = PropertyType.Maisonet,
            };

            var result = await propertyService.UpdateProperty(new Property
            {
                Id = 1,
                IdentificationNumber = "ID123456",
                Address = "456 Elm St",
                YearOfConstruction = 2005,
                Type = PropertyType.DetachedHouse
            }, newPropertyData); // Assuming ID 1 exists
            Console.WriteLine($"Update Property: {(result.IsSuccess ? result.Value : result.Error)}\n");
        }

        async Task DeletePropertyExample()
        {
            var result = await propertyService.DeleteProperty(new Property
            {
                Id = 1,
                IdentificationNumber = "ID123456",
                Address = "456 Elm St",
                YearOfConstruction = 2005,
                Type = PropertyType.DetachedHouse
            }); // Assuming ID 1 exists
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
                    Type = RepairType.Painting,
                    DateTime = DateTime.Now,
                    Description = "Routine maintenance on plumbing",
                    Address = "456 Elm St",
                    Status = Status.Pending,
                    Cost = 250.75m,

                };


                // Test: Create Repair
                var createResult = await repairService.CreateRepair(repair, owner);
                Console.WriteLine(createResult.IsSuccess
                    ? $"Repair created: {createResult.Value.Description}\n"
                    : $"Failed to create repair: {createResult.Error}\n");
            }
            else
            {
                Console.WriteLine($"Owner not found");
            }
        }


        async Task GetRepairExample()
        {
            var repairId = 1;
            var getResult = await repairService.GetRepair(repairId);
            Console.WriteLine(getResult.IsSuccess
                ? $"Repair retrieved: {getResult.Value.Description}\n"
                : $"Failed to get repair: {getResult.Error}");
        }

        async Task UpdateRepairExample()
        {
            // ONLY FOR THE TEST CASE
            var repairRepository = host.Services.GetRequiredService<IRepairRepository>();
            var repairId = 1;
            var existingRepairResult = await repairRepository.GetRepair(repairId);
            if (existingRepairResult != null)
            {
                var repairToUpdate = existingRepairResult;
                repairToUpdate.Description = "Updated description";

                // Test: Update Repair
                var updateResult = await repairService.UpdateRepair(repairToUpdate, repairToUpdate);
                Console.WriteLine(updateResult.IsSuccess
                    ? $"Repair updated: {updateResult.Value.Description}\n"
                    : $"Failed to update repair: {updateResult.Error}");
            }
            else
            {
                Console.WriteLine($"Repair not found for update");
            }
        }
        
         async Task DeleteRepairExample()
        {
            var repairId = 1; 
            var repairToDeleteResult = await repairService.GetRepair(repairId);
            if (repairToDeleteResult.IsSuccess)
            {
                // Test: Delete Repair
                var deleteResult = await repairService.DeleteRepair(new Repair(){Id = 1});
                Console.WriteLine(deleteResult.IsSuccess ? 
                    "Repair deleted successfully." : 
                    $"Failed to delete repair: {deleteResult.Error}");
            }
            else
            {
                Console.WriteLine($"Repair not found for deletion: {repairToDeleteResult.Error}");
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
