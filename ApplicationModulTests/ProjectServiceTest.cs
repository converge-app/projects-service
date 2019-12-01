using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Application;
using Application.Models.DataTransferObjects;
using Application.Models.Entities;
using Application.Utility.ClientLibrary.Project;
using Application.Utility.Models;
using Application.Utility.TestUtility;
using ApplicationModulTests.TestUtility;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace ApplicationIntegerationsTests
{
    public class ProjectServiceTest : IClassFixture<WebApplicationFactory<StartupDevelopment>>
    {

        private readonly WebApplicationFactory<StartupDevelopment> _factory;

        public ProjectServiceTest(WebApplicationFactory<StartupDevelopment> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_Health_ping()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.GetAsync("/api/health/ping");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GEt_Health_ping()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/health/ping");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var actual = JsonConvert.DeserializeObject<MessageObj>(await response.Content.ReadAsStringAsync());
            Assert.Equal("pong!", actual.Message);
        }

        [Fact]
        public async Task Get_Projects()
        {
            // Arrange
            Environment.SetEnvironmentVariable("USERS_SERVICE_HTTP", "users-service.api.converge-app.net");

            var client = _factory.CreateClient();

            var httpClient = new HttpClient();
            var authUser = await AuthUtility.GenerateAndAuthenticate(httpClient);

            AuthUtility.AddAuthorization(client, authUser.Token);
            // Act
            var response = await client.GetAsync("/api/Projects");

            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
        }


        [Fact]
        public async Task Post_Projects()
        {
            // Arrange
            Environment.SetEnvironmentVariable("USERS_SERVICE_HTTP", "users-service.api.converge-app.net");

            var client = _factory.CreateClient();

            var httpClient = new HttpClient();
            var authUser = await AuthUtility.GenerateAndAuthenticate(httpClient);
            AuthUtility.AddAuthorization(client, authUser.Token);

            var project = ProjectUtility.GenerateProject(authUser.Id);
            var pro = await ProjectUtility.CreateProject(client, project);


            // Act
            //Assert

            Assert.NotNull(pro);

        }

        [Fact]
        public async Task Get_Projects_ByOwnerId()
        {
            // Arrange
            Environment.SetEnvironmentVariable("USERS_SERVICE_HTTP", "users-service.api.converge-app.net");

            var client = _factory.CreateClient();

            var httpClient = new HttpClient();
            var authUser = await AuthUtility.GenerateAndAuthenticate(httpClient);
            AuthUtility.AddAuthorization(client, authUser.Token);

            var project = ProjectUtility.GenerateProject(authUser.Id);
            var create = await ProjectUtility.CreateProject(client, project);


            // Act
            //Assert
            var response = await client.GetAsync("/api/Projects/employer/" + create.OwnerId);

            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Project_ByUserId()
        {
            // Arrange
            Environment.SetEnvironmentVariable("USERS_SERVICE_HTTP", "users-service.api.converge-app.net");

            var client = _factory.CreateClient();

            var httpClient = new HttpClient();
            var authUser = await AuthUtility.GenerateAndAuthenticate(httpClient);
            AuthUtility.AddAuthorization(client, authUser.Token);

            var project = ProjectUtility.GenerateProject(authUser.Id);
            await ProjectUtility.CreateProject(client, project);


            // Act
            //Assert
            var response = await client.GetAsync("/api/Projects/user/" + authUser.Id);

            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Project_ByProjectId()
        {
            // Arrange
            Environment.SetEnvironmentVariable("USERS_SERVICE_HTTP", "users-service.api.converge-app.net");

            var client = _factory.CreateClient();

            var httpClient = new HttpClient();
            var authUser = await AuthUtility.GenerateAndAuthenticate(httpClient);
            AuthUtility.AddAuthorization(client, authUser.Token);

            var project = ProjectUtility.GenerateProject(authUser.Id);
            var create = await ProjectUtility.CreateProject(client, project);


            // Act
            //Assert
            var response = await client.GetAsync("/api/Projects/" + create.Id);

            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
        }


        [Fact]
        public async Task Update_Project_ByProjectId()
        {
            // Arrange
            Environment.SetEnvironmentVariable("USERS_SERVICE_HTTP", "users-service.api.converge-app.net");

            var client = _factory.CreateClient();

            var httpClient = new HttpClient();
            var authUser = await AuthUtility.GenerateAndAuthenticate(httpClient);
            AuthUtility.AddAuthorization(client, authUser.Token);

            var project = ProjectUtility.GenerateProject(authUser.Id);
            var create = await ProjectUtility.CreateProject(client, project);

            ProjectUpdateDto update = new ProjectUpdateDto
            {
                Id = create.Id,
                OwnerId = project.OwnerId,
                ProjectContent = new ProjectContentDto
                {
                    Title = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                    Category = Guid.NewGuid().ToString(),
                    SubCategory = Guid.NewGuid().ToString(),
                    Amount = 1000,
                }

            };


            // Act
            //Assert
            var response = await client.PutAsJsonAsync("/api/Projects/" + create.Id, update);
            Assert.NotNull(update);
        }

        [Fact]
        public async Task Delete_Project_ByProjectId()
        {
            // Arrange
            Environment.SetEnvironmentVariable("USERS_SERVICE_HTTP", "users-service.api.converge-app.net");

            var client = _factory.CreateClient();

            var httpClient = new HttpClient();
            var authUser = await AuthUtility.GenerateAndAuthenticate(httpClient);
            AuthUtility.AddAuthorization(client, authUser.Token);

            var project = ProjectUtility.GenerateProject(authUser.Id);
            var create = await ProjectUtility.CreateProject(client, project);


            // Act
            //Assert
            var response = await client.DeleteAsync("/api/Projects/" + create.Id);

            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
        }



    }
}