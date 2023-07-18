using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.Models
{
	public abstract class Entity
	{
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}"), Required]
        public DateOnly Date { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Priority { get; set; }
    }
}

