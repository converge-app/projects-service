using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.DataTransferObjects
{
    public class ProjectUpdateDto
    {
        [Required] public string Id { get; set; }

        [Required] public string OwnerId { get; set; }
        public string FreelancerId { get; set; }

        [Required] public ProjectContentDto ProjectContent { get; set; }
    }
}