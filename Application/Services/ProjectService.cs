using System;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Models.DataTransferObjects;
using Application.Models.Entities;
using Application.Repositories;
using Application.Utility.ClientLibrary;
using Application.Utility.Exception;
using Application.Utility.Models;

namespace Application.Services
{
    public interface IProjectService
    {
        Task<Project> Create(Project project);
        void Update(Project projectParam);
    }

    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IClient _client;

        public ProjectService(IProjectRepository projectRepository, IClient client)
        {
            _projectRepository = projectRepository;
            _client = client;
        }

        public async Task<Project> Create(Project project)
        {
            if (project == null)
                throw new InvalidProject();
            // Find if owner exists
            var user = await _client.GetUserAsync(project.OwnerId);
            if (user != null)
            {
                var createdProject = _projectRepository.Create(project);
                if (createdProject != null)
                    return createdProject;
                throw new InvalidProject("Could not create project");
            }

            throw new UserNotFound();
        }


        public void Update(Project project)
        {
            if (project == null)
                throw new InvalidProject();

            if (_projectRepository.GetById(project.Id) != null) ;
            {
                _projectRepository.Update(project.Id, project);
            }
        }
    }
}