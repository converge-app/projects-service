using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Helpers;
using Application.Models.DataTransferObjects;
using Application.Models.Entities;
using Application.Repositories;
using Application.Services;
using Application.Utility;
using Application.Utility.Exception;
using Application.Utility.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService, IProjectRepository projectRepository, IMapper mapper)
        {
            _projectService = projectService;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectCreationDto projectDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                    {message = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)});

            var createProject = _mapper.Map<Project>(projectDto);
            try
            {
                var createdProject = await _projectService.Create(createProject);
                return Ok(createdProject);
            }
            catch (UserNotFound)
            {
                return NotFound(new MessageObj("User not found"));
            }
            catch (EnvironmentNotSet)
            {
                throw;
            }
            catch (Exception e)
            {
                return BadRequest(new MessageObj(e.Message));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var projects = _projectRepository.Get();
            var projectDtos = _mapper.Map<IList<ProjectDto>>(projects);
            return Ok(projectDtos);
        }

        [HttpGet("employer/{id}")]
        [AllowAnonymous]
        public IActionResult GetByOwnerId(string id)
        {
            var project = _projectRepository.GetByOwnerId(id);
            var projectDto = _mapper.Map<ProjectDto>(project);
            return Ok(projectDto);
        }

        [HttpGet("freelancer/{id}")]
        [AllowAnonymous]
        public IActionResult GetByFreelancerId(string id)
        {
            var project = _projectRepository.GetByFreelancerId(id);
            var projectDto = _mapper.Map<ProjectDto>(project);
            return Ok(projectDto);
        }

        [HttpGet("open")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOpen()
        {
            var projects = await _projectRepository.GetAllFreelancerIsNull();
            var projectDtos = _mapper.Map<IList<ProjectDto>>(projects);
            return Ok(projectDtos);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetById(string id)
        {
            var project = _projectRepository.GetById(id);
            var projectDto = _mapper.Map<ProjectDto>(project);
            return Ok(projectDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] string id, [FromBody] ProjectUpdateDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            project.Id = id;

            try
            {
                _projectService.Update(project);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new MessageObj(e.Message));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                _projectRepository.Remove(id);
            }
            catch (Exception e)
            {
                return BadRequest(new MessageObj(e.Message));
            }

            return Ok();
        }
    }
}