using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models.Entities
{
    public class Project
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        public string OwnerId { get; set; }
        public string FreelancerId { get; set; }

        [Required]
        public ProjectContent ProjectContent { get; set; }
    }
}