using Microsoft.AspNetCore.Identity;
using PaparaApp.Project.API.Models.Users;
using PaparaApp.Project.API.Models.Users.ApartmanManagers;

namespace PaparaApp.Project.API.Seeders
{
    public class ManagerDataSeeder
    {
            public static async Task SeedManagerUserAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
            {
                const string managerRoleName = "Manager";
                const string managerUsername = "manager@example.com";
                const string managerPassword = "Manager123!";
                const string managerName = "Manager";

                if (!await roleManager.RoleExistsAsync(managerRoleName))
                {
                    await roleManager.CreateAsync(new AppRole { Name = managerRoleName });
                }

                var managerUser = await userManager.FindByNameAsync(managerUsername);
                if (managerUser == null)
                {
                    managerUser = new ApartmanManagerUser
                    {
                        UserName = managerUsername,
                        Email = managerUsername,
                    };
                    var result = await userManager.CreateAsync(managerUser, managerPassword);
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create the manager user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }

                    await userManager.AddToRoleAsync(managerUser, managerRoleName);
                }
            }

        }
    }