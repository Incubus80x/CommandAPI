using System;
using Xunit;
using CommandAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoMapper;
using CommandAPI.Data;
using CommandAPI.Models;
using System.Collections.Generic;
using CommandAPI.Profiles;
using CommandAPI.Dtos;

namespace CommandAPI.Test
{
    public class CommandsControllerTests : IDisposable
    {
        Mock<ICommandAPIRepo> mockRepo;
        CommandProfile realProfile;
        MapperConfiguration configuration;
        IMapper mapper;

#region ctor
        public CommandsControllerTests()
        {
            mockRepo = new Mock<ICommandAPIRepo>();
            realProfile = new CommandProfile();
            configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            mapper = new Mapper(configuration);
        }

        public void Dispose()
        {
            mockRepo = null;
            mapper = null;
            configuration = null;
            realProfile = null;
        }
#endregion

#region all commands
        [Fact]
        public void GetAllCommands_Returns200Ok_WhenDbIsEmpty()
        {
            //  Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(0));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //  Act
            var result = controller.GetAllCommands();

            //  Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllCommands_ReturnsOneItem_WhenDbHasOneResource()
        {
            //  Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //  Act
            var result = controller.GetAllCommands();

            //  Assert
            var okResult = result.Result as OkObjectResult;

            var commands = okResult.Value as List<CommandReadDto>;

            Assert.Single(commands);
        }

        [Fact]
        public void GetAllComands_Returns200Ok_WhenDbHasOneResource()
        {
            //  Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //  Act
            var result = controller.GetAllCommands();

            //  Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllCommands_ReturnsCorrectType_WhenDbHasOneResource()
        {
            //  Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //  Act
            var result = controller.GetAllCommands();

            //  Assert
            Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(result);
        }
#endregion

#region command by id
        [Fact]
        public void GetCommandById_Returns404NotFound_WhenNonExistentIDProvided()
        {
            //  Arrange
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //  Act
            var result = controller.GetCommandById(1);

            //  Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetCommandById_Returns200Ok_WhenValidIDProvided()
        {
            //  Arrange
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command {
                Id = 1,
                HowTo = "test",
                CommandLine = "Test",
                Platform = "Test"
            });

            var controller = new CommandsController(mockRepo.Object, mapper);

            //  Act
            var result = controller.GetCommandById(1);

            //  Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCommandById_ReturnsCorrectType_WhenValidIDProvided()
        {
            //  Arrange
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command {
                Id = 1,
                HowTo = "test",
                CommandLine = "Test",
                Platform = "Test"
            });

            var controller = new CommandsController(mockRepo.Object, mapper);

            //  Act
            var result = controller.GetCommandById(1);

            //  Assert
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }
#endregion

#region create command
        [Fact]
        public void CreateCommand_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
        {
            //  Arrange
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command {
                Id = 1,
                HowTo = "test",
                CommandLine = "Test",
                Platform = "Test"
            });

            var controller = new CommandsController(mockRepo.Object, mapper);

            //  Act
            var result = controller.CreateCommand(new CommandCreateDto{});

            //  Assert
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        [Fact]
        public void CreateCommand_Returns201Created_WhenValidObjectSubmitted()
        {
            //  Arrange
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command {
                Id = 1,
                HowTo = "test",
                CommandLine = "Test",
                Platform = "Test"
            });

            var controller = new CommandsController(mockRepo.Object, mapper);

            //  Act
            var result = controller.CreateCommand(new CommandCreateDto{});

            //  Assert
            Assert.IsType<CreatedAtRouteResult>(result.Result);
        }
#endregion

#region command update
        [Fact]
        public void UpdateCommand_Returns204NoContent_WhenValidObjectSubmitted()
        {
            //  Arrange
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command {
                Id = 1,
                HowTo = "test",
                CommandLine = "Test",
                Platform = "Test"
            });

            var controller = new CommandsController(mockRepo.Object, mapper);

            //  Act
            var result = controller.UpdateCommand(1, new CommandUpdateDto{});

            //  Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateCommand_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //  Arrange
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //  Act
            var result = controller.UpdateCommand(0, new CommandUpdateDto{});

            //  Assert
            Assert.IsType<NotFoundResult>(result);
        }
#endregion

#region command partial update
        [Fact]
        public void PartialCommandUpdate_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //  Arrange
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //  Act
            var result = controller.PartialCommandUpdate(0, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<CommandUpdateDto>{});

            //  Assert
            Assert.IsType<NotFoundResult>(result);
        }
#endregion

#region command delete
        [Fact]
        public void DeleteCommand_Returns204NoContect_WhenValidResourceIDSubmitted()
        {
            //  Arrange
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command {
                Id = 1,
                HowTo = "test",
                CommandLine = "Test",
                Platform = "Test"
            });

            var controller = new CommandsController(mockRepo.Object, mapper);

            //  Act
            var result = controller.DeleteCommand(1);

            //  Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCommand_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //  Arrange
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //  Act
            var result = controller.DeleteCommand(0);

            //  Assert
            Assert.IsType<NotFoundResult>(result);
        }
#endregion

#region supported methods
        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if (num > 0)
            {
                commands.Add(new Command()
                {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            }
            return commands;
        }
#endregion
    
    }
}