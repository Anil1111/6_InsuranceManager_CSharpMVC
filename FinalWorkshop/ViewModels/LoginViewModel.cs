using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWorkshop.ViewModels
{
	public class LoginViewModel
	{
		[Required]
		public string Login { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]
		public bool RememberMe { get; set; }
	}
}
