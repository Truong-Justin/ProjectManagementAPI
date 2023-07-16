using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.Models
{
	public class Project
	{
		public int ProjectId { get; set; }

		[Required]
		public DateOnly StartDate { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public string Priority { get; set; }

		public int ProjectManagerId { get; set; }
	}
}

