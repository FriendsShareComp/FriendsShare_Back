using Aplication.Utils;
using Domain.Models;

namespace Aplication.Interfaces
{
    public interface IUserCommands
    {
        public Task<Response> CreateUser(User user);
    }
}
