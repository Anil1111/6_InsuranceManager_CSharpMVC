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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace FinalWorkshop.Controllers
{
	public class CustomerController : Controller
	{
		private readonly EFCContext _context;

		public CustomerController(EFCContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index(string sortOrder)
		{
			ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
			var students = _context.Customers.ToAsyncEnumerable();
			switch (sortOrder)
			{
				case "name_desc":
					students = students.OrderByDescending(s => s.CompanyName);
					break;
				case "Date":
					students = students.OrderBy(s => s.DateAdded);
					break;
				case "date_desc":
					students = students.OrderByDescending(s => s.DateAdded);
					break;
				default:
					students = students.OrderBy(s => s.CompanyName);
					break;
			}
			return View(await students.ToList());
		}

		[HttpPost]
		public async Task<IActionResult> Index(string companyName, int? id)
		{
			var searchedCustomers = await _context.Customers.Where(x => x.CompanyName == companyName).ToListAsync();
			return View(searchedCustomers);
		}

		public async Task<IActionResult> SendEmail(int? id)
		{
			var customerModel = await _context.Customers.FindAsync(id);
			return View(customerModel);
		}

		[HttpPost]
		public IActionResult SendEmail(string receiver, string subject, string msgText)
		{
			var message = new MimeMessage();

			message.From.Add(new MailboxAddress("PL", "COMPANY-MAIL@gmail.com"));
			message.To.Add(new MailboxAddress(receiver, receiver));
			message.Subject = subject;

			message.Body = new TextPart("plain")
			{
				Text = msgText
			};

			using (var client = new SmtpClient())
			{
				client.Connect("smtp.gmail.com", 587, false); 
				client.Authenticate("COMPANY-MAIL@gmail.com", "PASSWORD");
				client.Send(message);
				client.Disconnect(true);
			}

			return RedirectToAction(nameof(Index));
		}

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
				_context.Add(customerModel);
				await _context.SaveChangesAsync();
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

			var customerModel = await _context.Customers.FindAsync(id);
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

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			try
			{
				var customerModel = await _context.Customers.FindAsync(id);
				_context.Customers.Remove(customerModel);

				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception e)
			{
				TempData["1"] = "Nie można usunąć klienta. Musisz usunąć wcześniej pojazdy/polisy do niego przypisane";
				return RedirectToAction(nameof(Index));
			}
		}

		private bool CustomerModelExists(int id)
		{
			return _context.Customers.Any(e => e.ID == id);
		}
	}
}
