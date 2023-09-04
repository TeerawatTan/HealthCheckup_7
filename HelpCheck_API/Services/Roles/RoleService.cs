using HelpCheck_API.Dtos;
using HelpCheck_API.Repositories.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.Roles
{
    public class RoleService : IRoleService
    {
        private IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<ResultResponse> GetRolesAsync()
        {
            var roles = await _roleRepository.GetRolesAsync();

            var getDropdownDto = roles.Select(s => new GetDropdownDto()
            {
                ID = s.ID,
                Name = s.RoleName
            });

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = getDropdownDto
            };
        }
    }
}
