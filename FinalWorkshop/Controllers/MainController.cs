using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalWorkshop.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalWorkshop.Controllers
{
	public class MainController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
