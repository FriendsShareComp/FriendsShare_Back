
using Aplication.Interfaces;
using Aplication.Utils;
using Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Usuario.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userServices;

        public UserController(IUserService use)
        {
            _userServices = use;
        }


        [HttpPost("UserRegister")]
        public async Task<IActionResult> UserRegister([FromBody] UserDto userDto)
        {
            Response response = _userServices.UserRegister(userDto);

            if (!response.succes)
            {
                return new JsonResult(new { Error = response.content }) { StatusCode = response.StatusCode };
            }
            return new JsonResult(new { Message = response.content, Token = response.objects }) { StatusCode = response.StatusCode };
        }

    }
}
