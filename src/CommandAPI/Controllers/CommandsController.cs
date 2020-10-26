using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Data;
using CommandAPI.Models;
using AutoMapper;
using CommandAPI.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        //  Random change
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

        [HttpGet("{id}", Name = "GetCommandById")]
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

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = __mapper.Map<Command>(commandCreateDto);
            __repository.CreateCommand(commandModel);
            __repository.SaveChanges();

            var commandReadDto = __mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id}, commandReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = __repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }
            __mapper.Map(commandUpdateDto, commandModelFromRepo);
            __repository.UpdateCommand(commandModelFromRepo);
            __repository.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelFromRepo = __repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            var commandToPatch = __mapper.Map<CommandUpdateDto>(commandModelFromRepo);
            patchDoc.ApplyTo(commandToPatch, ModelState);

            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            __mapper.Map(commandToPatch, commandModelFromRepo);

            __repository.UpdateCommand(commandModelFromRepo);
            __repository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = __repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            __repository.DeleteCommand(commandModelFromRepo);
            __repository.SaveChanges();

            return NoContent();
        }
    }
}