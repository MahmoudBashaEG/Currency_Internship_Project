using Microsoft.AspNetCore.Identity;
using Proj.Core.DTOs;
using Proj.Core.Exceptions;
using Proj.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public UserServices(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }
        public async Task Login(LoginDTO loginDTO)
        {

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if(user == null)
            {
                throw new NotFoundException($"The User With This Email:{loginDTO.Email} Not Founded");
            }

            var isPasswordRight = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!isPasswordRight)
            {
                throw new WrongPasswordException();
            }

           
        }

        public  async Task<IdentityResult> Register(RegisterDTO registerDto)
        {
            var newUser = new IdentityUser() { UserName = registerDto.UserName, Email = registerDto.Email, PasswordHash = registerDto.Password };

            var isRegisteredWithEmailBefore = await _userManager.FindByEmailAsync(newUser.Email);

           

            if (isRegisteredWithEmailBefore != null)
            {
                throw new RepeatedException("This Email is Registered Before");
            }
           return await _userManager.CreateAsync(newUser,registerDto.Password);
        }
    }
}
