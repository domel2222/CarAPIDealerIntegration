using AutoMapper;
using CarDealerAPI.Contexts;
using CarDealerAPI.DTOS;
using CarDealerAPI.Extensions;
using CarDealerAPI.Models;
using CarDealerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.Controllers
{

    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class DealerController : ControllerBase
    {
        private readonly IDealerService _dealerService;

        public DealerController(IDealerService dealerService)
        {
            this._dealerService = dealerService;
        }

        [HttpGet]
        [AllowAnonymous]
        //[Authorize(Policy = "ColorEyes")]
        //[Authorize(Policy = "OnlyForEagles")]
        public ActionResult<IEnumerable<DealerReadDTO>> GetAllDealers([FromQuery] DealerQuerySearch query)
        {
            try
            {
            var dealersDTO = _dealerService.GetAllDealers(query);

            return Ok(dealersDTO);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database fail");
            }

        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        //[Authorize(Policy = "HasNation")]
        public ActionResult<DealerReadDTO> GetOneDealer(int id)
        {
            var dealer = _dealerService.GetDealerById(id);

            return Ok(dealer);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator,Dealer Manager")]
        //[Authorize(Roles = "Dealer Manager")] // calim role must have in JWT

        public ActionResult CreateDealer(DealerCreateDTO createDto)
        {
            //use []  from client and iterate by them ..... :)
            // hot to itterate for DTO  form body
            //var userId =   int.Parse(User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value);
            //foreach (var item in createDto)
            var id = _dealerService.CreateDealer(createDto);

            return Created($"api/Dealer/{id}", null);

        }

        [HttpDelete("{id}")]
        public ActionResult DeleteDealer(int id)
        {
            //_dealerService.DeleteDealer(id, User);
            _dealerService.DeleteDealer(id);

            return NoContent();
        }
        [HttpPut("{id}")]
        public ActionResult UpdateDealer(DealerUpdateDTO dto, int id)
        {
            //_dealerService.UpdateDealer(dto, id, User);
            _dealerService.UpdateDealer(dto, id);

            return Ok();
        }
    }
}
