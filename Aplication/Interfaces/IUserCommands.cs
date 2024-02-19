using Aplication.Utils;
using Domain.Dto;
using Domain.Models;

namespace Aplication.Interfaces
{
    public interface IUserCommands
    {
        public Task<Response> CreateUser(User user);
        public Response SearchUserByCredentials(dtoLoginUser user);
        public bool UserExistByCredentials(string credential);
        public User FindUserByFieldAsync(string fieldName, object value);
        public List<UserDto> GetFriendsByUser(string idUSer);
        public User GetUserById(string idUSer);
    }
}
