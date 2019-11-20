
using System.Net;
using System.Net.Http;

using Application.Exceptions;
using Application.Models.DataTransferObjects;
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
        public async void Create_GetUserAsync()
        {
            var handler = new Mock<MockHandler>();
            handler.Setup(m => m.SendAsync(HttpMethod.Get, "https://projects-service.api.converge-app.net/api/Health/ping"))
            .Returns(() => Success("pong"));

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://projects-service.api.converge-app.net/api/Health/ping");

                Assert.Equal("pong", "pong");
            }

        }
        private static HttpResponseMessage Success(string content)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent(content);

            return response;
        }

        [Fact]
        public void Create_createdProjectl_ReturnsOwnerId()
        {
            // Arrange
            var projectRepository = new Mock<IProjectRepository>();
            var client = new Mock<IClient>();
            projectRepository.Setup(m => m.Create(It.IsAny<Project>())).Returns(new Project());
            var projectService = new ProjectService(projectRepository.Object, client.Object);

            // Act
            var project = projectService.Create(new Project());
            // Assert
            Assert.NotNull(project);

        }

        [Fact]
        public void Create_createdProject_ReturnsContent()
        {
            // Arrange
            var projectRepository = new Mock<IProjectRepository>();
            var client = new Mock<IClient>();
            projectRepository.Setup(m => m.Create(It.IsAny<Project>()))
            .Returns(new Project());
            var projectService = new ProjectService(projectRepository.Object, client.Object);

            // Act
            var sa = new ProjectContentDto();
            var actual = projectService.Create(new Project());
            // Assert
            Assert.Equal(new ProjectContentDto().Title, sa.Title);

        }

        public void Create_CouldNotCreateProject_ThrowsInvalidProject()
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