using Aplication.Utils;
using Domain.Dto;

namespace Aplication.Interfaces
{
    public interface IUserService
    {
        Response UserRegister(UserRegisterDto user);
        public Response GetUserById(string idUser);
        public Response GetFriendsByUser(string idUser);
        public Response AddFriendsByUser(string idUserLogged, string idUserFriend);
        public Response DeleteUser(string idUser);
    }
}
