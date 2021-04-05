using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Catalog.Context
{
    //В случае отсутствия открытого конструктора DbContext для применения миграций применяется IDesignTimeDbContextFactory
    public class DbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();

            // получаем конфигурацию из файла appsettings.json
            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            var config = builder.Build();


            // получаем строку подключения
            string connectionString = config.GetConnectionString("DefaultConnection");

            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;

            return new CatalogDbContext(options);
        }
    }
}
