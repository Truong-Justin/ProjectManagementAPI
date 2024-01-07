using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.Models
{
    // Abstract class models the attributes of entities
    // that extend this class such as a Bug or Project
	public abstract class Entity
	{
        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Priority { get; set; }
    }
}

