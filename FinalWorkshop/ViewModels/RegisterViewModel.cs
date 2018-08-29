using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWorkshop.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
		public string Login { get; set; }
		[Required]
		public string Password { get; set; }
		[Compare("Password")]
		public string RepeatPassword { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
