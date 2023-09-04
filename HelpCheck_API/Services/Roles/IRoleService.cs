using HelpCheck_API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.Roles
{
    public interface IRoleService
    {
        Task<ResultResponse> GetRolesAsync();
    }
}
