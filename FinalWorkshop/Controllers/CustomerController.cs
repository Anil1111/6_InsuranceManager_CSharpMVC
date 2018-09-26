using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalWorkshop.Context;
using FinalWorkshop.Models;
using FinalWorkshop.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using MailService = FinalWorkshop.Services.MailService;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace FinalWorkshop.Controllers
{
	public class CustomerController : Controller
	{
		private readonly DatabaseManager _databaseManager;
		private readonly MailService _mailService;

		public CustomerController(DatabaseManager databaseManager, MailService mailService)
		{
			_databaseManager = databaseManager;
			_mailService = mailService;
		}

		public async Task<IActionResult> Index(string sortOrder)
		{
			ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

			var customers = _databaseManager.GetAllCustomers();

			switch (sortOrder)
			{
				case "name_desc":
					customers = customers.OrderByDescending(s => s.CompanyName);
					break;
				case "Date":
					customers = customers.OrderBy(s => s.DateAdded);
					break;
				case "date_desc":
					customers = customers.OrderByDescending(s => s.DateAdded);
					break;
				default:
					customers = customers.OrderBy(s => s.CompanyName);
					break;
			}

			return View(await customers.ToList());
		}

		[HttpPost]
		public async Task<IActionResult> Index(string companyName, int? id)
		{
			var searchedCustomers = _databaseManager.GetSpecificCustomer(companyName);

			return View(await searchedCustomers.ToList());
		}

		public async Task<IActionResult> SendEmail(int? id)
		{
			var customerModel = await _databaseManager.GetSpecificCustomer(id);

			return View(customerModel);
		}

		[HttpPost]
		public IActionResult SendEmail(string receiver, string subject, string msgText)
		{
			_mailService.SendEmail(receiver, subject, msgText);

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var customerModel = await _databaseManager.GetSpecificCustomer(id);

			if (customerModel == null)
			{
				return NotFound();
			}

			return View(customerModel);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ID,CompanyName,DateAdded,Email")] CustomerModel customerModel)
		{
			if (ModelState.IsValid)
			{
				_databaseManager.AddCustomer(customerModel);
				return RedirectToAction(nameof(Index));
			}
			return View(customerModel);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var customerModel = await _databaseManager.GetSpecificCustomer(id);

			if (customerModel == null)
			{
				return NotFound();
			}
			return View(customerModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,CompanyName,DateAdded,DateUpdate,Email")] CustomerModel customerModel)
		{
			if (id != customerModel.ID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_databaseManager.UpdateCustomer(customerModel);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!_databaseManager.CustomerModelExists(customerModel.ID))
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

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var customerModel = await _databaseManager.GetSpecificCustomer(id);

			if (customerModel == null)
			{
				return NotFound();
			}

			return View(customerModel);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			try
			{
				var customerModel = await _databaseManager.GetSpecificCustomer(id);

				_databaseManager.DeleteCustomer(customerModel);

				return RedirectToAction(nameof(Index));
			}
			catch (Exception e)
			{
				TempData["1"] = "Cannot Remove Client. Delete vehicles/policies which it has connected";

				return RedirectToAction(nameof(Index));
			}
		}
	}
}
