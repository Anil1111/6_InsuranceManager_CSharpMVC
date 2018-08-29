using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalWorkshop.Models
{
	public class VehicleModel
	{
		public int ID { get; set; }
		public string RegistrationNumber { get; set; }
		public string Type { get; set; }
		public string CarBrand { get; set; }
		public string CarModel { get; set; }

		[ForeignKey("CustomerVehicle")]
		public int CustomerModelID { get; set; }
		public CustomerModel CustomerVehicle { get; set; }

		public ICollection<VehiclePolicyModel> Policies { get; set; }
	}
}
