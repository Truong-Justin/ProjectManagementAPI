using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.Models
{
	// Models the attributes of Bug records
	// retrieved from the database
	public class Bug : Entity
	{
		public int BugId { get; set; }

		[Required]
		public string? Assignment { get; set; }

		//Foreign Key
		[Required]
		public int ProjectId { get; set; }
	}
}

