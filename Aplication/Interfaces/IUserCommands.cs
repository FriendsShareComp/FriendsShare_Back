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
        public User FindUserByFieldAsync(string fieldName, object value, List<string> excludeFields);
        public bool ExistUserByFieldAsync(string fieldName, object value);
        public Task<bool> UpdateUserForFieldsById(string id, User user, List<string> fieldsToUpdate);
        public List<UserDto> GetFriendsByUser(string idUSer);
        public User GetUserById(string idUSer);
        public Task<bool> DeleteUser(string id);
    }
}
