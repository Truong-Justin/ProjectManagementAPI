using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.Models
{
    // Models the attributes of Project records
    // retrieved from the database
    public class Project : Entity
	{
		public int ProjectId { get; set; }

		[Required]
		public string ProjectTitle { get; set; }

		// Foreign key
		public int ProjectManagerId { get; set; }
	}
}

