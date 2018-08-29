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
	public class CustomerController : Controller
	{
		private readonly EFCContext _context;

		public CustomerController(EFCContext context)
		{
			_context = context;
		}

		// GET: Customer
		public async Task<IActionResult> Index()
		{
			return View(await _context.Customers.ToListAsync());
		}

		// GET: Customer/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var customerModel = await _context.Customers
				.FirstOrDefaultAsync(m => m.ID == id);
			if (customerModel == null)
			{
				return NotFound();
			}

			return View(customerModel);
		}

		// GET: Customer/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Customer/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ID,CompanyName,DateAdded")] CustomerModel customerModel)
		{
			if (ModelState.IsValid)
			{
				_context.Add(customerModel);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(customerModel);
		}

		// GET: Customer/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var customerModel = await _context.Customers.FindAsync(id);
			if (customerModel == null)
			{
				return NotFound();
			}
			return View(customerModel);
		}

		// POST: Customer/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,CompanyName,DateAdded,DateUpdate")] CustomerModel customerModel)
		{
			if (id != customerModel.ID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(customerModel);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CustomerModelExists(customerModel.ID))
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
			return View(customerModel);
		}

		// GET: Customer/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var customerModel = await _context.Customers
				.FirstOrDefaultAsync(m => m.ID == id);
			if (customerModel == null)
			{
				return NotFound();
			}

			return View(customerModel);
		}

		// POST: Customer/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var customerModel = await _context.Customers.FindAsync(id);
			_context.Customers.Remove(customerModel);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool CustomerModelExists(int id)
		{
			return _context.Customers.Any(e => e.ID == id);
		}
	}
}
