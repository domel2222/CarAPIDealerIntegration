using CarDealerAPI.DTOS;
using CarDealerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpPost("register")]
        public ActionResult RegisterUser ([FromBody] UserCreateDTO userDto)
        {
            
            _accountService.RegisterUser(userDto);

            return Ok();
        }

        [HttpPost("login")]
        public ActionResult LoginUser(UserLoginDTO login)
        {
            string token = _accountService.GenerateToken(login);

            return Ok(token);
        }
    }
}
