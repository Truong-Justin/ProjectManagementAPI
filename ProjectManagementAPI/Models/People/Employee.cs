using System.ComponentModel.DataAnnotations;


namespace ProjectManagementAPI.Models.People
{
	public class Employee : Person
	{
		public int EmployeeId { get; set; }

		// Foreign key
		public int ProjectId { get; set; }
	}
}

