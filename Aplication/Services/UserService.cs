using Aplication.Interfaces;
using Aplication.Utils;
using AutoMapper;
using Domain.Dto;
using Domain.Models;
using Domain.Security;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;


namespace Aplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserCommands _userCommands;
        private readonly JwtAuthManager _jwtAuthManager;
        private readonly IMapper _mapper;
        private readonly DateTime _dateTime;
        private List<string> _excludeFields;
        public UserService(IUserCommands com, JwtAuthManager manager, IMapper mapper) 
        {
            _userCommands = com;
            _jwtAuthManager = manager;
            _mapper = mapper;
            _dateTime = DateTime.Now;
            _excludeFields = new List<string>() { "password"};
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

            bool existsUsername = _userCommands.ExistUserByFieldAsync("username", userDto.UserName);
            bool existEmail = _userCommands.ExistUserByFieldAsync("email", userDto.Email);
            if (existsUsername)
            {
                response.content = "ya existe un usuario con el username";
                response.succes = false;
                response.StatusCode = 400;
                return response;
            }
            if (existEmail)
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
        public Response DeleteUser(string idUser)
        {
            var response = new Response(true, "Se ha eliminado el usuario correctamente");
            response.StatusCode = 200;

            bool delete=_userCommands.DeleteUser(idUser).Result;

            if (!delete)
            {
                response.content = "error al eliminar el usuario"; 
                response.StatusCode = 500;
                response.succes=false;
            }

            return response;
        }
        public Response GetFriendsByUser(string idUser)
        {
            var response = new Response(true, "Friends");
            response.StatusCode = 200;

            List<UserDto> friends=_userCommands.GetFriendsByUser(idUser);

            response.objects=friends;
            return response;

        }

        public Response GetUserById(string idUser)
        {
            var response = new Response(true, "Friends");
            response.StatusCode = 200;

            User friends = _userCommands.FindUserByFieldAsync("_id",idUser,_excludeFields);

            response.objects = friends;
            return response;
        }

        public Response AddFriendsByUser(string idUserLogged, string idUserFriend)
        {
            var response = new Response(true, "Lista de amigos actualizada");
            response.StatusCode = 200;

            List<string> updateFields = new List<string> { "Friends" };
            User user1=_userCommands.FindUserByFieldAsync("_id", idUserLogged, _excludeFields);
            User user2 = _userCommands.FindUserByFieldAsync("_id", idUserFriend, _excludeFields);

            
            if(user2 == null) 
            {
                response.content = "el usuario a seguir no existe";
                response.StatusCode=404;
                response.succes=false; 
                return response;
            }

            user1.Friends.Add(_mapper.Map<UserDto>(user2));
            user2.Friends.Add(_mapper.Map<UserDto>(user1));



            response.succes = _userCommands.UpdateUserForFieldsById(user1._id, user1,updateFields).Result;
            response.succes = _userCommands.UpdateUserForFieldsById(user2._id, user2, updateFields).Result;

            if (!response.succes) response.content = "error al actualizar la lista de amigos";


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
        public Response UpdateUser(string idUser, UserUpdateDto user)
        {
            var response = new Response(true, "Usuario actualizado");
            response.StatusCode = 200;
            List<string> updateFields = new List<string> {  };
            var properties = typeof(UserUpdateDto).GetProperties();

            user.UpdateAt = DateTime.Now;
            // Iterar sobre cada propiedad y agregar su nombre a la lista
            foreach (var property in properties)
            {
                var value = property.GetValue(user);
                if(value!=null) updateFields.Add(property.Name);
            }

            User userUpdate=_mapper.Map<User>(user);
            if (updateFields.Count>1 && userUpdate!=null) response.succes = _userCommands.UpdateUserForFieldsById(idUser, userUpdate, updateFields).Result;

            return response;
        }
    }
}
