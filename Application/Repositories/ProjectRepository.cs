using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Database;
using Application.Models.Entities;
using MongoDB.Driver;

namespace Application.Repositories
{
    public interface IProjectRepository
    {
        List<Project> Get();
        Project GetById(string id);
        Project GetByOwnerId(string ownerId);
        Project GetByFreelancerId(string freelancerId);
        Project Create(Project project);
        void Update(string id, Project projectIn);
        void Remove(Project projectIn);
        void Remove(string id);
        Task<List<Project>> GetAllFreelancerIsNull();
        Task<List<Project>> getByUser(string userId);
    }

    public class ProjectRepository : IProjectRepository
    {
        private readonly IDatabaseContext dbContext;
        private readonly IMongoCollection<Project> _projects;

        public ProjectRepository(IDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
            if (dbContext.IsConnectionOpen())_projects = dbContext.Projects;
        }

        public List<Project> Get()
        {
            return _projects.Find(project => true).ToList();
        }

        public Project GetById(string id)
        {
            return _projects.Find(project => project.Id == id).FirstOrDefault();
        }

        public Project GetByOwnerId(string ownerId)
        {
            return _projects.Find(project => project.OwnerId == ownerId).FirstOrDefault();
        }

        public Project GetByFreelancerId(string freelancerId)
        {
            return _projects.Find(project => project.FreelancerId == freelancerId).FirstOrDefault();
        }

        public Project Create(Project project)
        {
            _projects.InsertOne(project);
            return project;
        }

        public void Update(string id, Project projectIn)
        {
            _projects.ReplaceOne(project => project.Id == id, projectIn);
        }

        public void Remove(Project projectIn)
        {
            _projects.DeleteOne(project => project.Id == projectIn.Id);
        }

        public void Remove(string id)
        {
            _projects.DeleteOne(project => project.Id == id);
        }

        public async Task<List<Project>> GetAllFreelancerIsNull()
        {
            return await (await _projects.FindAsync(project => project.FreelancerId == null)).ToListAsync();
        }

        public async Task<List<Project>> getByUser(string userId) => await (await _projects.FindAsync(project => project.OwnerId == userId || project.FreelancerId == userId)).ToListAsync();

    }
}