using Microsoft.AspNetCore.Identity;

namespace SmartRegistry.Web.Data.Configuration
{
    public class UserRoleSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleSeeder(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }


        public async void Seed()
        {
            if ((await _roleManager.FindByNameAsync("Admin")) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }
        }
    }
}
