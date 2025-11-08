using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.RolesModule.Services
{
    public interface IRoleService
    {

        IEnumerable<string> GetRoles();

    }

    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IEnumerable<string> GetRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return roles;
        }
    }
}
