using Application.Exceptions;
using Application.Models.Entities;
using Application.Repositories;
using Application.Services;
using Application.Utility.ClientLibrary;
using Moq;
using Xunit;

namespace ApplicationUnitTests
{
    public class ProjectServiceTest
    {
        [Fact]
        public void Create_ProjectIsNull_ThrowsInvalidProject()
        {
            // Arrange
            var projectRepository = new Mock<IProjectRepository>();
            var client = new Mock<IClient>();
            projectRepository.Setup(m => m.Create(It.IsAny<Project>())).Returns((Project)null);
            var projectService = new ProjectService(projectRepository.Object, client.Object);

            // Act
            // Assert
            Assert.ThrowsAsync<InvalidProject>(() => projectService.Create(new Project()));
        }

        [Fact]
        public void Update_ProjectIsNull_ThrowsInvalidProject()
        {
            // Arrange
            var projectRepository = new Mock<IProjectRepository>();
            var client = new Mock<IClient>();
            var projectService = new ProjectService(projectRepository.Object, client.Object);

            // Act
            // Assert
            Assert.Throws<InvalidProject>(() => projectService.Update((Project)null));
        }


        [Fact]
        public void Update_GetById_ReturnsUpdateProject()
        {
            // Arrange
            var projectRepository = new Mock<IProjectRepository>();
            var client = new Mock<IClient>();
            projectRepository.Setup(m => m.GetById(It.IsAny<string>())).Returns(new Project() { Id = "1123423523" });
            projectRepository.Setup(m => m.Update(It.IsAny<string>(), It.IsAny<Project>()));
            var projectService = new ProjectService(projectRepository.Object, client.Object);

            // Act
            projectService.Update(new Project() { Id = "1123423523" });
            // Assert

        }





    }
}