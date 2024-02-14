using Aplication.Interfaces;
using Aplication.Utils;
using AutoMapper;
using Domain.Dto;
using Domain.Models;
using System.Collections;
using System.Collections.Generic;


namespace Aplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserCommands _userCommands;
        private readonly JwtAuthManager _jwtAuthManager;
        private readonly IMapper _mapper;
        public UserService(IUserCommands com, JwtAuthManager manager, IMapper mapper) 
        {
            _userCommands = com;
            _jwtAuthManager = manager;
            _mapper = mapper;
        }

        public Response UserRegister(UserDto userDto)
        {
            User user;
            var response = new Response(true, "Se ha creado el usuario correctamente");
            response.StatusCode = 200;

            var fieldsvalidator = this.ValidateFields(userDto);
            if (!fieldsvalidator.succes)
            {
                response.content = fieldsvalidator.content;
                response.succes = false;
                response.StatusCode = fieldsvalidator.StatusCode;
                return response;
            }

            user = _mapper.Map<User>(userDto);

            response = _userCommands.CreateUser(user).Result;

            var token = _jwtAuthManager.Authenticate((User)response.objects);
            response.objects = token;
            
            return response;

        }
        private Response ValidateFields(UserDto user)
        {
            var response = new Response(true, "La informacion ingresada se verifico correctamente");
            if (user.Name == "" || user.Name == null)
            {
                response.content = "El campo de Nombre ingresado es nulo o se encuentra vacio";
                response.succes = false;
                response.StatusCode = 400;
                return response;
            }
            if (user.LastName == "" || user.LastName == null)
            {
                response.content = "El campo de Apellido ingresado es nulo o se encuentra vacio";
                response.succes = false;
                response.StatusCode = 400;
                return response;
            }
            if (user.Email == "" || user.Email == null)
            {
                response.content = "El campo de Email ingresado es nulo o se encuentra vacio";
                response.succes = false;
                response.StatusCode = 400;
                return response;
            }
            if (user.Password == "" || user.Password == null)
            {
                response.content = "El campo Contraseña no puede contener digitos no numericos";
                response.succes = false;
                response.StatusCode = 400;
                return response;
            }
            return response;
        }
    }
}
