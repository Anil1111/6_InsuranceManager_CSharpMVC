using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWorkshop.Models
{
	public class CustomerModel
	{
		public int ID { get; set; }
		[Required]
		public string CompanyName { get; set; }
		public DateTime DateAdded { get; set; } 
		public DateTime? DateUpdate { get; set; }
		[EmailAddress]
		public string Email { get; set; }
		public ICollection<VehicleModel> Vehicles { get; set; }
		public ICollection<VehiclePolicyModel> Policies { get; set; }

	}
}
