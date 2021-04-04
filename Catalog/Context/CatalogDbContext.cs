using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Context
{
    class CatalogDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        :base(options)
        {
            Database.EnsureCreated();
        }

    }
}
