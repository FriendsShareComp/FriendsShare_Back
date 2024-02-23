using Aplication.Interfaces;
using Aplication.Utils;
using Domain.Dto;
using Domain.Models;
using Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class AuthServices : IAuthService
    {
        private readonly JwtAuthManager _jwtAuthManager;
        private readonly IUserCommands _userCommands;
        public AuthServices(JwtAuthManager jwtAuthManager, IUserCommands userCommands) 
        {
            _jwtAuthManager = jwtAuthManager;
            _userCommands = userCommands;
        }

        public Response GetLoginUser(dtoLoginUser userDto)
        {
            Response response = new Response(true, "Usuario Encontrado");
            response.StatusCode = 200;
            userDto.password=Encrypt.encryption(userDto.password);

            response = _userCommands.SearchUserByCredentials(userDto);

            if (response.objects==null)
            {
                response.succes = false;
                response.content = "Los datos ingresados no pertenecen a ningun usuario activo, vualva a intentarlo";
                response.StatusCode = 400;
                return response;
            }
            var token = _jwtAuthManager.Authenticate((User)response.objects);
            response.objects = token;
            return response;
        }
    }
}
