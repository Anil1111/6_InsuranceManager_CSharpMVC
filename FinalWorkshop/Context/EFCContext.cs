using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalWorkshop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalWorkshop.Context
{
	public class EFCContext : IdentityDbContext
	{

		public DbSet<CustomerModel> Customers { get; set; }
		public DbSet<VehicleModel> Vehicles { get; set; }
		public DbSet<VehiclePolicyModel> VehiclePolicies { get; set; }
		public EFCContext(DbContextOptions options) : base(options)
		{
		}
	}
}
