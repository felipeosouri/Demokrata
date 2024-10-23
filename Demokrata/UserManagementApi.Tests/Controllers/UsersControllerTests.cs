using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementApi.Controllers;
using UserManagementApi.Data;
using UserManagementApi.Models;
using UserManagementApi.Models.DTO;

namespace UserManagementApi.Tests.Controllers
{
    public class UsersControllerTests
    {

        private UserContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + Guid.NewGuid())
                .Options;
            return new UserContext(options);
        }


        [Fact]
        public async Task GetAll_ReturnsUsers()
        {
            // Arrange
            var context = GetInMemoryContext();
            context.Users.Add(new User { PrimerNombre = "Jaime", PrimerApellido = "Diaz", FechaNacimiento = new DateTime(1990, 1, 1), Salario = 1000000 });
            context.Users.Add(new User { PrimerNombre = "Yaneth", PrimerApellido = "Zuluaga", FechaNacimiento = new DateTime(1985, 5, 10), Salario = 1500000 });
            await context.SaveChangesAsync();

            var controller = new UsersController(context);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var users = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public async Task GetById_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var context = GetInMemoryContext();
            context.Users.Add(new User { PrimerNombre = "Jaime", PrimerApellido = "Diaz", FechaNacimiento = new DateTime(1990, 1, 1), Salario = 1000000 });
            await context.SaveChangesAsync();

            var controller = new UsersController(context);

            // Act
            var result = await controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsType<User>(okResult.Value);
            Assert.Equal("Jaime", user.PrimerNombre);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryContext();
            var controller = new UsersController(context);

            // Act
            var result = await controller.GetById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var context = GetInMemoryContext();
            var controller = new UsersController(context);

            var newUser = new UserDto
            {
                PrimerNombre = "Luciana",
                PrimerApellido = "Mejia",
                FechaNacimiento = new System.DateTime(1992, 3, 15),
                Salario = 12000000
            };

            // Act
            var result = await controller.Create(newUser);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdUser = Assert.IsType<User>(createdAtActionResult.Value);
            Assert.Equal("Luciana", createdUser.PrimerNombre);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenUserExists()
        {
            // Arrange
            var context = GetInMemoryContext();
            context.Users.Add(new User { PrimerNombre = "Jaime", PrimerApellido = "Diaz", FechaNacimiento = new DateTime(1990, 1, 1), Salario = 1000000 });
            await context.SaveChangesAsync();

            var controller = new UsersController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryContext();
            var controller = new UsersController(context);

            // Act
            var result = await controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
