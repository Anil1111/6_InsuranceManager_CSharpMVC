using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalWorkshop.Context;
using FinalWorkshop.Models;

namespace FinalWorkshop.Controllers
{
	public class VehiclePolicyController : Controller
	{
		private readonly EFCContext _context;

		public VehiclePolicyController(EFCContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index(string sortOrder)
		{
			ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

			var policies = _context.VehiclePolicies.Include(v => v.VehiclePolicyVehicle).Include(x => x.CustomerVehiclePolicy).ToAsyncEnumerable();

			switch (sortOrder)
			{
				case "name_desc":
					policies = policies.OrderByDescending(s => s.Risk);
					break;
				case "Date":
					policies = policies.OrderBy(s => s.StartTime);
					break;
				case "date_desc":
					policies = policies.OrderByDescending(s => s.StartTime);
					break;
				default:
					policies = policies.OrderBy(s => s.EndTime);
					break;
			}

			return View(await policies.ToList());
		}
		public async Task<IActionResult> SpecificPolicy(int id)
		{
			var eFCContext = _context.VehiclePolicies.Where(v => v.VehiclePolicyVehicle.ID == id);

			return View(await eFCContext.ToListAsync());
		}
		public async Task<IActionResult> SpecificPolicyFromCustomerId(int id)
		{
			ViewBag.CustomerName = _context.Customers.Single(x => x.ID == id).CompanyName;
			var eFCContext = _context.VehiclePolicies.Where(v => v.CustomerModelID == id).Include(v => v.VehiclePolicyVehicle).Include(x => x.CustomerVehiclePolicy);

			return View(await eFCContext.ToListAsync());
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var vehiclePolicyModel = await _context.VehiclePolicies
				.Include(v => v.VehiclePolicyVehicle)
				.FirstOrDefaultAsync(m => m.ID == id);
			if (vehiclePolicyModel == null)
			{
				return NotFound();
			}

			return View(vehiclePolicyModel);
		}

		public IActionResult Create()
		{
			ViewData["VehicleModelID"] = new SelectList(_context.Vehicles, "ID", "RegistrationNumber");
			ViewData["CustomerModelID"] = new SelectList(_context.Customers, "ID", "CompanyName");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ID,Risk,StartTime,EndTime,VehicleModelID,CustomerModelID")] VehiclePolicyModel vehiclePolicyModel)
		{
			if (ModelState.IsValid)
			{
				_context.Add(vehiclePolicyModel);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["VehicleModelID"] = new SelectList(_context.Vehicles, "ID", "ID", vehiclePolicyModel.VehicleModelID);
			ViewData["CustomerModelID"] = new SelectList(_context.Customers, "ID", "ID", vehiclePolicyModel.CustomerModelID);

			return View(vehiclePolicyModel);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var vehiclePolicyModel = await _context.VehiclePolicies.FindAsync(id);
			if (vehiclePolicyModel == null)
			{
				return NotFound();
			}

			ViewData["VehicleModelID"] = new SelectList(_context.Vehicles, "ID", "RegistrationNumber", vehiclePolicyModel.VehicleModelID);
			ViewData["CustomerModelID"] = new SelectList(_context.Customers, "ID", "CompanyName");

			return View(vehiclePolicyModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Risk,StartTime,EndTime,VehicleModelID,CustomerModelID")] VehiclePolicyModel vehiclePolicyModel)
		{
			if (id != vehiclePolicyModel.ID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(vehiclePolicyModel);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!VehiclePolicyModelExists(vehiclePolicyModel.ID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["VehicleModelID"] = new SelectList(_context.Vehicles, "ID", "ID", vehiclePolicyModel.VehicleModelID);
			ViewData["CustomerModelID"] = new SelectList(_context.Customers, "ID", "ID", vehiclePolicyModel.CustomerModelID);

			return View(vehiclePolicyModel);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var vehiclePolicyModel = await _context.VehiclePolicies
				.Include(v => v.VehiclePolicyVehicle)
				.FirstOrDefaultAsync(m => m.ID == id);
			if (vehiclePolicyModel == null)
			{
				return NotFound();
			}

			return View(vehiclePolicyModel);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var vehiclePolicyModel = await _context.VehiclePolicies.FindAsync(id);
			_context.VehiclePolicies.Remove(vehiclePolicyModel);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool VehiclePolicyModelExists(int id)
		{
			return _context.VehiclePolicies.Any(e => e.ID == id);
		}
	}
}
