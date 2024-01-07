using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.Models.People
{
    // Abstract class models the attributes of a person
    // that extend this class such as an Employee or
    // a Project Manager
    public abstract class Person
	{
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateOnly HireDate { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Zip { get; set; }

        [Required]
        public string Address { get; set; }
    }
}

