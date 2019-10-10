using System.Collections.Generic;
using Application.Models.DataTransferObjects;
using Application.Models.Entities;
using AutoMapper;

namespace Application.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();
            CreateMap<ProjectUpdateDto, Project>();
            CreateMap<ProjectCreationDto, Project>();
            CreateMap<ProjectContentDto, ProjectContent>();
            CreateMap<ProjectContent, ProjectContentDto>();
            CreateMap<List<Project>, List<ProjectDto>>();
            CreateMap<List<ProjectDto>, List<Project>>();
        }
    }
}