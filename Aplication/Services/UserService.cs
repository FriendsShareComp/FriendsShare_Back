using Aplication.Interfaces;
using Aplication.Utils;
using AutoMapper;
using Domain.Dto;
using Domain.Models;
using Domain.Security;
using System.Text.RegularExpressions;


namespace Aplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserCommands _userCommands;
        private readonly JwtAuthManager _jwtAuthManager;
        private readonly IMapper _mapper;
        private readonly DateTime _dateTime;
        public UserService(IUserCommands com, JwtAuthManager manager, IMapper mapper) 
        {
            _userCommands = com;
            _jwtAuthManager = manager;
            _mapper = mapper;
            _dateTime = DateTime.Now;
        }

        public Response UserRegister(UserRegisterDto userDto)
        {
            User user;
            
            var response = new Response(true, "Se ha creado el usuario correctamente");
            response.StatusCode = 200;

            var fieldsvalidator = this.ValidateFieldsRegister(userDto);
            if (!fieldsvalidator.succes)
            {
                response.content = fieldsvalidator.content;
                response.succes = false;
                response.StatusCode = fieldsvalidator.StatusCode;
                return response;
            }


            //var existsUsername = _userCommands.UserExistByCredentials(userDto.UserName);
            //var existsEmail = _userCommands.UserExistByCredentials(userDto.Email);
            string existsUsername = _userCommands.FindUserByFieldAsync("username", userDto.UserName).UserName.ToString();
            string existEmail = _userCommands.FindUserByFieldAsync("email", userDto.Email).Email.ToString();
            if (existsUsername!="")
            {
                response.content = "ya existe un usuario con el username";
                response.succes = false;
                response.StatusCode = 400;
                return response;
            }
            if (existEmail != "")
            {
                response.content = "ya existe un usuario con el email";
                response.succes = false;
                response.StatusCode = 400;
                return response;
            }



            user = _mapper.Map<User>(userDto);
            user.Password = Encrypt.encryption(user.Password);
            user.CreatedAt= _dateTime;
            user.Active = 1;
            response = _userCommands.CreateUser(user).Result;

            var token = _jwtAuthManager.Authenticate((User)response.objects);
            response.objects = token;
            
            return response;

        }
        public Response GetFriendsByUser(string idUser)
        {
            var response = new Response(true, "Se ha creado el usuario correctamente");
            response.StatusCode = 200;

            List<UserDto> friends=_userCommands.GetFriendsByUser(idUser);

            response.objects=friends;
            return response;

        }
        public Response AddFriendsByUser(string idUserLogged, string idUserFriend)
        {
            User user=_userCommands.GetUserById(idUserLogged);
            var response = new Response(true, "Se ha creado el usuario correctamente");

            response.StatusCode = 200;
            return response;
        }
        private Response ValidateFieldsRegister(UserRegisterDto user)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            var response = new Response(true, "La informacion ingresada se verifico correctamente");
            if (user.UserName == "" || user.UserName == null)
            {
                response.content = "El campo de UserName ingresado es nulo o se encuentra vacio";
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
            if(user.Email!=null && user.Email.Length>0)
            {
                bool validate=Regex.IsMatch(user.Email, pattern);
                if (!validate)
                {
                    response.content = "El campo de Email ingresado no es valido";
                    response.succes = false;
                    response.StatusCode = 400;
                    return response;
                }
            }
            return response;
        }
    }
}
