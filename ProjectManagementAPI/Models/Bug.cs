using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.Models
{
	public class Bug : Entity
	{
		public int BugId { get; set; }

		[Required]
		public string? Assignment { get; set; }

		[Required]
		public int ProjectId { get; set; }
	}
}

