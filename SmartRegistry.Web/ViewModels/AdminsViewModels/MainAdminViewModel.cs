using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SmartRegistry.Web.ViewModels.AdminsViewModels
{
    public class MainAdminViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        IEnumerable<IdentityRole> UserRoles { get; set; }
    }
}
