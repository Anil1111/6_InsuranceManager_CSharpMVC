using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWorkshop.Models
{
	public class VehiclePolicyModel
	{

		public int ID { get; set; }
		public string Risk { get; set; }
		[Required]
		public DateTime StartTime { get; set; }
		[Required]
		public DateTime EndTime { get; set; }

		[ForeignKey("VehicleVehiclePolicy")]
		public int? VehicleModelID { get; set; }
		public VehicleModel VehiclePolicyVehicle { get; set; }

		[ForeignKey("CustomerVehiclePolicy")]
		public int? CustomerModelID { get; set; }
		public CustomerModel CustomerVehiclePolicy { get; set; }
	}
}
