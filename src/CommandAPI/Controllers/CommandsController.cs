using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Data;
using CommandAPI.Models;
using AutoMapper;
using CommandAPI.Dtos;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandAPIRepo __repository;
        private readonly IMapper __mapper;
        public CommandsController(ICommandAPIRepo repository, IMapper mapper)
        {
            __repository = repository;
            __mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = __repository.GetAllCommands();
            return Ok(__mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        [HttpGet("{id}")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = __repository.GetCommandById(id);
            if (commandItem == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(__mapper.Map<CommandReadDto>(commandItem));
            }
        }
    }
}