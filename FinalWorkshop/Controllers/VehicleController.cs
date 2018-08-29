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
	public class VehicleController : Controller
	{
		private readonly EFCContext _context;

		public VehicleController(EFCContext context)
		{
			_context = context;
		}

		// GET: Vehicle
		public async Task<IActionResult> Index()
		{
			var eFCContext = _context.Vehicles.Include(v => v.CustomerVehicle);
			return View(await eFCContext.ToListAsync());
		}
		public async Task<IActionResult> SpecificVehicle(int id)
		{
			var eFCContext = _context.Vehicles.Where(x => x.CustomerVehicle.ID == id);
			return View(await eFCContext.ToListAsync());
		}

		// GET: Vehicle/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var vehicleModel = await _context.Vehicles
				.Include(v => v.CustomerVehicle)
				.FirstOrDefaultAsync(m => m.ID == id);
			if (vehicleModel == null)
			{
				return NotFound();
			}

			return View(vehicleModel);
		}

		// GET: Vehicle/Create
		public IActionResult Create()
		{
			ViewData["CustomerModelID"] = new SelectList(_context.Customers, "ID", "CompanyName");
			return View();
		}

		// POST: Vehicle/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ID,RegistrationNumber,Type,CarBrand,CarModel,EngineCapacity,MaxLoad,MaxWeight,CustomerModelID")] VehicleModel vehicleModel)
		{
			if (ModelState.IsValid)
			{
				_context.Add(vehicleModel);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["CustomerModelID"] = new SelectList(_context.Customers, "ID", "ID", vehicleModel.CustomerModelID);
			return View(vehicleModel);
		}

		// GET: Vehicle/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var vehicleModel = await _context.Vehicles.FindAsync(id);
			if (vehicleModel == null)
			{
				return NotFound();
			}
			ViewData["CustomerModelID"] = new SelectList(_context.Customers, "ID", "ID", vehicleModel.CustomerModelID);
			return View(vehicleModel);
		}

		// POST: Vehicle/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,RegistrationNumber,Type,CarBrand,CarModel,EngineCapacity,MaxLoad,MaxWeight,CustomerModelID")] VehicleModel vehicleModel)
		{
			if (id != vehicleModel.ID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(vehicleModel);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!VehicleModelExists(vehicleModel.ID))
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
			ViewData["CustomerModelID"] = new SelectList(_context.Customers, "ID", "ID", vehicleModel.CustomerModelID);
			return View(vehicleModel);
		}

		// GET: Vehicle/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var vehicleModel = await _context.Vehicles
				.Include(v => v.CustomerVehicle)
				.FirstOrDefaultAsync(m => m.ID == id);
			if (vehicleModel == null)
			{
				return NotFound();
			}

			return View(vehicleModel);
		}

		// POST: Vehicle/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var vehicleModel = await _context.Vehicles.FindAsync(id);
			_context.Vehicles.Remove(vehicleModel);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool VehicleModelExists(int id)
		{
			return _context.Vehicles.Any(e => e.ID == id);
		}
	}
}
