using System;
using System.Collections.Generic;
using System.Linq;
using Application.Helpers;
using Application.Models.DataTransferObjects;
using Application.Models.Entities;
using Application.Repositories;
using Application.Services;
using Application.Utility;
using Application.Utility.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Application.Controllers {
    [Route ("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase {
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectService _projectService;
        private AppSettings _appSettings;

        public ProjectsController (IProjectService projectService, IProjectRepository projectRepository, IMapper mapper,
            IOptions<AppSettings> appSettings) {
            _projectService = projectService;
            _projectRepository = projectRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateProject ([FromBody] ProjectCreationDto projectDto) {
            if (!ModelState.IsValid)
                return BadRequest (new { message = ModelState.Values.SelectMany (v => v.Errors).Select (e => e.ErrorMessage) });

            var createProject = _mapper.Map<Project> (projectDto);
            try {
                var createdProject = _projectService.Create (createProject);
                return Ok (createdProject);
            } catch (Exception e) {
                if (e.Message == "Email is already taken")
                    return BadRequest (new { message = "Email is already taken" });
                return BadRequest (new { message = e.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll () {
            var projects = _projectRepository.Get ();
            var projectDtos = _mapper.Map<IList<ProjectDto>> (projects);
            return Ok (projectDtos);
        }

        [HttpGet ("{id}")]
        [AllowAnonymous]
        public IActionResult GetById (string id) {
            var project = _projectRepository.GetById (id);
            var projectDto = _mapper.Map<ProjectDto> (project);
            return Ok (projectDto);
        }

        [HttpGet ("email/{email}")]
        [AllowAnonymous]
        public IActionResult GetByEmail (string email) {
            var project = _projectRepository.GetByEmail (email);
            var projectDto = _mapper.Map<ProjectDto> (project);
            return Ok (projectDto);
        }

        [HttpPut ("{id}")]
        public IActionResult Update ([FromRoute] string id, [FromBody] ProjectUpdateDto projectDto) {
            var project = _mapper.Map<Project> (projectDto);
            project.Id = id;

            try {
                _projectService.Update (project);
                return Ok ();
            } catch (Exception e) {
                return BadRequest (new { e.Message });
            }
        }

        [HttpDelete ("{id}")]
        public IActionResult Delete (string id) {
            _projectRepository.Remove (id);
            return Ok ();
        }
    }
}