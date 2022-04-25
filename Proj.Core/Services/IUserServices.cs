using Microsoft.AspNetCore.Identity;
using Proj.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Core.Services
{
    public interface IUserServices
    {
        public Task Login(LoginDTO user);
        public Task<IdentityResult> Register(RegisterDTO registerDto);

    }
}
