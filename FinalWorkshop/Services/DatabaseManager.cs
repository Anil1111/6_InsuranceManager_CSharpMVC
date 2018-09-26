using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalWorkshop.Context;
using FinalWorkshop.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalWorkshop.Services
{
	public class DatabaseManager
	{
		private readonly EFCContext _context;

		public DatabaseManager(EFCContext context)
		{
			_context = context;
		}

		public bool CustomerModelExists(int id)
		{
			return _context.Customers.Any(e => e.ID == id);
		}

		public IAsyncEnumerable<CustomerModel> GetAllCustomers()
		{
			return _context.Customers.ToAsyncEnumerable();
		}

		public IAsyncEnumerable<CustomerModel> GetSpecificCustomer(string companyName)
		{
			var specificCustomer = _context.Customers.Where(x => x.CompanyName == companyName).ToAsyncEnumerable();

			return specificCustomer;
		}

		public Task<CustomerModel> GetSpecificCustomer(int? id)
		{
			var specificCustomer = _context.Customers.FindAsync(id);

			return specificCustomer;
		}

		public async Task AddCustomer(CustomerModel customerModel)
		{
			_context.Add(customerModel);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateCustomer(CustomerModel customerModel)
		{
			_context.Update(customerModel);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteCustomer(CustomerModel customerModel)
		{
			_context.Remove(customerModel);
			await _context.SaveChangesAsync();
		}

	}
}
