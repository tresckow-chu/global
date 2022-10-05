using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BPSGobalClient.Entities;

namespace BPSGobalClient.Services
{
    public interface IAuthenticationService
    {
        //Task<RegistrationResponseDto> RegisterUser(UserForRegistrationDto userForRegistration);
        Task<LoginResult> Login(LoginModel login); 
        Task Logout();
    }
}
