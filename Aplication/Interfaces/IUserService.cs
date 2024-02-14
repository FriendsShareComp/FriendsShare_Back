using Aplication.Utils;
using Domain.Dto;

namespace Aplication.Interfaces
{
    public interface IUserService
    {
        Response UserRegister(UserDto user);
    }
}
