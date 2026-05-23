using BookShoppingMVCUI.Constants;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BookShoppingMVCUI.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();

            // adding roles to DB
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));
            // Create admin account
            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };
            var userInDb = await userMgr.FindByEmailAsync(admin.Email);

            if (userInDb == null)
            {
                var result = await userMgr.CreateAsync(admin, "Admin@123");

                if (result.Succeeded)
                {
                    var createdUser = await userMgr.FindByEmailAsync(admin.Email);

                    await userMgr.AddToRoleAsync(createdUser, Roles.Admin.ToString());
                }else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
            }
        }
    }
}
