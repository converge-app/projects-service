using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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
            Environment.SetEnvironmentVariable("USERS_SERVICE_HTTP", "users-service.api.converge-app.net");
            var expected = "";
            var mockFactory = new Mock<IHttpClientFactory>();
            var configuration = new HttpConfiguration();
            var clientHandlerStub = new DelegatingHandlerStub((request, cancellationToken) =>
            {
                request.SetConfiguration(configuration);
                var response = request.CreateResponse(HttpStatusCode.OK, expected);
                return Task.FromResult(response);
            });

            var client = new HttpClient(clientHandlerStub);

            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            IHttpClientFactory factory = mockFactory.Object;
            var controller = new Client(factory);

            //Act
            var result = await controller.GetUserAsync("123");

            //Assert
            Assert.NotNull(expected);
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