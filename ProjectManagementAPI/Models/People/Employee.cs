
namespace ProjectManagementAPI.Models.People
{
	// Models the attributes of Employee
	// records retrieved from the database
	public class Employee : Person
	{
		public int EmployeeId { get; set; }

		// Foreign key
		public int ProjectId { get; set; }
	}
}

