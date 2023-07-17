using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.Models
{
	public class Project : Entity
	{
		public int ProjectId { get; set; }

		[Required]
		public string ProjectTitle { get; set; }

		public int ProjectManagerId { get; set; }
	}
}

