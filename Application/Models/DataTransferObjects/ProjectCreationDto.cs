using System.ComponentModel.DataAnnotations;

namespace Application.Models.DataTransferObjects
{
    public class ProjectCreationDto
    {
                [Required]
                public string OwnerId { get; set; }
                public string FreelancerId { get; set; }
        
                [Required]
                public ProjectContentDto ProjectContent { get; set; }
    }
}