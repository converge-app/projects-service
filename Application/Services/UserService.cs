using System;
using Application.Models.Entities;
using Application.Repositories;

namespace Application.Services {
    public interface IProjectService {
        Project Create (Project project);
        void Update (Project projectParam);
    }

    public class ProjectService : IProjectService {
        private readonly IProjectRepository _projectRepository;

        public ProjectService (IProjectRepository projectRepository) {
            _projectRepository = projectRepository;
        }

        public Project Create (Project project) {
            if (_projectRepository.GetByEmail (project.Email) != null)
                throw new Exception ("Email is already taken");

            return _projectRepository.Create (project);
        }

        public void Update (Project projectParam) {
            var project = _projectRepository.GetById (projectParam.Id) ??
                throw new ArgumentNullException ("_projectRepository.GetById(projectParam.Id)");

            if (projectParam.Email != project.Email)
                if (_projectRepository.GetByEmail (projectParam.Email) != null)
                    throw new Exception ("Email was already taken");

            project.FirstName = projectParam.FirstName;
            project.LastName = projectParam.LastName;
            project.Email = projectParam.Email;

            _projectRepository.Update (project.Id, project);
        }
    }
}