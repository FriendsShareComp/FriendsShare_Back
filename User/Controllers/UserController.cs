
using Aplication.Interfaces;
using Aplication.Utils;
using Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Usuario.Controllers
{
    [ApiController]
    [Route("/User")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userServices;
        //private readonly ILogger _logger;
        public UserController(IUserService use)
        {
            _userServices = use;
            //_logger = logger;
        }


        [HttpPost("/Register")]
        public async Task<IActionResult> UserRegister([FromBody] UserRegisterDto userDto)
        {
            //_logger.LogInformation(userDto.ToString());
            Response response = _userServices.UserRegister(userDto);

            if (!response.succes)
            {
                return new JsonResult(new { Error = response.content }) { StatusCode = response.StatusCode };
            }
            return new JsonResult(new { Message = response.content, Token = response.objects }) { StatusCode = response.StatusCode };
        }

        [HttpGet("/GetUserById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(string id)
        {
            //_logger.LogInformation(userDto.ToString());
            //string authenticatedUserId = User.Identity.Name; //id del usuario desde el token

            Response response = _userServices.GetUserById(id);

            if (!response.succes)
            {
                return new JsonResult(new { Error = response.content }) { StatusCode = response.StatusCode };
            }
            return new JsonResult(new { Data = response.objects }) { StatusCode = response.StatusCode };
        }
        [HttpGet("/GetFriends/{id}")]
        [Authorize]
        public async Task<IActionResult> GetFriends(string id)
        {
            //_logger.LogInformation(userDto.ToString());
            //string authenticatedUserId = User.Identity.Name; //id del usuario desde el token

            Response response = _userServices.GetFriendsByUser(id);

            if (!response.succes)
            {
                return new JsonResult(new { Error = response.content }) { StatusCode = response.StatusCode };
            }
            return new JsonResult(new { Data = response.objects }) { StatusCode = response.StatusCode };
        }

        [HttpGet("/AddFriend/{id}")]
        [Authorize]
        public async Task<IActionResult> AddFriends(string id)//id del usuario que voy a seguir
        {
            
            
            string userId = User.Identity.Name; //id del usuario desde el token

            Response response = _userServices.AddFriendsByUser(id, userId);

            if (!response.succes)
            {
                return new JsonResult(new { Error = response.content }) { StatusCode = response.StatusCode };
            }
            return new JsonResult(new { Data = response.objects }) { StatusCode = response.StatusCode };
        }

        [HttpDelete("/Delete")]
        [Authorize]
        public async Task<IActionResult> Delete()//id del usuario que voy a seguir
        {


            string userId = User.Identity.Name; //id del usuario desde el token

            Response response = _userServices.DeleteUser(userId);

            
            return new JsonResult(new { Data = response.content }) { StatusCode = response.StatusCode };
        }
    }
}
