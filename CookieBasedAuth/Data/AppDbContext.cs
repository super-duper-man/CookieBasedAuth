﻿using CookieBasedAuth.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CookieBasedAuth.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        public DbSet<Employee> Employees { get; set; }
    }
}
