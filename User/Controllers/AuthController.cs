using Aplication.Interfaces;
using Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace User.Controllers
{
    [ApiController]
    //[Route("/User")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult LoginUser(dtoLoginUser loginUser)
        {
            try
            {
                var ClientExist = _authService.GetLoginUser(loginUser);
                if (!ClientExist.succes)
                {
                    return new JsonResult(new { Error = ClientExist.content }) { StatusCode = ClientExist.StatusCode };
                }
                return new JsonResult(new { Token = ClientExist.objects }) { StatusCode = ClientExist.StatusCode };
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }
    }
}
