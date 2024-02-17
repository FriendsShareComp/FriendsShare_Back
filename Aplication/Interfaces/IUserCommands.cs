using Aplication.Utils;
using Domain.Dto;
using Domain.Models;

namespace Aplication.Interfaces
{
    public interface IUserCommands
    {
        public Task<Response> CreateUser(User user);
        public Response SearchUserByCredentials(dtoLoginUser user);
    }
}
