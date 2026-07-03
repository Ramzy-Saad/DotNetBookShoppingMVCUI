using BookShoppingMVCUI.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookShoppingMVCUI.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();
            var dbContext = service.GetRequiredService<ApplicationDbContext>();


            await UpsertOrderStatus(dbContext, "Pending");
            await UpsertOrderStatus(dbContext, "Shipped");
            await UpsertOrderStatus(dbContext, "Delivered");
            await UpsertOrderStatus(dbContext, "Cancelled");

            await dbContext.SaveChangesAsync();


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
        private static async Task UpsertOrderStatus(ApplicationDbContext dbContext, string name)
        {
            var status = await dbContext.orderStatuses
                .FirstOrDefaultAsync(s => s.Name == name);

            if (status == null)
            {
                dbContext.orderStatuses.Add(new OrderStatus
                {
                    Name = name,
                    StatusID = name switch
                    {
                        "Pending" => 1,
                        "Shipped" => 2,
                        "Delivered" => 3,
                        "Cancelled" => 4,
                        _ => throw new ArgumentException($"Invalid order status name: {name}")
                    }
                });
            }
            else
            {
                // Update any fields if needed
                status.Name = name;
            }
        }
    }
}
